using System.Management.Automation;
using Globalping;

namespace Globalping.PowerShell;

/// <summary>Start a ping measurement using Globalping.</summary>
/// <para>Instructs remote probes to send ICMP echo requests to the specified target.</para>
/// <para>The cmdlet outputs <see cref="PingTimingResult"/> objects, raw results or classic text depending on the selected parameters.</para>
/// <example>
///   <summary>Ping from multiple locations</summary>
///   <prefix>PS> </prefix>
///   <code>Start-GlobalpingPing -Target "evotec.xyz" -SimpleLocations "DE", "US"</code>
///   <para>Runs ping from probes in Germany and the United States.</para>
/// </example>
/// <example>
///   <summary>Request five packets</summary>
///   <prefix>PS> </prefix>
///   <code>Start-GlobalpingPing -Target "example.com" -SimpleLocations "PL" -Options @{ Packets = 5 }</code>
///   <para>Uses <see cref="PingOptions"/> to set the packet count.</para>
/// </example>
/// <example>
///   <summary>Output classic text</summary>
///   <prefix>PS> </prefix>
///   <code>Start-GlobalpingPing -Target "example.com" -Classic</code>
///   <para>Displays the raw ping output returned by the probe.</para>
/// </example>
[Cmdlet(VerbsLifecycle.Start, "GlobalpingPing")]
[OutputType(typeof(PingTimingResult))]
[OutputType(typeof(string))]
[OutputType(typeof(MeasurementResponse))]
public class StartGlobalpingPingCommand : StartGlobalpingBaseCommand
{
    /// <para>Return the unprocessed measurement response.</para>
    /// <para>When set the cmdlet outputs the <see cref="MeasurementResponse"/> object returned by the API.</para>
    [Parameter]
    [Alias("AsRaw")]
    public SwitchParameter Raw { get; set; }

    /// <para>Output ping results in the classic text format.</para>
    /// <para>Each probe returns the textual output of its ping utility.</para>
    [Parameter]
    public SwitchParameter Classic { get; set; }

    /// <para>Additional ping options sent with the request.</para>
    /// <para>Use this to configure packet count or other low level settings.</para>
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
