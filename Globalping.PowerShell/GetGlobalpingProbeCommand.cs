using System.Collections.Generic;
using System.Management.Automation;
using System.Net;
using System.Net.Http;

namespace Globalping.PowerShell;

/// <summary>Retrieve currently online probes.</summary>
/// <para>Calls the Globalping <c>/probes</c> endpoint and returns probe information.</para>
/// <example>
///   <summary>List probes</summary>
///   <code>Get-GlobalpingProbe</code>
///   <para>Outputs flattened probe objects describing each online probe.</para>
/// </example>
[Cmdlet(VerbsCommon.Get, "GlobalpingProbe")]
[OutputType(typeof(ProbeInfo))]
[OutputType(typeof(Probes))]
public class GetGlobalpingProbeCommand : PSCmdlet {
    /// <summary>API key used for authenticated calls.</summary>
    [Parameter]
    [Alias("Token")]
    public string? ApiKey { get; set; }

    /// <summary>Return the raw <see cref="Probes"/> objects.</summary>
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
        var probes = service.GetOnlineProbesAsync().GetAwaiter().GetResult();

        if (Raw.IsPresent)
        {
            WriteObject(probes, true);
            return;
        }

        foreach (var p in probes)
        {
            WriteObject(p.ToProbeInfo());
        }
    }
}
