using System.Management.Automation;
using System.Net;
using System.Net.Http;

namespace Globalping.PowerShell;

/// <summary>Retrieve current API rate limits.</summary>
/// <para>Calls the Globalping <c>/limits</c> endpoint and returns limit information.</para>
/// <list type="alertSet">
///   <item>
///     <term>Note</term>
///     <description>Unauthenticated requests may report lower limits.</description>
///   </item>
/// </list>
/// <example>
///   <summary>Check remaining requests</summary>
///   <code>
///     <para><prefix>PS&gt; </prefix>Get-GlobalpingLimit</para>
///   </code>
///   <para>Returns rate limit information for the anonymous user or provided API key.</para>
/// </example>
/// <example>
///   <summary>Retrieve raw limit data</summary>
///   <code>
///     <para><prefix>PS&gt; </prefix>Get-GlobalpingLimit -Raw</para>
///   </code>
///   <para>Outputs the unprocessed <see cref="Limits"/> response.</para>
/// </example>
/// <seealso href="https://learn.microsoft.com/powershell/" />
/// <seealso href="https://github.com/EvotecIT/Globalping" />
[Cmdlet(VerbsCommon.Get, "GlobalpingLimit")]
[OutputType(typeof(LimitInfo))]
[OutputType(typeof(Limits))]
[OutputType(typeof(Credits))]
public class GetGlobalpingLimitCommand : PSCmdlet {
    /// <summary>API key used for authenticated calls.</summary>
    /// <para>Provide this to view limits associated with your account.</para>
    [Parameter]
    [Alias("Token")]
    public string? ApiKey { get; set; }

    /// <summary>Return the raw <see cref="Limits"/> object.</summary>
    /// <para>Outputs the service response without flattening to <see cref="LimitInfo"/>.</para>
    [Parameter]
    [Alias("AsRaw")]
    public SwitchParameter Raw { get; set; }

    /// <inheritdoc/>
    protected override void ProcessRecord() {
        using var httpClient = new HttpClient(new HttpClientHandler {
#if NET6_0_OR_GREATER
            AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate | DecompressionMethods.Brotli
#else
            AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
#endif
        });
        var service = new ProbeService(httpClient, ApiKey);
        var limits = service.GetLimitsAsync().GetAwaiter().GetResult();

        if (Raw.IsPresent)
        {
            WriteObject(limits);
            return;
        }

        if (limits is null)
        {
            return;
        }

        foreach (var info in limits.Flatten())
        {
            WriteObject(info);
        }
    }
}
