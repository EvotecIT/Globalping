using System.Text.Json.Serialization;

namespace Globalping;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum HttpProtocol
{
    HTTP,
    HTTPS,
    HTTP2
}
