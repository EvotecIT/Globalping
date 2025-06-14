using System.Management.Automation;
using Globalping;

namespace Globalping.PowerShell;

[Cmdlet(VerbsLifecycle.Start, "GlobalpingPing")]
[OutputType(typeof(PingTimingResult))]
[OutputType(typeof(string))]
[OutputType(typeof(MeasurementResponse))]
public class StartGlobalpingPingCommand : StartGlobalpingBaseCommand
{
    [Parameter]
    [Alias("Raw")]
    public SwitchParameter AsRaw { get; set; }

    [Parameter]
    public SwitchParameter Classic { get; set; }

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
        else if (!AsRaw.IsPresent && result is not null)
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
