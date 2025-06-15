using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Globalping;

public class LocationRequest {
    [JsonPropertyName("continent")]
    public ContinentCode? Continent { get; set; }

    [JsonPropertyName("region")]
    public RegionName? Region { get; set; }

    [JsonPropertyName("country")]
    public string? Country { get; set; }

    [JsonPropertyName("state")]
    public string? State { get; set; }

    [JsonPropertyName("city")]
    public string? City { get; set; }

    [JsonPropertyName("limit")]
    public int? Limit { get; set; } // Optional limit for probes

    [JsonPropertyName("magic")]
    public string? Magic { get; set; } // For "magic" location requests

    [JsonPropertyName("asn")]
    public int? Asn { get; set; }

    [JsonPropertyName("network")]
    public string? Network { get; set; }

    [JsonPropertyName("tags")]
    public List<string>? Tags { get; set; }
}