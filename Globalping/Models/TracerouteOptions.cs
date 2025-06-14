using System.Text.Json.Serialization;
namespace Globalping;

public class TracerouteOptions : IMeasurementOptions {
    [JsonPropertyName("port")]
    public int Port { get; set; } = 80;

    [JsonPropertyName("protocol")]
    public TracerouteProtocol Protocol { get; set; } = TracerouteProtocol.ICMP;

    [JsonPropertyName("ipVersion")]
    public IpVersion IpVersion { get; set; } = IpVersion.Four;
}
