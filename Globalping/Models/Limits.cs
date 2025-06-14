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
}
