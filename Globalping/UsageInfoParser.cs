namespace Globalping;

using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;

/// <summary>
/// Helper class for parsing usage information from HTTP headers.
/// </summary>
internal static class UsageInfoParser
{
    internal static ApiUsageInfo Parse(HttpResponseMessage response)
    {
        var headers = response.Headers;
        return new ApiUsageInfo
        {
            RateLimitLimit = TryGetInt(headers, "X-RateLimit-Limit"),
            RateLimitConsumed = TryGetInt(headers, "X-RateLimit-Consumed"),
            RateLimitRemaining = TryGetInt(headers, "X-RateLimit-Remaining"),
            RateLimitReset = TryGetLong(headers, "X-RateLimit-Reset"),
            CreditsConsumed = TryGetInt(headers, "X-Credits-Consumed"),
            CreditsRemaining = TryGetInt(headers, "X-Credits-Remaining"),
            RequestCost = TryGetInt(headers, "X-Request-Cost"),
            RetryAfter = TryGetInt(headers, "Retry-After"),
            ETag = headers.ETag?.Tag
        };
    }

    private static int? TryGetInt(HttpResponseHeaders headers, string name)
    {
        if (headers.TryGetValues(name, out var values) && int.TryParse(values.FirstOrDefault(), out var v))
        {
            return v;
        }
        return null;
    }

    private static long? TryGetLong(HttpResponseHeaders headers, string name)
    {
        if (headers.TryGetValues(name, out var values) && long.TryParse(values.FirstOrDefault(), out var v))
        {
            return v;
        }
        return null;
    }
}
