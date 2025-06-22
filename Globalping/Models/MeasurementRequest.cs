using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Globalping;

/// <summary>
/// Request payload for creating a measurement.
/// </summary>
public class MeasurementRequest {
    /// <summary>Type of measurement to execute.</summary>
    [JsonPropertyName("type")]
    public MeasurementType Type { get; set; }

    /// <summary>Target host or address.</summary>
    [JsonPropertyName("target")]
    public string Target { get; set; } = string.Empty;

    /// <summary>Whether to receive updates while running.</summary>
    [JsonPropertyName("inProgressUpdates")]
    public bool InProgressUpdates { get; set; } = false;

    /// <summary>Explicit probe locations.</summary>
    [JsonIgnore]
    public List<LocationRequest>? Locations { get; set; }

    /// <summary>Reuse probe selection from another measurement.</summary>
    [JsonIgnore]
    public string? ReuseLocationsFromId { get; set; }

    [JsonPropertyName("locations")]
    public object? SerializedLocations =>
        (object?)ReuseLocationsFromId ?? Locations;

    /// <summary>Maximum number of probes to select.</summary>
    [JsonPropertyName("limit")]
    public int? Limit { get; set; }

    /// <summary>Measurement specific options.</summary>
    [JsonPropertyName("measurementOptions")]
    public IMeasurementOptions? MeasurementOptions { get; set; }
}

