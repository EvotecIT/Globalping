using System.Management.Automation;
using Globalping;

namespace Globalping.PowerShell;

[Cmdlet(VerbsLifecycle.Start, "GlobalpingTraceroute")]
[OutputType(typeof(MeasurementResponse))]
public class StartGlobalpingTracerouteCommand : StartGlobalpingBaseCommand
{
    [Parameter]
    public TracerouteOptions? Options { get; set; }

    protected override MeasurementType Type => MeasurementType.Traceroute;

    protected override IMeasurementOptions? MeasurementOptions => Options;
}
