using System.Management.Automation;
using Globalping;

namespace Globalping.PowerShell;

/// <summary>Start a DNS lookup using Globalping.</summary>
/// <para>Queries DNS records from remote probes and converts them to <see cref="DnsRecordResult"/> objects.</para>
/// <example>
///   <summary>Resolve A record</summary>
///   <prefix>PS> </prefix>
///   <code>Start-GlobalpingDns -Target "evotec.xyz"</code>
///   <para>Returns DNS records from available probes.</para>
/// </example>
/// <example>
///   <summary>Use custom resolver</summary>
///   <prefix>PS> </prefix>
///   <code>Start-GlobalpingDns -Target "cloudflare.com" -Options @{ Resolver = "8.8.8.8" }</code>
///   <para>Sends the DNS query using the Google public resolver.</para>
/// </example>
[Cmdlet(VerbsLifecycle.Start, "GlobalpingDns")]
[OutputType(typeof(DnsRecordResult))]
[OutputType(typeof(string))]
[OutputType(typeof(MeasurementResponse))]
public class StartGlobalpingDnsCommand : StartGlobalpingBaseCommand
{
    /// <para>Return the raw measurement response.</para>
    /// <para>Use this switch to inspect the <see cref="MeasurementResponse"/> object without conversion.</para>
    [Parameter]
    [Alias("AsRaw")]
    public SwitchParameter Raw { get; set; }

    /// <para>Output DNS results in classic text form.</para>
    /// <para>The original text returned by the resolver is emitted.</para>
    [Parameter]
    public SwitchParameter Classic { get; set; }

    /// <para>Additional DNS options for the request.</para>
    /// <para>Allows custom resolver, query type or trace options.</para>
    [Parameter]
    public DnsOptions? Options { get; set; }

    protected override MeasurementType Type => MeasurementType.Dns;

    protected override IMeasurementOptions? MeasurementOptions => Options;

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
