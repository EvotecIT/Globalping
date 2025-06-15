using System.Text.Json.Serialization;
namespace Globalping;
public class DnsTimings
{
    [JsonPropertyName("total")]
    public double Total { get; set; }
}
