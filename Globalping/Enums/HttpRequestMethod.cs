using System.Text.Json.Serialization;

namespace Globalping;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum HttpRequestMethod
{
    HEAD,
    GET,
    OPTIONS
}
