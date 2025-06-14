using System.Text.Json.Serialization;
namespace Globalping;

public class DnsQuery {
    [JsonPropertyName("type")]
    public DnsQueryType Type { get; set; } = DnsQueryType.A;
}
