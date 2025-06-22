using System.Text.Json.Serialization;

namespace Globalping;

/// <summary>
/// HTTP methods supported by the Globalping service.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum HttpRequestMethod
{
    /// <summary>HEAD request.</summary>
    HEAD,

    /// <summary>GET request.</summary>
    GET,

    /// <summary>OPTIONS request.</summary>
    OPTIONS
}
