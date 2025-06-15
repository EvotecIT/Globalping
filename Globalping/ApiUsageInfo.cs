using System;

namespace Globalping;

/// <summary>
/// Captures rate limit and credit information returned via HTTP headers.
/// </summary>
public class ApiUsageInfo
{
    public int? RateLimitLimit { get; set; }
    public int? RateLimitConsumed { get; set; }
    public int? RateLimitRemaining { get; set; }
    public long? RateLimitReset { get; set; }
    public int? CreditsConsumed { get; set; }
    public int? CreditsRemaining { get; set; }
    public int? RequestCost { get; set; }
    public int? RetryAfter { get; set; }
    public string? ETag { get; set; }
}
