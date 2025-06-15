namespace Globalping;

/// <summary>
/// Timing information for an individual ping packet.
/// </summary>
public class PingTimingResult {
    public string Target { get; set; } = string.Empty;
    public string? ResolvedAddress { get; set; }
    public int IcmpSequence { get; set; }
    public int TTL { get; set; }
    public double Time { get; set; }
    public string Country { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string Network { get; set; } = string.Empty;
    public int Asn { get; set; }
    public string State { get; set; } = string.Empty;
    public string Continent { get; set; } = string.Empty;
    public string Version { get; set; } = string.Empty;
    public string? ResolvedHostname { get; set; }
    public TestStatus Status { get; set; }
}
