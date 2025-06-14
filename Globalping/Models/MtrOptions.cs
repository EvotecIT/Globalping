using System.Text.Json.Serialization;
namespace Globalping;

public class MtrOptions : IMeasurementOptions
{
    [JsonPropertyName("port")]
    public int Port { get; set; } = 80;

    [JsonPropertyName("protocol")]
    public MtrProtocol Protocol { get; set; } = MtrProtocol.Icmp;

    [JsonPropertyName("ipVersion")]
    public IpVersion IpVersion { get; set; } = IpVersion.Four;

    [JsonPropertyName("packets")]
    public int Packets { get; set; } = 3;
}
