using System.Text.Json.Serialization;
namespace Globalping;

/// <summary>
/// Options for the MTR (My Traceroute) measurement.
/// </summary>
public class MtrOptions : IMeasurementOptions
{
    /// <summary>Destination port for the test.</summary>
    [JsonPropertyName("port")]
    public int Port { get; set; } = 80;

    /// <summary>Transport protocol to use.</summary>
    [JsonPropertyName("protocol")]
    public MtrProtocol Protocol { get; set; } = MtrProtocol.ICMP;

    /// <summary>IP version used for destination resolution.</summary>
    [JsonPropertyName("ipVersion")]
    public IpVersion IpVersion { get; set; } = IpVersion.Four;

    /// <summary>Number of packets to send.</summary>
    [JsonPropertyName("packets")]
    public int Packets { get; set; } = 3;
}
