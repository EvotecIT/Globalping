using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Globalping;

/// <summary>
/// Container for API error details returned in non-successful responses.
/// </summary>
public class ErrorResponse
{
    /// <summary>Error information.</summary>
    [JsonPropertyName("error")]
    public ErrorDetails Error { get; set; } = new();
}

/// <summary>
/// Describes an error returned by the Globalping API.
/// </summary>
public class ErrorDetails
{
    /// <summary>Machine readable error type.</summary>
    [JsonPropertyName("type")]
    public string Type { get; set; } = string.Empty;

    /// <summary>Human readable error message.</summary>
    [JsonPropertyName("message")]
    public string Message { get; set; } = string.Empty;

    /// <summary>Additional error parameters when provided.</summary>
    [JsonPropertyName("params")]
    public Dictionary<string, string>? Params { get; set; }
}
