using System.Text.Json.Serialization;

namespace Globalping;

/// <summary>
/// HTTP response protocol versions.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum HttpProtocolVersion
{
    /// <summary>Unknown HTTP version.</summary>
    Unknown,

    /// <summary>HTTP/1.0.</summary>
    Http10,

    /// <summary>HTTP/1.1.</summary>
    Http11,

    /// <summary>HTTP/2.</summary>
    Http20,

    /// <summary>HTTP/3.</summary>
    Http30
}
