namespace Globalping;

public class DnsOptions : IMeasurementOptions {
    public DnsQuery Query { get; set; } = new DnsQuery();
    public string? Resolver { get; set; } // Can be IPv4, IPv6, or hostname
    public int Port { get; set; } = 53;
    public string Protocol { get; set; } = "UDP";
    public int IpVersion { get; set; } = 4;
    public bool Trace { get; set; } = false;
}
