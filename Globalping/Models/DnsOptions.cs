using System.Text.Json.Serialization;
namespace Globalping;

/// <summary>
/// Options controlling a DNS measurement.
/// </summary>
public class DnsOptions : IMeasurementOptions {
    /// <summary>Query parameters for the DNS request.</summary>
    [JsonPropertyName("query")]
    public DnsQuery Query { get; set; } = new DnsQuery();

    /// <summary>Resolver hostname or IP address.</summary>
    [JsonPropertyName("resolver")]
    public string? Resolver { get; set; } // Can be IPv4, IPv6, or hostname

    /// <summary>Network port used by the DNS server.</summary>
    [JsonPropertyName("port")]
    public int Port { get; set; } = 53;

    /// <summary>Transport protocol for the query.</summary>
    [JsonPropertyName("protocol")]
    public DnsProtocol Protocol { get; set; } = DnsProtocol.UDP;

    /// <summary>Preferred IP protocol version.</summary>
    [JsonPropertyName("ipVersion")]
    public IpVersion IpVersion { get; set; } = IpVersion.Four;

    /// <summary>Whether to perform a DNS trace.</summary>
    [JsonPropertyName("trace")]
    public bool Trace { get; set; } = false;
}
