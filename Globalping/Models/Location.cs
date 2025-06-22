using System.Text.Json.Serialization;
namespace Globalping;

/// <summary>
/// Represents geographical information for a probe.
/// </summary>
public class Location {
    /// <summary>Continent in which the probe resides.</summary>
    [JsonPropertyName("continent")]
    public string Continent { get; set; } = string.Empty;

    /// <summary>Region such as a world sub-region or state.</summary>
    [JsonPropertyName("region")]
    public string Region { get; set; } = string.Empty;

    /// <summary>Country name.</summary>
    [JsonPropertyName("country")]
    public string Country { get; set; } = string.Empty;

    /// <summary>State or province.</summary>
    [JsonPropertyName("state")]
    public string State { get; set; } = string.Empty;

    /// <summary>City name.</summary>
    [JsonPropertyName("city")]
    public string City { get; set; } = string.Empty;

    /// <summary>Autonomous system number.</summary>
    [JsonPropertyName("asn")]
    public int Asn { get; set; }

    /// <summary>Provider network name.</summary>
    [JsonPropertyName("network")]
    public string Network { get; set; } = string.Empty;

    /// <summary>Latitude coordinate.</summary>
    [JsonPropertyName("latitude")]
    public double Latitude { get; set; }

    /// <summary>Longitude coordinate.</summary>
    [JsonPropertyName("longitude")]
    public double Longitude { get; set; }
}

