using System.Management.Automation;
using Globalping;

namespace Globalping.PowerShell;

/// <summary>Start an HTTP request using Globalping.</summary>
/// <para>Sends an HTTP request from remote probes and returns <see cref="HttpResponseResult"/> objects.</para>
/// <list type="alertSet">
///   <item>
///     <term>Note</term>
///     <description>Requests are executed from remote probes and may trigger security alerts on the target.</description>
///   </item>
/// </list>
/// <example>
///   <summary>Fetch a webpage</summary>
///   <code>
///     <para><prefix>PS&gt; </prefix>Start-GlobalpingHttp -Target "evotec.xyz" -SimpleLocations "Krakow+PL"</para>
///   </code>
///   <para>Returns HTTP response details from the Krakow probe.</para>
/// </example>
/// <example>
///   <summary>Headers only</summary>
///   <code>
///     <para><prefix>PS&gt; </prefix>Start-GlobalpingHttp -Target "https://example.com" -HeadersOnly</para>
///   </code>
///   <para>Outputs only the HTTP headers from each probe.</para>
/// </example>
/// <seealso href="https://learn.microsoft.com/powershell/" />
/// <seealso href="https://github.com/EvotecIT/Globalping" />
[Cmdlet(VerbsLifecycle.Start, "GlobalpingHttp")]
[OutputType(typeof(HttpResponseResult))]
[OutputType(typeof(string))]
[OutputType(typeof(MeasurementResponse))]
public class StartGlobalpingHttpCommand : StartGlobalpingBaseCommand
{
    /// <summary>Return the raw measurement response.</summary>
    /// <para>The full <see cref="MeasurementResponse"/> is emitted without conversion.</para>
    [Parameter]
    [Alias("AsRaw")]
    public SwitchParameter Raw { get; set; }

    /// <summary>Output HTTP results in classic text form.</summary>
    /// <para>Each probe returns the textual output of the underlying HTTP tool.</para>
    [Parameter]
    public SwitchParameter Classic { get; set; }

    /// <summary>Emit only HTTP headers from each response.</summary>
    /// <para>Ignores the body content from the probes.</para>
    [Parameter]
    public SwitchParameter HeadersOnly { get; set; }

    /// <summary>Additional HTTP options for the request.</summary>
    /// <para>Allows setting request method, headers or body.</para>
    [Parameter]
    public HttpOptions? Options { get; set; }

    /// <inheritdoc/>
    protected override MeasurementType Type => MeasurementType.Http;

    /// <inheritdoc/>
    protected override IMeasurementOptions? MeasurementOptions => Options;

    /// <inheritdoc/>
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
