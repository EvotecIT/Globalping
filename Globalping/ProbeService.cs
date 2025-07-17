using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
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
            RequestCost = TryGetInt(headers, "X-Request-Cost"),
            RetryAfter = TryGetInt(headers, "Retry-After"),
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
            // ignore parse failures
        }

        throw new GlobalpingApiException((int)response.StatusCode, error?.Error ?? new ErrorDetails(), LastResponseInfo);
    }

    private static void NormalizeHttpRequest(MeasurementRequest request)
    {
        if (request.Type != MeasurementType.Http)
        {
            return;
        }

        if (!Uri.TryCreate(request.Target, UriKind.Absolute, out var uri))
        {
            return;
        }

        var options = request.MeasurementOptions as HttpOptions ?? new HttpOptions();
        request.MeasurementOptions ??= options;

        var reqOpts = options.Request ??= new HttpRequestOptions();
        if (string.IsNullOrEmpty(reqOpts.Path))
        {
            reqOpts.Path = string.IsNullOrEmpty(uri.AbsolutePath) ? "/" : uri.AbsolutePath;
        }
        if (string.IsNullOrEmpty(reqOpts.Query) && !string.IsNullOrEmpty(uri.Query))
        {
            reqOpts.Query = uri.Query.TrimStart('?');
        }

        if (options.Protocol == HttpProtocol.HTTPS && uri.Scheme.Equals("http", StringComparison.OrdinalIgnoreCase))
        {
            options.Protocol = HttpProtocol.HTTP;
        }
        else if (options.Protocol == HttpProtocol.HTTPS && uri.Scheme.Equals("https", StringComparison.OrdinalIgnoreCase))
        {
            options.Protocol = HttpProtocol.HTTPS;
        }

        if (uri.IsDefaultPort)
        {
            options.Port = options.Protocol == HttpProtocol.HTTPS ? 443 : 80;
        }
        else
        {
            options.Port = uri.Port;
        }

        request.Target = uri.Host;
    }

    /// <summary>
    /// Retrieves the list of currently online probes.
    /// </summary>
    /// <returns>Collection of available probes.</returns>
    public async Task<List<Probes>> GetOnlineProbesAsync(CancellationToken cancellationToken = default) {
        var response = await _httpClient.GetAsync("https://api.globalping.io/v1/probes", cancellationToken).ConfigureAwait(false);
        await EnsureSuccessOrThrow(response).ConfigureAwait(false);
#if NETSTANDARD2_0 || NET472
        var stream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
#else
        var stream = await response.Content.ReadAsStreamAsync(cancellationToken).ConfigureAwait(false);
#endif
        return await JsonSerializer.DeserializeAsync<List<Probes>>(stream, _jsonOptions, cancellationToken).ConfigureAwait(false) ?? new List<Probes>();
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
    public async Task<Limits?> GetLimitsAsync(CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.GetAsync("https://api.globalping.io/v1/limits", cancellationToken).ConfigureAwait(false);
        await EnsureSuccessOrThrow(response).ConfigureAwait(false);
#if NETSTANDARD2_0 || NET472
        var stream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
#else
        var stream = await response.Content.ReadAsStreamAsync(cancellationToken).ConfigureAwait(false);
#endif
        return await JsonSerializer.DeserializeAsync<Limits>(stream, _jsonOptions, cancellationToken).ConfigureAwait(false);
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
    /// <returns>Information about the created measurement.</returns>
    public async Task<CreateMeasurementResponse> CreateMeasurementAsync(
        MeasurementRequest measurementRequest,
        int waitTime = 150,
        CancellationToken cancellationToken = default) {
        NormalizeHttpRequest(measurementRequest);

        var requestUri = "https://api.globalping.io/v1/measurements";
        var requestContent = new StringContent(
            JsonSerializer.Serialize(measurementRequest, _jsonOptions),
            Encoding.UTF8,
            "application/json");

        using var request = new HttpRequestMessage(HttpMethod.Post, requestUri)
        {
            Content = requestContent
        };

        if (measurementRequest.InProgressUpdates)
        {
            request.Headers.Add("Prefer", $"respond-async, wait={waitTime}");
        }

        var response = await _httpClient.SendAsync(request, cancellationToken).ConfigureAwait(false);
        await EnsureSuccessOrThrow(response).ConfigureAwait(false);
        var result = await response.Content.ReadFromJsonAsync<CreateMeasurementResponse>(_jsonOptions, cancellationToken).ConfigureAwait(false);
        return result ?? new CreateMeasurementResponse();
    }

    /// <summary>
    /// Synchronous wrapper for <see cref="CreateMeasurementAsync"/>.
    /// </summary>
    public CreateMeasurementResponse CreateMeasurement(
        MeasurementRequest measurementRequest,
        int waitTime = 150) =>
        CreateMeasurementAsync(measurementRequest, waitTime).GetAwaiter().GetResult();
}
