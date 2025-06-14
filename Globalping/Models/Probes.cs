using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Globalping;

public class Probes {
    [JsonPropertyName("version")]
    public string Version { get; set; } = string.Empty;

    [JsonPropertyName("location")]
    public Location? Location { get; set; }

    [JsonPropertyName("tags")]
    public List<string>? Tags { get; set; }

    [JsonPropertyName("resolvers")]
    public List<string>? Resolvers { get; set; }
}
