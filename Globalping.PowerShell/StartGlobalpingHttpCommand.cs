using System.Management.Automation;
using Globalping;

namespace Globalping.PowerShell;

/// <summary>Start an HTTP request using Globalping.</summary>
/// <para>Sends an HTTP request from remote probes and returns the responses.</para>
/// <example>
///   <summary>Fetch a webpage</summary>
///   <prefix>PS> </prefix>
///   <code>Start-GlobalpingHttp -Target "evotec.xyz" -SimpleLocations "Krakow+PL"</code>
///   <para>Returns HTTP response details from the Krakow probe.</para>
/// </example>
[Cmdlet(VerbsLifecycle.Start, "GlobalpingHttp")]
[OutputType(typeof(HttpResponseResult))]
[OutputType(typeof(string))]
[OutputType(typeof(MeasurementResponse))]
public class StartGlobalpingHttpCommand : StartGlobalpingBaseCommand
{
    /// <para>Return the raw measurement response.</para>
    [Parameter]
    [Alias("AsRaw")]
    public SwitchParameter Raw { get; set; }

    /// <para>Output HTTP results in classic text form.</para>
    [Parameter]
    public SwitchParameter Classic { get; set; }

    /// <para>Emit only HTTP headers from each response.</para>
    [Parameter]
    public SwitchParameter HeadersOnly { get; set; }

    /// <para>Additional HTTP options for the request.</para>
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
