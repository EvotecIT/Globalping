using System.Collections.Generic;

namespace Globalping;

public class MtrHopResult
{
    public string Target { get; set; } = string.Empty;
    public int Hop { get; set; }
    public List<int> Asn { get; set; } = new();
    public string Host { get; set; } = string.Empty;
    public string IpAddress { get; set; } = string.Empty;
    public double? LossPercent { get; set; }
    public int? Drop { get; set; }
    public int? Rcv { get; set; }
    public double? Avg { get; set; }
    public double? StDev { get; set; }
    public double? Javg { get; set; }
    public string Country { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string Network { get; set; } = string.Empty;
    public int ProbeAsn { get; set; }
    public string State { get; set; } = string.Empty;
    public string Continent { get; set; } = string.Empty;
    public string? ResolvedAddress { get; set; }
    public string? ResolvedHostname { get; set; }
    public TestStatus Status { get; set; }
}
