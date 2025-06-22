using System.Text.Json.Serialization;
namespace Globalping;

/// <summary>
/// Options for an ICMP ping measurement.
/// </summary>
public class PingOptions : IMeasurementOptions {
    /// <summary>Number of echo requests to send.</summary>
    [JsonPropertyName("packets")]
    public int Packets { get; set; } = 3;

    /// <summary>IP protocol version used to resolve the target.</summary>
    [JsonPropertyName("ipVersion")]
    public IpVersion IpVersion { get; set; } = IpVersion.Four;
}

