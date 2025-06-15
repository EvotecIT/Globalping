namespace Globalping;

/// <summary>
/// Provides a flattened view of API limit information.
/// </summary>
public class LimitInfo
{
    /// <summary>Name of the rate limit category.</summary>
    public string Category { get; set; } = string.Empty;

    /// <summary>Action within the category.</summary>
    public string Action { get; set; } = string.Empty;

    /// <summary>Kind of limit applied by the API.</summary>
    public string Type { get; set; } = string.Empty;

    /// <summary>Maximum number of allowed requests.</summary>
    public int Limit { get; set; }

    /// <summary>Number of requests remaining before the limit is reached.</summary>
    public int Remaining { get; set; }

    /// <summary>Epoch timestamp when the current window resets.</summary>
    public long Reset { get; set; }

    /// <summary>Credits still available for the authenticated user.</summary>
    public int? CreditsRemaining { get; set; }
}
