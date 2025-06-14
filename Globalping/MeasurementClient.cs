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

    /// <summary>
    /// Initializes a new instance of the <see cref="MeasurementClient"/> class.
    /// </summary>
    /// <param name="httpClient">HTTP client used for requests.</param>
    /// <param name="apiKey">Optional API key for authenticated calls.</param>
    public MeasurementClient(HttpClient httpClient, string? apiKey = null) {
        _httpClient = httpClient;
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

    /// <summary>
    /// Retrieves a measurement result by its identifier.
    /// </summary>
    /// <param name="id">Unique measurement identifier.</param>
    /// <returns>The full measurement response or <c>null</c> if not found.</returns>
    public async Task<MeasurementResponse?> GetMeasurementByIdAsync(string id) {
        if (string.IsNullOrWhiteSpace(id)) {
            throw new ArgumentException("Measurement id cannot be empty", nameof(id));
        }

        var url = $"https://api.globalping.io/v1/measurements/{id}";
        MeasurementResponse? measurementResponse;

        do {
            var response = await _httpClient.GetAsync(url).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            var stream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
            measurementResponse = await JsonSerializer.DeserializeAsync<MeasurementResponse>(stream, _jsonOptions).ConfigureAwait(false);
            if (measurementResponse?.Results != null)
            {
                foreach (var r in measurementResponse.Results)
                {
                    r.Target = measurementResponse.Target;
                }
            }

            if (measurementResponse?.Status == "in-progress") {
                await Task.Delay(500).ConfigureAwait(false);
            }
        } while (measurementResponse != null && measurementResponse.Status == "in-progress");

        return measurementResponse;
    }

    /// <summary>
    /// Synchronous wrapper for <see cref="GetMeasurementByIdAsync"/>.
    /// </summary>
    public MeasurementResponse? GetMeasurementById(string id) =>
        GetMeasurementByIdAsync(id).GetAwaiter().GetResult();
}
