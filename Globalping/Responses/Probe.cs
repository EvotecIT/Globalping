using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Globalping;

public class Probe {
    [JsonPropertyName("continent")]
    public string Continent { get; set; } = string.Empty;

    [JsonPropertyName("region")]
    public string Region { get; set; } = string.Empty;

    [JsonPropertyName("country")]
    public string Country { get; set; } = string.Empty;

    [JsonPropertyName("state")]
    public string State { get; set; } = string.Empty;

    [JsonPropertyName("city")]
    public string City { get; set; } = string.Empty;

    [JsonPropertyName("asn")]
    public int Asn { get; set; }

    [JsonPropertyName("longitude")]
    public double Longitude { get; set; }

    [JsonPropertyName("latitude")]
    public double Latitude { get; set; }

    [JsonPropertyName("network")]
    public string Network { get; set; } = string.Empty;

    [JsonPropertyName("tags")]
    public List<string> Tags { get; set; } = new();

    [JsonPropertyName("resolvers")]
    public List<string> Resolvers { get; set; } = new();

    [JsonPropertyName("version")]
    public string Version { get; set; } = string.Empty;
}
