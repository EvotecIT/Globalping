using System.Text.Json.Serialization;
namespace Globalping;

public class TracerouteOptions : IMeasurementOptions {
    [JsonPropertyName("port")]
    public int Port { get; set; } = 80;

    [JsonPropertyName("protocol")]
    public string Protocol { get; set; } = "ICMP";

    [JsonPropertyName("ipVersion")]
    public int IpVersion { get; set; } = 4;
}
