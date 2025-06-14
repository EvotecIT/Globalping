using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Globalping;

/// <summary>
/// Represents API usage limits returned by the service.
/// </summary>
public class Limits
{
    /// <summary>
    /// Nested dictionary containing rate limits grouped by category and action.
    /// </summary>
    [JsonPropertyName("rateLimit")]
    public Dictionary<string, Dictionary<string, RateLimitDetails>> RateLimit { get; set; } = new();

    /// <summary>
    /// Remaining credits for the authenticated user.
    /// </summary>
    [JsonPropertyName("credits")]
    public Credits? Credits { get; set; }
}

/// <summary>
/// Represents credit information returned by the API.
/// </summary>
public class Credits
{
    /// <summary>
    /// Number of credits still available.
    /// </summary>
    [JsonPropertyName("remaining")]
    public int Remaining { get; set; }
}
