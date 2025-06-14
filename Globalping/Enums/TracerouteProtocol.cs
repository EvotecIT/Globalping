using System.Text.Json.Serialization;

namespace Globalping;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum TracerouteProtocol
{
    Icmp,
    Tcp,
    Udp
}
