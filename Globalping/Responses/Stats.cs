using System.Text.Json.Serialization;
namespace Globalping;

public class Stats {
    [JsonPropertyName("min")]
    public double Min { get; set; }

    [JsonPropertyName("max")]
    public double Max { get; set; }

    [JsonPropertyName("avg")]
    public double Avg { get; set; }

    [JsonPropertyName("total")]
    public int Total { get; set; }

    [JsonPropertyName("loss")]
    public int Loss { get; set; }

    [JsonPropertyName("rcv")]
    public int Rcv { get; set; }

    [JsonPropertyName("drop")]
    public int Drop { get; set; }
}

