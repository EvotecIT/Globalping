using System.Management.Automation;
using Globalping;

namespace Globalping.PowerShell;

[Cmdlet(VerbsLifecycle.Start, "GlobalpingHttp")]
[OutputType(typeof(HttpResponseResult))]
[OutputType(typeof(string))]
[OutputType(typeof(MeasurementResponse))]
public class StartGlobalpingHttpCommand : StartGlobalpingBaseCommand
{
    [Parameter]
    [Alias("AsRaw")]
    public SwitchParameter Raw { get; set; }

    [Parameter]
    public SwitchParameter Classic { get; set; }

    [Parameter]
    public SwitchParameter HeadersOnly { get; set; }

    [Parameter]
    public HttpOptions? Options { get; set; }

    protected override MeasurementType Type => MeasurementType.Http;

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
            foreach (var resp in result.GetHttpResponses())
            {
                if (HeadersOnly.IsPresent)
                {
                    WriteObject(resp.Headers);
                }
                else
                {
                    WriteObject(resp);
                }
            }
        }
        else
        {
            WriteObject(result);
        }
    }
}
