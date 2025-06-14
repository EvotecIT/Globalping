using System.Management.Automation;
using Globalping;

namespace Globalping.PowerShell;

/// <summary>Start an MTR (My Traceroute) measurement using Globalping.</summary>
/// <para>Combines ping and traceroute information from multiple probes.</para>
/// <example>
///   <summary>Run MTR against a host</summary>
///   <prefix>PS> </prefix>
///   <code>Start-GlobalpingMtr -Target "evotec.xyz"</code>
///   <para>Produces hop statistics for the target.</para>
/// </example>
[Cmdlet(VerbsLifecycle.Start, "GlobalpingMtr")]
[OutputType(typeof(MtrHopResult))]
[OutputType(typeof(string))]
[OutputType(typeof(MeasurementResponse))]
public class StartGlobalpingMtrCommand : StartGlobalpingBaseCommand
{
    /// <para>Return the raw measurement response.</para>
    [Parameter]
    [Alias("AsRaw")]
    public SwitchParameter Raw { get; set; }

    /// <para>Output MTR results in classic text form.</para>
    [Parameter]
    public SwitchParameter Classic { get; set; }

    /// <para>Additional MTR options for the request.</para>
    [Parameter]
    public MtrOptions? Options { get; set; }

    protected override MeasurementType Type => MeasurementType.Mtr;

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
            foreach (var hop in result.GetMtrHops())
            {
                WriteObject(hop);
            }
        }
        else
        {
            WriteObject(result);
        }
    }
}
