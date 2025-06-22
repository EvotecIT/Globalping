using System.Text.Json.Serialization;

namespace Globalping;

/// <summary>
/// Network protocols that can be used for traceroute operations.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum TracerouteProtocol
{
    /// <summary>ICMP traceroute.</summary>
    ICMP,

    /// <summary>TCP traceroute.</summary>
    TCP,

    /// <summary>UDP traceroute.</summary>
    UDP
}
