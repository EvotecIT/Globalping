using System.Net.Http;
using System.Threading.Tasks;
using Globalping.Models;
using System.Threading;

namespace Globalping;

public class MeasurementClient {
    private readonly HttpClient _httpClient;

    public MeasurementClient(HttpClient httpClient) {
        _httpClient = httpClient;
    }

    public async Task<MeasurementResponse> GetMeasurementByIdAsync(string id) {
        MeasurementResponse measurementResponse = null;
        string url = $"https://api.example.com/v1/measurements/{id}";

        do {
            var response = await _httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode) {
                measurementResponse = await response.Content.ReadAsAsync<MeasurementResponse>();

                if (measurementResponse.Status == "in-progress") {
                    // Wait 500ms as per guidelines before polling again
                    Thread.Sleep(500);
                }
            } else {
                // Handle error response
                break;
            }
        } while (measurementResponse.Status == "in-progress");

        return measurementResponse;
    }
}
