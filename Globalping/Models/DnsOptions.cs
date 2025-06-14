using System.Text.Json.Serialization;
namespace Globalping;

public class DnsOptions : IMeasurementOptions {
    [JsonPropertyName("query")]
    public DnsQuery Query { get; set; } = new DnsQuery();

    [JsonPropertyName("resolver")]
    public string? Resolver { get; set; } // Can be IPv4, IPv6, or hostname

    [JsonPropertyName("port")]
    public int Port { get; set; } = 53;

    [JsonPropertyName("protocol")]
    public DnsProtocol Protocol { get; set; } = DnsProtocol.Udp;

    [JsonPropertyName("ipVersion")]
    public IpVersion IpVersion { get; set; } = IpVersion.Four;

    [JsonPropertyName("trace")]
    public bool Trace { get; set; } = false;
}
