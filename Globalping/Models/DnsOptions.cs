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
    public string Protocol { get; set; } = "UDP";

    [JsonPropertyName("ipVersion")]
    public int IpVersion { get; set; } = 4;

    [JsonPropertyName("trace")]
    public bool Trace { get; set; } = false;
}
