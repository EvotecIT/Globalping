using System.Text.Json.Serialization;
namespace Globalping;

/// <summary>
/// Describes the DNS record to query.
/// </summary>
public class DnsQuery {
    /// <summary>Type of DNS record to request.</summary>
    [JsonPropertyName("type")]
    public DnsQueryType Type { get; set; } = DnsQueryType.A;
}
