using System.Text.Json.Serialization;
namespace Globalping;

/// <summary>
/// Options used when executing a traceroute measurement.
/// </summary>
public class TracerouteOptions : IMeasurementOptions {
    /// <summary>Destination port for traceroute packets.</summary>
    [JsonPropertyName("port")]
    public int Port { get; set; } = 80;

    /// <summary>Protocol used to send traceroute packets.</summary>
    [JsonPropertyName("protocol")]
    public TracerouteProtocol Protocol { get; set; } = TracerouteProtocol.ICMP;

    /// <summary>IP version used when resolving the target.</summary>
    [JsonPropertyName("ipVersion")]
    public IpVersion IpVersion { get; set; } = IpVersion.Four;
}
