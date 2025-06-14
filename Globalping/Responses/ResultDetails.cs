using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Globalping;

public class ResultDetails {
    [JsonPropertyName("status")]
    public string? Status { get; set; }

    [JsonPropertyName("rawOutput")]
    public string? RawOutput { get; set; }

    [JsonPropertyName("resolvedAddress")]
    public string? ResolvedAddress { get; set; }

    [JsonPropertyName("resolvedHostname")]
    public string? ResolvedHostname { get; set; }

    [JsonPropertyName("timings")]
    public JsonElement? Timings { get; set; }

    [JsonPropertyName("stats")]
    public Stats? Stats { get; set; }
}

