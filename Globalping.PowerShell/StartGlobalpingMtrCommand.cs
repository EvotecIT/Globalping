using System.Management.Automation;
using Globalping;

namespace Globalping.PowerShell;

[Cmdlet(VerbsLifecycle.Start, "GlobalpingMtr")]
[OutputType(typeof(MeasurementResponse))]
public class StartGlobalpingMtrCommand : StartGlobalpingBaseCommand
{
    [Parameter]
    public MtrOptions? Options { get; set; }

    protected override MeasurementType Type => MeasurementType.Mtr;

    protected override IMeasurementOptions? MeasurementOptions => Options;
}
