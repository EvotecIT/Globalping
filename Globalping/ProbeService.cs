using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Globalping {
    public class ProbeService {
        private readonly HttpClient _httpClient;

        public ProbeService(HttpClient httpClient) {
            _httpClient = httpClient;
        }

        public async Task<List<Probes>> GetOnlineProbesAsync() {
            var response = await _httpClient.GetAsync("https://api.globalping.io/v1/probes");
            if (response.IsSuccessStatusCode) {
                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<Probes>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            } else {
                // Handle non-successful response
                throw new HttpRequestException($"Failed to fetch probes. Status code: {response.StatusCode}");
            }
        }

        public async Task<string> CreateMeasurementAsync(MeasurementRequest measurementRequest, bool inProgressUpdates = false) {
            var requestUri = "https://api.globalping.io/v1/measurements";
            var requestContent = new StringContent(JsonSerializer.Serialize(measurementRequest), Encoding.UTF8, "application/json");

            // Optionally handle interactive mode
            if (inProgressUpdates) {
                _httpClient.DefaultRequestHeaders.Add("Prefer", "respond-async, wait=150");
            }

            var response = await _httpClient.PostAsync(requestUri, requestContent);
            if (response.IsSuccessStatusCode) {
                // Assuming the Location header contains the URL to retrieve the measurement's current state
                return response.Headers.Location?.ToString() ?? string.Empty;
            } else {
                // Handle non-successful response
                throw new HttpRequestException($"Failed to create measurement. Status code: {response.StatusCode}");
            }
        }
    }
}
