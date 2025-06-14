using System;
using System.Text.Json.Serialization;

namespace Globalping;

public partial class Result {
    [JsonIgnore]
    public string Target { get; set; } = string.Empty;
    [JsonPropertyName("probe")]
    public Probe Probe { get; set; } = new();

    [JsonPropertyName("result")]
    public ResultDetails Data { get; set; } = new();
}
