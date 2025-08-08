using System;
using System.Text.Json.Serialization;

namespace Globalping;

/// <summary>Represents the outcome from a single probe.</summary>
public partial class Result {
    [JsonIgnore]
    public string Target { get; set; } = string.Empty;
    [JsonPropertyName("probe")]
    public Probe Probe { get; set; } = new();

    [JsonPropertyName("result")]
    public ResultDetails Data { get; set; } = new();
}
