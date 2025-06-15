using System.Text.Json.Serialization;
namespace Globalping;
public class HttpTimings
{
    [JsonPropertyName("total")]
    public int? Total { get; set; }

    [JsonPropertyName("dns")]
    public int? Dns { get; set; }

    [JsonPropertyName("tcp")]
    public int? Tcp { get; set; }

    [JsonPropertyName("tls")]
    public int? Tls { get; set; }

    [JsonPropertyName("firstByte")]
    public int? FirstByte { get; set; }

    [JsonPropertyName("download")]
    public int? Download { get; set; }
}
