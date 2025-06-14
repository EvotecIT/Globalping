using System;
using System.Text.Json.Serialization;

namespace Globalping;

public class Result {
    [JsonPropertyName("probe")]
    public Probe Probe { get; set; } = new();

    [JsonPropertyName("result")]
    public ResultDetails Data { get; set; } = new();
}
