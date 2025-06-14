using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Globalping;

public class MeasurementRequest {
    [JsonPropertyName("type")]
    public MeasurementType Type { get; set; }

    [JsonPropertyName("target")]
    public string Target { get; set; } = string.Empty;

    [JsonPropertyName("inProgressUpdates")]
    public bool InProgressUpdates { get; set; } = false;

    [JsonPropertyName("locations")]
    public List<LocationRequest>? Locations { get; set; }

    [JsonPropertyName("limit")]
    public int Limit { get; set; } = 1;

    [JsonPropertyName("measurementOptions")]
    public IMeasurementOptions? MeasurementOptions { get; set; }
}

