using System.Text.Json.Serialization;

namespace Globalping;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum MtrProtocol
{
    ICMP,
    TCP,
    UDP
}
