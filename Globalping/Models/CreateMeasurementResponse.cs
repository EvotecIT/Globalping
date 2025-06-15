using System.Text.Json.Serialization;

namespace Globalping;

public class CreateMeasurementResponse {
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("probesCount")]
    public int ProbesCount { get; set; }
}
