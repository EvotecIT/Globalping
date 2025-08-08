using System.Text.Json.Serialization;
namespace Globalping;

/// <summary>Timing metrics for a DNS query.</summary>
public class DnsTimings
{
    [JsonPropertyName("total")]
    public double Total { get; set; }
}
