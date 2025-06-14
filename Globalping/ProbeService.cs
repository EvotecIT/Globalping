using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Globalping;

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
        if (!string.IsNullOrEmpty(apiKey))
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
        }
    }

    public async Task<List<Probes>> GetOnlineProbesAsync() {
        var response = await _httpClient.GetAsync("https://api.globalping.io/v1/probes").ConfigureAwait(false);
        response.EnsureSuccessStatusCode();
        var stream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
        return await JsonSerializer.DeserializeAsync<List<Probes>>(stream, _jsonOptions).ConfigureAwait(false) ?? new List<Probes>();
    }

    public List<Probes> GetOnlineProbes() =>
        GetOnlineProbesAsync().GetAwaiter().GetResult();

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

    public string CreateMeasurement(MeasurementRequest measurementRequest) =>
        CreateMeasurementAsync(measurementRequest).GetAwaiter().GetResult();
}
