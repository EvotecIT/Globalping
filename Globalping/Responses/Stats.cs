using System.Text.Json.Serialization;
namespace Globalping;

/// <summary>Statistical summary for measurement results.</summary>
public class Stats {
    [JsonPropertyName("min")]
    public double? Min { get; set; }

    [JsonPropertyName("max")]
    public double? Max { get; set; }

    [JsonPropertyName("avg")]
    public double? Avg { get; set; }

    [JsonPropertyName("total")]
    public int? Total { get; set; }

    [JsonPropertyName("loss")]
    public double? Loss { get; set; }

    [JsonPropertyName("rcv")]
    public int? Rcv { get; set; }

    [JsonPropertyName("drop")]
    public int? Drop { get; set; }

    [JsonPropertyName("stDev")]
    public double? StDev { get; set; }

    [JsonPropertyName("jMin")]
    public double? JMin { get; set; }

    [JsonPropertyName("jAvg")]
    public double? JAvg { get; set; }

    [JsonPropertyName("jMax")]
    public double? JMax { get; set; }
}

