using System.Management.Automation;
using Globalping;

namespace Globalping.PowerShell;

[Cmdlet(VerbsLifecycle.Start, "GlobalpingMtr")]
[OutputType(typeof(MtrHopResult))]
[OutputType(typeof(string))]
[OutputType(typeof(MeasurementResponse))]
public class StartGlobalpingMtrCommand : StartGlobalpingBaseCommand
{
    [Parameter]
    [Alias("AsRaw")]
    public SwitchParameter Raw { get; set; }

    [Parameter]
    public SwitchParameter Classic { get; set; }

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
