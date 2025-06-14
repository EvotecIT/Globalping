using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Globalping;

public class HttpRequestOptions
{
    [JsonPropertyName("host")]
    public string? Host { get; set; }

    [JsonPropertyName("path")]
    public string? Path { get; set; }

    [JsonPropertyName("query")]
    public string? Query { get; set; }

    [JsonPropertyName("method")]
    public HttpRequestMethod Method { get; set; } = HttpRequestMethod.HEAD;

    [JsonPropertyName("headers")]
    public Dictionary<string, string> Headers { get; set; } = new(StringComparer.OrdinalIgnoreCase);
}
