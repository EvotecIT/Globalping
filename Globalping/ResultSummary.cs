using System.Collections.Generic;
using System.Text.Json;

namespace Globalping;

public class ResultSummary
{
    public string Country { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    public string Continent { get; set; } = string.Empty;
    public int Asn { get; set; }
    public string Network { get; set; } = string.Empty;
    public string? ResolvedAddress { get; set; }
    public string? ResolvedHostname { get; set; }
    public TestStatus Status { get; set; }
    public JsonElement? Timings { get; set; }
    public Stats? Stats { get; set; }
}
