using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Globalping;

/// <summary>
/// Describes the HTTP request issued by the measurement.
/// </summary>
public class HttpRequestOptions
{
    /// <summary>Hostname used for the request.</summary>
    [JsonPropertyName("host")]
    public string? Host { get; set; }

    /// <summary>Request path.</summary>
    [JsonPropertyName("path")]
    public string? Path { get; set; }

    /// <summary>Query string portion of the request.</summary>
    [JsonPropertyName("query")]
    public string? Query { get; set; }

    /// <summary>HTTP method to use.</summary>
    [JsonPropertyName("method")]
    public HttpRequestMethod Method { get; set; } = HttpRequestMethod.HEAD;

    /// <summary>Optional HTTP headers to include.</summary>
    [JsonPropertyName("headers")]
    public Dictionary<string, string> Headers { get; set; } = new(StringComparer.OrdinalIgnoreCase);
}
