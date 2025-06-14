using System.Management.Automation;
using Globalping;

namespace Globalping.PowerShell;

[Cmdlet(VerbsLifecycle.Start, "GlobalpingDns")]
[OutputType(typeof(MeasurementResponse))]
public class StartGlobalpingDnsCommand : StartGlobalpingBaseCommand
{
    [Parameter]
    public DnsOptions? Options { get; set; }

    protected override MeasurementType Type => MeasurementType.Dns;

    protected override IMeasurementOptions? MeasurementOptions => Options;
}
