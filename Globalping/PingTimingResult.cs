namespace Globalping;

public class PingTimingResult
{
    public string Country { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    public string Continent { get; set; } = string.Empty;
    public int Asn { get; set; }
    public string Network { get; set; } = string.Empty;
    public string? ResolvedAddress { get; set; }
    public string? ResolvedHostname { get; set; }
    public string? Status { get; set; }
    public int Ttl { get; set; }
    public double Rtt { get; set; }
}
