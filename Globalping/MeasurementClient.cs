using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

namespace Globalping;

public class MeasurementClient {
    private readonly HttpClient _httpClient;

    public MeasurementClient(HttpClient httpClient) {
        _httpClient = httpClient;
    }

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
            measurementResponse = await JsonSerializer.DeserializeAsync<MeasurementResponse>(stream, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }).ConfigureAwait(false);

            if (measurementResponse?.Status == "in-progress") {
                await Task.Delay(500).ConfigureAwait(false);
            }
        } while (measurementResponse != null && measurementResponse.Status == "in-progress");

        return measurementResponse;
    }

    public MeasurementResponse? GetMeasurementById(string id) =>
        GetMeasurementByIdAsync(id).GetAwaiter().GetResult();
}
