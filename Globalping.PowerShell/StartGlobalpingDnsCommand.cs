using System.Management.Automation;
using Globalping;

namespace Globalping.PowerShell;

/// <summary>Start a DNS lookup using Globalping.</summary>
/// <para>Queries DNS records from remote probes.</para>
/// <example>
///   <summary>Resolve A record</summary>
///   <prefix>PS> </prefix>
///   <code>Start-GlobalpingDns -Target "evotec.xyz"</code>
///   <para>Returns DNS records from available probes.</para>
/// </example>
[Cmdlet(VerbsLifecycle.Start, "GlobalpingDns")]
[OutputType(typeof(DnsRecordResult))]
[OutputType(typeof(string))]
[OutputType(typeof(MeasurementResponse))]
public class StartGlobalpingDnsCommand : StartGlobalpingBaseCommand
{
    /// <para>Return the raw measurement response.</para>
    [Parameter]
    [Alias("AsRaw")]
    public SwitchParameter Raw { get; set; }

    /// <para>Output DNS results in classic text form.</para>
    [Parameter]
    public SwitchParameter Classic { get; set; }

    /// <para>Additional DNS options for the request.</para>
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
