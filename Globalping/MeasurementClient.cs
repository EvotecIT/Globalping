using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Globalping;

public class MeasurementClient {
    private readonly HttpClient _httpClient;
    private readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
    };

    public MeasurementClient(HttpClient httpClient, string? apiKey = null) {
        _httpClient = httpClient;
        _jsonOptions.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
        if (!string.IsNullOrEmpty(apiKey)) {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
        }
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

    public MeasurementResponse? GetMeasurementById(string id) =>
        GetMeasurementByIdAsync(id).GetAwaiter().GetResult();
}
