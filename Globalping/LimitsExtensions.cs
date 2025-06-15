using System.Collections.Generic;
using System.Text.Json;

namespace Globalping;

/// <summary>
/// Extension helpers for <see cref="Limits"/>.
/// </summary>
public static class LimitsExtensions
{
    /// <summary>
    /// Flattens nested rate limit information into friendly objects.
    /// </summary>
    /// <param name="limits">API limits returned by the service.</param>
    /// <returns>Collection of <see cref="LimitInfo"/> objects.</returns>
    public static IEnumerable<LimitInfo> Flatten(this Limits limits)
    {
        var credits = limits.Credits?.Remaining;
        foreach (var category in limits.RateLimit)
        {
            foreach (var action in category.Value)
            {
                var detail = action.Value;
                yield return new LimitInfo
                {
                    Category = category.Key,
                    Action = action.Key,
                    Type = JsonNamingPolicy.CamelCase.ConvertName(detail.Type.ToString()),
                    Limit = detail.Limit,
                    Remaining = detail.Remaining,
                    Reset = detail.Reset,
                    CreditsRemaining = credits
                };
            }
        }
    }
}
