using System;
using System.Collections.Generic;
namespace Globalping;

public class HttpResponseResult
{
    public string Target { get; set; } = string.Empty;
    public string Protocol { get; set; } = string.Empty;
    public int StatusCode { get; set; }
    public string StatusDescription { get; set; } = string.Empty;
    public Dictionary<string, List<string>> Headers { get; set; } = new(StringComparer.OrdinalIgnoreCase);
    public string? Body { get; set; }
    public string Country { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string Network { get; set; } = string.Empty;
    public int Asn { get; set; }
    public string State { get; set; } = string.Empty;
    public string Continent { get; set; } = string.Empty;
    public string? ResolvedAddress { get; set; }
    public string? ResolvedHostname { get; set; }
    public TestStatus Status { get; set; }
}
