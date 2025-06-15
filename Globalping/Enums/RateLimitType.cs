using System.Text.Json.Serialization;

namespace Globalping;

/// <summary>
/// Specifies the type of rate limit applied by the API.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum RateLimitType
{
    /// <summary>
    /// Rate limit that applies per IP address.
    /// </summary>
    Ip,

    /// <summary>
    /// Rate limit that applies per authenticated user.
    /// </summary>
    User
}
