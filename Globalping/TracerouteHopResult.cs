namespace Globalping;

/// <summary>Represents a single hop in a traceroute measurement.</summary>
public class TracerouteHopResult
{
    public string Target { get; set; } = string.Empty;
    public int Hop { get; set; }
    public string Host { get; set; } = string.Empty;
    public string IpAddress { get; set; } = string.Empty;
    public double? Time1 { get; set; }
    public double? Time2 { get; set; }
    public double? Time3 { get; set; }
    public string Country { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string Network { get; set; } = string.Empty;
    public int Asn { get; set; }
    public string State { get; set; } = string.Empty;
    public string Continent { get; set; } = string.Empty;
    public string Version { get; set; } = string.Empty;
    public string? ResolvedAddress { get; set; }
    public string? ResolvedHostname { get; set; }
    public TestStatus Status { get; set; }
}
