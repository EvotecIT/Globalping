using System.Text.Json.Serialization;

namespace Globalping;

/// <summary>
/// Available protocols for DNS queries.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum DnsProtocol
{
    /// <summary>Transmission Control Protocol.</summary>
    TCP,

    /// <summary>User Datagram Protocol.</summary>
    UDP
}
