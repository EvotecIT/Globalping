using System.Management.Automation;
using Globalping;

namespace Globalping.PowerShell;

[Cmdlet(VerbsLifecycle.Start, "GlobalpingHttp")]
[OutputType(typeof(MeasurementResponse))]
public class StartGlobalpingHttpCommand : StartGlobalpingBaseCommand
{
    [Parameter]
    public HttpOptions? Options { get; set; }

    protected override MeasurementType Type => MeasurementType.Http;

    protected override IMeasurementOptions? MeasurementOptions => Options;
}
