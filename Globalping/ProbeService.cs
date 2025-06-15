using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Linq;

namespace Globalping;

/// <summary>
/// Provides simple access to the Globalping API for probe discovery and
/// measurement management.
/// </summary>
public class ProbeService {
    private readonly HttpClient _httpClient;
    private readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
    };

    public ApiUsageInfo LastResponseInfo { get; private set; } = new();

    public ProbeService(HttpClient httpClient, string? apiKey = null)
    {
        _httpClient = httpClient;
        _jsonOptions.Converters.Add(new JsonStringEnumConverter<MeasurementStatus>(JsonNamingPolicy.KebabCaseLower));
        _jsonOptions.Converters.Add(new JsonStringEnumConverter<TestStatus>(JsonNamingPolicy.KebabCaseLower));
        _jsonOptions.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));

        if (!_httpClient.DefaultRequestHeaders.UserAgent.Any())
        {
            _httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("Globalping.Net/1.0 (+https://github.com/EvotecIT/Globalping)");
        }

        if (!_httpClient.DefaultRequestHeaders.AcceptEncoding.Any())
        {
            if (Enum.TryParse("Brotli", out DecompressionMethods _))
            {
                _httpClient.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("br"));
            }
            else
            {
                _httpClient.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("gzip"));
            }
        }

        if (!string.IsNullOrEmpty(apiKey))
        {
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
            RequestCost = TryGetInt(headers, "X-Request-Cost")
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
            // ignore parse failures
        }

        throw new GlobalpingApiException((int)response.StatusCode, error?.Error ?? new ErrorDetails(), LastResponseInfo);
    }

    /// <summary>
    /// Retrieves the list of currently online probes.
    /// </summary>
    /// <returns>Collection of available probes.</returns>
    public async Task<List<Probes>> GetOnlineProbesAsync() {
        var response = await _httpClient.GetAsync("https://api.globalping.io/v1/probes").ConfigureAwait(false);
        await EnsureSuccessOrThrow(response).ConfigureAwait(false);
        var stream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
        return await JsonSerializer.DeserializeAsync<List<Probes>>(stream, _jsonOptions).ConfigureAwait(false) ?? new List<Probes>();
    }

    /// <summary>
    /// Synchronous wrapper for <see cref="GetOnlineProbesAsync"/>.
    /// </summary>
    public List<Probes> GetOnlineProbes() =>
        GetOnlineProbesAsync().GetAwaiter().GetResult();

    /// <summary>
    /// Retrieves API usage limits for the current caller.
    /// </summary>
    /// <returns>Object describing remaining rate limits.</returns>
    public async Task<Limits?> GetLimitsAsync()
    {
        var response = await _httpClient.GetAsync("https://api.globalping.io/v1/limits").ConfigureAwait(false);
        await EnsureSuccessOrThrow(response).ConfigureAwait(false);
        var stream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
        return await JsonSerializer.DeserializeAsync<Limits>(stream, _jsonOptions).ConfigureAwait(false);
    }

    /// <summary>
    /// Synchronous wrapper for <see cref="GetLimitsAsync"/>.
    /// </summary>
    public Limits? GetLimits() =>
        GetLimitsAsync().GetAwaiter().GetResult();

    /// <summary>
    /// Creates a measurement request on the Globalping service.
    /// </summary>
    /// <param name="measurementRequest">Request payload describing the measurement.</param>
    /// <returns>Identifier of the created measurement.</returns>
    public async Task<string> CreateMeasurementAsync(
        MeasurementRequest measurementRequest,
        int waitTime = 150) {
        var requestUri = "https://api.globalping.io/v1/measurements";
        var requestContent = new StringContent(JsonSerializer.Serialize(measurementRequest, _jsonOptions), Encoding.UTF8, "application/json");

        _httpClient.DefaultRequestHeaders.Remove("Prefer");
        if (measurementRequest.InProgressUpdates) {
            _httpClient.DefaultRequestHeaders.Add("Prefer", $"respond-async, wait={waitTime}");
        }

        var response = await _httpClient.PostAsync(requestUri, requestContent).ConfigureAwait(false);
        await EnsureSuccessOrThrow(response).ConfigureAwait(false);
        var result = await response.Content.ReadFromJsonAsync<MeasurementResponse>(_jsonOptions).ConfigureAwait(false);
        return result?.Id ?? string.Empty;
    }

    /// <summary>
    /// Synchronous wrapper for <see cref="CreateMeasurementAsync"/>.
    /// </summary>
    public string CreateMeasurement(
        MeasurementRequest measurementRequest,
        int waitTime = 150) =>
        CreateMeasurementAsync(measurementRequest, waitTime).GetAwaiter().GetResult();
}
