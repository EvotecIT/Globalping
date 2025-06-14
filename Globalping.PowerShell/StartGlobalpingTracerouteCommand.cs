using System.Management.Automation;
using Globalping;

namespace Globalping.PowerShell;

/// <summary>Start a traceroute measurement using Globalping.</summary>
/// <para>Runs traceroute from specified probes and returns hop details or raw output.</para>
/// <example>
///   <summary>Traceroute to a host</summary>
///   <prefix>PS> </prefix>
///   <code>Start-GlobalpingTraceroute -Target "evotec.xyz"</code>
///   <para>Performs traceroute to the target host.</para>
/// </example>
[Cmdlet(VerbsLifecycle.Start, "GlobalpingTraceroute")]
[OutputType(typeof(TracerouteHopResult))]
[OutputType(typeof(string))]
[OutputType(typeof(MeasurementResponse))]
public class StartGlobalpingTracerouteCommand : StartGlobalpingBaseCommand
{
    /// <para>Return the full measurement response.</para>
    [Parameter]
    [Alias("AsRaw")]
    public SwitchParameter Raw { get; set; }

    /// <para>Output traceroute in classic text form.</para>
    [Parameter]
    public SwitchParameter Classic { get; set; }

    /// <para>Additional traceroute options for the request.</para>
    [Parameter]
    public TracerouteOptions? Options { get; set; }

    protected override MeasurementType Type => MeasurementType.Traceroute;

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
            foreach (var hop in result.GetTracerouteHops())
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
