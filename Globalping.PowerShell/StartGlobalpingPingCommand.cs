using System.Management.Automation;
using Globalping;

namespace Globalping.PowerShell;

/// <summary>Start a ping measurement using Globalping.</summary>
/// <para>Executes ping from selected probes and returns timing results or raw data.</para>
/// <example>
///   <summary>Ping from multiple locations</summary>
///   <prefix>PS> </prefix>
///   <code>Start-GlobalpingPing -Target "evotec.xyz" -SimpleLocations "DE", "US"</code>
///   <para>Runs ping from probes in Germany and the United States.</para>
/// </example>
[Cmdlet(VerbsLifecycle.Start, "GlobalpingPing")]
[OutputType(typeof(PingTimingResult))]
[OutputType(typeof(string))]
[OutputType(typeof(MeasurementResponse))]
public class StartGlobalpingPingCommand : StartGlobalpingBaseCommand
{
    /// <para>Return the unprocessed measurement response.</para>
    [Parameter]
    [Alias("AsRaw")]
    public SwitchParameter Raw { get; set; }

    /// <para>Output ping results in classic text format.</para>
    [Parameter]
    public SwitchParameter Classic { get; set; }

    /// <para>Additional ping options sent with the request.</para>
    [Parameter]
    public PingOptions? Options { get; set; }

    protected override MeasurementType Type => MeasurementType.Ping;

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
            foreach (var timing in result.GetPingTimings())
            {
                WriteObject(timing);
            }
        }
        else
        {
            WriteObject(result);
        }
    }
}
