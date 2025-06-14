using System.Text.Json.Serialization;

namespace Globalping;

public class LocationRequest {
    [JsonPropertyName("country")]
    public string? Country { get; set; }

    [JsonPropertyName("limit")]
    public int? Limit { get; set; } // Optional limit for probes

    [JsonPropertyName("magic")]
    public string? Magic { get; set; } // For "magic" location requests
}