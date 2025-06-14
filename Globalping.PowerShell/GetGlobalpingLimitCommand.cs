using System.Management.Automation;
using System.Net;
using System.Net.Http;

namespace Globalping.PowerShell;

/// <summary>Retrieve current API rate limits.</summary>
/// <para>Calls the Globalping <c>/limits</c> endpoint and returns limit information.</para>
/// <example>
///   <summary>Check remaining requests</summary>
///   <code>Get-GlobalpingLimit</code>
///   <para>Returns rate limit information for the anonymous user or provided API key.</para>
/// </example>
[Cmdlet(VerbsCommon.Get, "GlobalpingLimit")]
[OutputType(typeof(Limits))]
[OutputType(typeof(Credits))]
public class GetGlobalpingLimitCommand : PSCmdlet
{
    /// <summary>API key used for authenticated calls.</summary>
    [Parameter]
    [Alias("Token")]
    public string? ApiKey { get; set; }

    /// <inheritdoc/>
    protected override void ProcessRecord()
    {
        using var httpClient = new HttpClient(new HttpClientHandler
        {
#if NET6_0_OR_GREATER
            AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate | DecompressionMethods.Brotli
#else
            AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
#endif
        });
        var service = new ProbeService(httpClient, ApiKey);
        var limits = service.GetLimitsAsync().GetAwaiter().GetResult();
        WriteObject(limits);
    }
}
