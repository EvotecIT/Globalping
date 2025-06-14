using System.Text.Json.Serialization;
namespace Globalping;

public class PingOptions : IMeasurementOptions {
    [JsonPropertyName("packets")]
    public int Packets { get; set; } = 3;

    [JsonPropertyName("ipVersion")]
    public IpVersion IpVersion { get; set; } = IpVersion.Four;
}

