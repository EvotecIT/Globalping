using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Globalping;

/// <summary>
/// Represents a probe available for running measurements.
/// </summary>
public class Probes {
    /// <summary>Probe software version.</summary>
    [JsonPropertyName("version")]
    public string Version { get; set; } = string.Empty;

    /// <summary>Geographical information about the probe.</summary>
    [JsonPropertyName("location")]
    public Location? Location { get; set; }

    /// <summary>Tags describing the probe.</summary>
    [JsonPropertyName("tags")]
    public List<string>? Tags { get; set; }

    /// <summary>DNS resolvers configured for the probe.</summary>
    [JsonPropertyName("resolvers")]
    public List<string>? Resolvers { get; set; }
}
