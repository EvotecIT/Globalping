using System.Text.Json.Serialization;

namespace Globalping;

/// <summary>
/// IP protocol versions supported by the API.
/// </summary>
[JsonConverter(typeof(IpVersionConverter))]
public enum IpVersion
{
    /// <summary>IPv4.</summary>
    Four = 4,

    /// <summary>IPv6.</summary>
    Six = 6
}
