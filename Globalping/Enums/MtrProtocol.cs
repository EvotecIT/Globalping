using System.Text.Json.Serialization;

namespace Globalping;

/// <summary>
/// Transport protocols for the MTR measurement.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum MtrProtocol
{
    /// <summary>ICMP protocol.</summary>
    ICMP,

    /// <summary>TCP protocol.</summary>
    TCP,

    /// <summary>UDP protocol.</summary>
    UDP
}
