using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Globalping;

/// <summary>
/// Parameters used to select probes by location.
/// </summary>
public class LocationRequest {
    /// <summary>Desired continent for the probe.</summary>
    [JsonPropertyName("continent")]
    public ContinentCode? Continent { get; set; }

    /// <summary>Specific region within the continent.</summary>
    [JsonPropertyName("region")]
    public RegionName? Region { get; set; }

    /// <summary>ISO country code.</summary>
    [JsonPropertyName("country")]
    public CountryCode? Country { get; set; }

    /// <summary>State or province within the country.</summary>
    [JsonPropertyName("state")]
    public string? State { get; set; }

    /// <summary>City name.</summary>
    [JsonPropertyName("city")]
    public string? City { get; set; }

    /// <summary>Maximum number of probes to use.</summary>
    [JsonPropertyName("limit")]
    public int? Limit { get; set; } // Optional limit for probes

    /// <summary>Optional "magic" location identifier.</summary>
    [JsonPropertyName("magic")]
    public string? Magic { get; set; } // For "magic" location requests

    /// <summary>Filter by autonomous system number.</summary>
    [JsonPropertyName("asn")]
    public int? Asn { get; set; }

    /// <summary>Filter by network name.</summary>
    [JsonPropertyName("network")]
    public string? Network { get; set; }

    /// <summary>Optional probe tags.</summary>
    [JsonPropertyName("tags")]
    public List<string>? Tags { get; set; }
}