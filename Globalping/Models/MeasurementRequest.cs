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

    [JsonIgnore]
    public List<LocationRequest>? Locations { get; set; }

    [JsonIgnore]
    public string? ReuseLocationsFromId { get; set; }

    [JsonPropertyName("locations")]
    public object? SerializedLocations =>
        (object?)ReuseLocationsFromId ?? Locations;

    [JsonPropertyName("limit")]
    public int? Limit { get; set; }

    [JsonPropertyName("measurementOptions")]
    public IMeasurementOptions? MeasurementOptions { get; set; }
}

