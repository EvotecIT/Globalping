using System.Text.Json.Serialization;

namespace Globalping;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum MtrProtocol
{
    Icmp,
    Tcp,
    Udp
}
