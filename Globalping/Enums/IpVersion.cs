using System.Text.Json.Serialization;

namespace Globalping;

[JsonConverter(typeof(IpVersionConverter))]
public enum IpVersion
{
    Four = 4,
    Six = 6
}
