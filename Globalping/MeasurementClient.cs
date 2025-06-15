using System;
using System.Net;
using System.Net.Http;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Globalping;

/// <summary>
/// Client used to retrieve measurement results from the Globalping API.
/// </summary>
public class MeasurementClient {
    private readonly HttpClient _httpClient;
    private readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
    };

    public ApiUsageInfo LastResponseInfo { get; private set; } = new();
    public MeasurementResponse? LastMeasurement { get; private set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="MeasurementClient"/> class.
    /// </summary>
    /// <param name="httpClient">HTTP client used for requests.</param>
    /// <param name="apiKey">Optional API key for authenticated calls.</param>
    public MeasurementClient(HttpClient httpClient, string? apiKey = null) {
        _httpClient = httpClient;
        _jsonOptions.Converters.Add(new JsonStringEnumConverter<MeasurementStatus>(JsonNamingPolicy.KebabCaseLower));
        _jsonOptions.Converters.Add(new JsonStringEnumConverter<TestStatus>(JsonNamingPolicy.KebabCaseLower));
        _jsonOptions.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));

        if (!_httpClient.DefaultRequestHeaders.UserAgent.Any()) {
            _httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("Globalping.Net/1.0 (+https://github.com/EvotecIT/Globalping)");
        }

        if (!_httpClient.DefaultRequestHeaders.AcceptEncoding.Any()) {
            if (Enum.TryParse("Brotli", out DecompressionMethods _)) {
                _httpClient.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("br"));
            } else {
                _httpClient.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("gzip"));
            }
        }

        if (!string.IsNullOrEmpty(apiKey)) {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
        }
    }

    private void UpdateUsageInfo(HttpResponseMessage response)
    {
        var headers = response.Headers;
        LastResponseInfo = new ApiUsageInfo
        {
            RateLimitLimit = TryGetInt(headers, "X-RateLimit-Limit"),
            RateLimitConsumed = TryGetInt(headers, "X-RateLimit-Consumed"),
            RateLimitRemaining = TryGetInt(headers, "X-RateLimit-Remaining"),
            RateLimitReset = TryGetLong(headers, "X-RateLimit-Reset"),
            CreditsConsumed = TryGetInt(headers, "X-Credits-Consumed"),
            CreditsRemaining = TryGetInt(headers, "X-Credits-Remaining"),
            RequestCost = TryGetInt(headers, "X-Request-Cost"),
            ETag = headers.ETag?.Tag
        };
    }

    private static int? TryGetInt(HttpResponseHeaders headers, string name)
    {
        if (headers.TryGetValues(name, out var values) && int.TryParse(values.FirstOrDefault(), out var v))
        {
            return v;
        }
        return null;
    }

    private static long? TryGetLong(HttpResponseHeaders headers, string name)
    {
        if (headers.TryGetValues(name, out var values) && long.TryParse(values.FirstOrDefault(), out var v))
        {
            return v;
        }
        return null;
    }

    private async Task EnsureSuccessOrThrow(HttpResponseMessage response)
    {
        UpdateUsageInfo(response);
        if (response.IsSuccessStatusCode)
        {
            return;
        }

        ErrorResponse? error = null;
        try
        {
            var stream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
            error = await JsonSerializer.DeserializeAsync<ErrorResponse>(stream, _jsonOptions).ConfigureAwait(false);
        }
        catch
        {
            // Ignore parse failures
        }

        throw new GlobalpingApiException((int)response.StatusCode, error?.Error ?? new ErrorDetails(), LastResponseInfo);
    }

    /// <summary>
    /// Retrieves a measurement result by its identifier.
    /// </summary>
    /// <param name="id">Unique measurement identifier.</param>
    /// <returns>The full measurement response or <c>null</c> if not found.</returns>
    public async Task<MeasurementResponse?> GetMeasurementByIdAsync(string id, string? etag = null) {
        if (string.IsNullOrWhiteSpace(id)) {
            throw new ArgumentException("Measurement id cannot be empty", nameof(id));
        }

        var url = $"https://api.globalping.io/v1/measurements/{id}";
        MeasurementResponse? measurementResponse;

        do {
            using var request = new HttpRequestMessage(HttpMethod.Get, url);
            if (!string.IsNullOrEmpty(etag)) {
                request.Headers.IfNoneMatch.ParseAdd(etag);
            }

            var response = await _httpClient.SendAsync(request).ConfigureAwait(false);

            if (response.StatusCode == HttpStatusCode.NotModified) {
                UpdateUsageInfo(response);
                return LastMeasurement;
            }

            await EnsureSuccessOrThrow(response).ConfigureAwait(false);
            var stream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
            measurementResponse = await JsonSerializer.DeserializeAsync<MeasurementResponse>(stream, _jsonOptions).ConfigureAwait(false);
            if (measurementResponse?.Results != null)
            {
                foreach (var r in measurementResponse.Results)
                {
                    r.Target = measurementResponse.Target;
                }
            }

            LastMeasurement = measurementResponse;

            if (measurementResponse?.Status == MeasurementStatus.InProgress) {
                await Task.Delay(500).ConfigureAwait(false);
            }
        } while (measurementResponse != null && measurementResponse.Status == MeasurementStatus.InProgress);

        return measurementResponse;
    }

    /// <summary>
    /// Synchronous wrapper for <see cref="GetMeasurementByIdAsync"/>.
    /// </summary>
    public MeasurementResponse? GetMeasurementById(string id, string? etag = null) =>
        GetMeasurementByIdAsync(id, etag).GetAwaiter().GetResult();
}
