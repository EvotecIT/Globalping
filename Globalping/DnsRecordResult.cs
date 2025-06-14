namespace Globalping;

public class DnsRecordResult
{
    public string Target { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public int Ttl { get; set; }
    public string Type { get; set; } = string.Empty;
    public string Data { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string Network { get; set; } = string.Empty;
    public int Asn { get; set; }
    public string State { get; set; } = string.Empty;
    public string Continent { get; set; } = string.Empty;
    public string? ResolvedAddress { get; set; }
    public string? ResolvedHostname { get; set; }
    public string? Status { get; set; }
}
