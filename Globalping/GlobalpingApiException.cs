using System;

namespace Globalping;

/// <summary>
/// Exception thrown when the Globalping API returns an error response.
/// </summary>
public class GlobalpingApiException : Exception
{
    /// <summary>HTTP status code returned by the API.</summary>
    public int StatusCode { get; }

    /// <summary>Parsed error information.</summary>
    public ErrorDetails Error { get; }

    /// <summary>Usage info associated with the response.</summary>
    public ApiUsageInfo UsageInfo { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="GlobalpingApiException"/> class.
    /// </summary>
    /// <param name="statusCode">HTTP status code.</param>
    /// <param name="error">Error details returned by the API.</param>
    /// <param name="usageInfo">Captured usage information.</param>
    public GlobalpingApiException(int statusCode, ErrorDetails error, ApiUsageInfo usageInfo)
        : base(error?.Message)
    {
        StatusCode = statusCode;
        Error = error;
        UsageInfo = usageInfo;
    }
}
