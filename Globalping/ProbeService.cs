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

    public ProbeService(HttpClient httpClient, string? apiKey = null)
    {
        _httpClient = httpClient;
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

    /// <summary>
    /// Retrieves the list of currently online probes.
    /// </summary>
    /// <returns>Collection of available probes.</returns>
    public async Task<List<Probes>> GetOnlineProbesAsync() {
        var response = await _httpClient.GetAsync("https://api.globalping.io/v1/probes").ConfigureAwait(false);
        response.EnsureSuccessStatusCode();
        var stream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
        return await JsonSerializer.DeserializeAsync<List<Probes>>(stream, _jsonOptions).ConfigureAwait(false) ?? new List<Probes>();
    }

    /// <summary>
    /// Synchronous wrapper for <see cref="GetOnlineProbesAsync"/>.
    /// </summary>
    public List<Probes> GetOnlineProbes() =>
        GetOnlineProbesAsync().GetAwaiter().GetResult();

    /// <summary>
    /// Creates a measurement request on the Globalping service.
    /// </summary>
    /// <param name="measurementRequest">Request payload describing the measurement.</param>
    /// <returns>Identifier of the created measurement.</returns>
    public async Task<string> CreateMeasurementAsync(MeasurementRequest measurementRequest) {
        var requestUri = "https://api.globalping.io/v1/measurements";
        var requestContent = new StringContent(JsonSerializer.Serialize(measurementRequest, _jsonOptions), Encoding.UTF8, "application/json");

        if (measurementRequest.InProgressUpdates) {
            _httpClient.DefaultRequestHeaders.Remove("Prefer");
            _httpClient.DefaultRequestHeaders.Add("Prefer", "respond-async, wait=150");
        }

        var response = await _httpClient.PostAsync(requestUri, requestContent).ConfigureAwait(false);
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<MeasurementResponse>(_jsonOptions).ConfigureAwait(false);
        return result?.Id ?? string.Empty;
    }

    /// <summary>
    /// Synchronous wrapper for <see cref="CreateMeasurementAsync"/>.
    /// </summary>
    public string CreateMeasurement(MeasurementRequest measurementRequest) =>
        CreateMeasurementAsync(measurementRequest).GetAwaiter().GetResult();
}
