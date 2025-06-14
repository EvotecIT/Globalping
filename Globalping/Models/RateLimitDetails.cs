using System.Text.Json.Serialization;

namespace Globalping;

/// <summary>
/// Describes rate limit information for a single API action.
/// </summary>
public class RateLimitDetails
{
    /// <summary>
    /// Kind of limit applied by the API, typically <c>ip</c> or <c>user</c>.
    /// </summary>
    [JsonPropertyName("type")]
    public string Type { get; set; } = string.Empty;

    /// <summary>
    /// Maximum number of allowed actions within the window.
    /// </summary>
    [JsonPropertyName("limit")]
    public int Limit { get; set; }

    /// <summary>
    /// Number of actions remaining before the limit is reached.
    /// </summary>
    [JsonPropertyName("remaining")]
    public int Remaining { get; set; }

    /// <summary>
    /// Epoch timestamp (seconds) when the current window resets.
    /// </summary>
    [JsonPropertyName("reset")]
    public long Reset { get; set; }
}
