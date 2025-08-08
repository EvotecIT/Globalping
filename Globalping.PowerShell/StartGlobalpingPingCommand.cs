using System.Management.Automation;
using Globalping;

namespace Globalping.PowerShell;

/// <summary>Start a ping measurement using Globalping.</summary>
/// <para>Instructs remote probes to send ICMP echo requests to the specified target.</para>
/// <para>The cmdlet outputs <see cref="PingTimingResult"/> objects, raw results or classic text depending on the selected parameters.</para>
/// <list type="alertSet">
///   <item>
///     <term>Note</term>
///     <description>Some networks block ICMP traffic which may affect results.</description>
///   </item>
/// </list>
/// <example>
///   <summary>Ping from multiple locations</summary>
///   <code>
///     <para><prefix>PS&gt; </prefix>Start-GlobalpingPing -Target "evotec.xyz" -SimpleLocations "DE", "US"</para>
///   </code>
///   <para>Runs ping from probes in Germany and the United States.</para>
/// </example>
/// <example>
///   <summary>Request five packets</summary>
///   <code>
///     <para><prefix>PS&gt; </prefix>Start-GlobalpingPing -Target "example.com" -SimpleLocations "PL" -Options @{ Packets = 5 }</para>
///   </code>
///   <para>Uses <see cref="PingOptions"/> to set the packet count.</para>
/// </example>
/// <example>
///   <summary>Output classic text</summary>
///   <code>
///     <para><prefix>PS&gt; </prefix>Start-GlobalpingPing -Target "example.com" -Classic</para>
///   </code>
///   <para>Displays the raw ping output returned by the probe.</para>
/// </example>
/// <seealso href="https://learn.microsoft.com/powershell/" />
/// <seealso href="https://github.com/EvotecIT/Globalping" />
[Cmdlet(VerbsLifecycle.Start, "GlobalpingPing")]
[OutputType(typeof(PingTimingResult))]
[OutputType(typeof(string))]
[OutputType(typeof(MeasurementResponse))]
public class StartGlobalpingPingCommand : StartGlobalpingBaseCommand
{
    /// <summary>Return the unprocessed measurement response.</summary>
    /// <para>When set the cmdlet outputs the <see cref="MeasurementResponse"/> object returned by the API.</para>
    [Parameter]
    [Alias("AsRaw")]
    public SwitchParameter Raw { get; set; }

    /// <summary>Output ping results in the classic text format.</summary>
    /// <para>Each probe returns the textual output of its ping utility.</para>
    [Parameter]
    public SwitchParameter Classic { get; set; }

    /// <summary>Additional ping options sent with the request.</summary>
    /// <para>Use this to configure packet count or other low level settings.</para>
    [Parameter]
    public PingOptions? Options { get; set; }

    /// <inheritdoc/>
    protected override MeasurementType Type => MeasurementType.Ping;

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
