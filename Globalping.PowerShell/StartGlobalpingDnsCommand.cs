using System.Management.Automation;
using Globalping;

namespace Globalping.PowerShell;

/// <summary>Start a DNS lookup using Globalping.</summary>
/// <para>Queries DNS records from remote probes and converts them to <see cref="DnsRecordResult"/> objects.</para>
/// <list type="alertSet">
///   <item>
///     <term>Note</term>
///     <description>DNS caches may cause different probes to return varying results.</description>
///   </item>
/// </list>
/// <example>
///   <summary>Resolve A record</summary>
///   <code>
///     <para><prefix>PS&gt; </prefix>Start-GlobalpingDns -Target "evotec.xyz"</para>
///   </code>
///   <para>Returns DNS records from available probes.</para>
/// </example>
/// <example>
///   <summary>Use custom resolver</summary>
///   <code>
///     <para><prefix>PS&gt; </prefix>Start-GlobalpingDns -Target "cloudflare.com" -Options @{ Resolver = "8.8.8.8" }</para>
///   </code>
///   <para>Sends the DNS query using the Google public resolver.</para>
/// </example>
/// <seealso href="https://learn.microsoft.com/powershell/" />
/// <seealso href="https://github.com/EvotecIT/Globalping" />
[Cmdlet(VerbsLifecycle.Start, "GlobalpingDns")]
[OutputType(typeof(DnsRecordResult))]
[OutputType(typeof(string))]
[OutputType(typeof(MeasurementResponse))]
public class StartGlobalpingDnsCommand : StartGlobalpingBaseCommand
{
    /// <summary>Return the raw measurement response.</summary>
    /// <para>Use this switch to inspect the <see cref="MeasurementResponse"/> object without conversion.</para>
    [Parameter]
    [Alias("AsRaw")]
    public SwitchParameter Raw { get; set; }

    /// <summary>Output DNS results in classic text form.</summary>
    /// <para>The original text returned by the resolver is emitted.</para>
    [Parameter]
    public SwitchParameter Classic { get; set; }

    /// <summary>Additional DNS options for the request.</summary>
    /// <para>Allows custom resolver, query type or trace options.</para>
    [Parameter]
    public DnsOptions? Options { get; set; }

    /// <inheritdoc/>
    protected override MeasurementType Type => MeasurementType.Dns;

    /// <inheritdoc/>
    protected override IMeasurementOptions? MeasurementOptions => Options;

    /// <inheritdoc/>
    protected override void HandleOutput(MeasurementResponse? result)
    {
        if (Classic.IsPresent && result?.Results != null)
        {
            foreach (var r in result.Results)
            {
                if (r.Data?.RawOutput is not null)
                {
                    WriteObject(r.Data.RawOutput);
                }
            }
        }
        else if (!Raw.IsPresent && result is not null)
        {
            foreach (var rec in result.GetDnsRecords())
            {
                WriteObject(rec);
            }
        }
        else
        {
            WriteObject(result);
        }
    }
}
