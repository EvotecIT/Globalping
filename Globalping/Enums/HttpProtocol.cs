using System.Text.Json.Serialization;

namespace Globalping;

/// <summary>
/// HTTP protocol versions that can be used when performing HTTP measurements.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum HttpProtocol
{
    /// <summary>Classic HTTP/1.x.</summary>
    HTTP,

    /// <summary>Secure HTTP over TLS.</summary>
    HTTPS,

    /// <summary>HTTP/2.</summary>
    HTTP2
}
