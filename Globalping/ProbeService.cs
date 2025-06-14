using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Globalping;

public class ProbeService {
    private readonly HttpClient _httpClient;

    public ProbeService(HttpClient httpClient) {
        _httpClient = httpClient;
    }

    public async Task<List<Probes>> GetOnlineProbesAsync() {
        var response = await _httpClient.GetAsync("https://api.globalping.io/v1/probes").ConfigureAwait(false);
        response.EnsureSuccessStatusCode();
        var stream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
        return await JsonSerializer.DeserializeAsync<List<Probes>>(stream, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }).ConfigureAwait(false) ?? new List<Probes>();
    }

    public async Task<string> CreateMeasurementAsync(MeasurementRequest measurementRequest, bool inProgressUpdates = false) {
        var requestUri = "https://api.globalping.io/v1/measurements";
        var requestContent = new StringContent(JsonSerializer.Serialize(measurementRequest), Encoding.UTF8, "application/json");

        if (inProgressUpdates) {
            _httpClient.DefaultRequestHeaders.Add("Prefer", "respond-async, wait=150");
        }

        var response = await _httpClient.PostAsync(requestUri, requestContent).ConfigureAwait(false);
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<MeasurementResponse>(new JsonSerializerOptions { PropertyNameCaseInsensitive = true }).ConfigureAwait(false);
        return result?.Id ?? string.Empty;
    }
}
