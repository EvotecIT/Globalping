using System.Text.Json.Serialization;

namespace Globalping;

/// <summary>
/// Response returned after creating a measurement request.
/// </summary>
public class CreateMeasurementResponse {
    /// <summary>Identifier of the newly created measurement.</summary>
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    /// <summary>Number of probes involved in the measurement.</summary>
    [JsonPropertyName("probesCount")]
    public int ProbesCount { get; set; }
}
