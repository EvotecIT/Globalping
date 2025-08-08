using System.Management.Automation;
using Globalping;

namespace Globalping.PowerShell;

/// <summary>Start a traceroute measurement using Globalping.</summary>
/// <para>Collects hop information from remote probes with optional classic text output.</para>
/// <list type="alertSet">
///   <item>
///     <term>Note</term>
///     <description>Intermediate hops may drop packets, resulting in missing data.</description>
///   </item>
/// </list>
/// <example>
///   <summary>Traceroute to a host</summary>
///   <code>
///     <para><prefix>PS&gt; </prefix>Start-GlobalpingTraceroute -Target "evotec.xyz"</para>
///   </code>
///   <para>Performs traceroute to the target host.</para>
/// </example>
/// <example>
///   <summary>Return raw output</summary>
///   <code>
///     <para><prefix>PS&gt; </prefix>Start-GlobalpingTraceroute -Target "example.com" -Classic</para>
///   </code>
///   <para>Writes the traceroute text produced by the probe.</para>
/// </example>
/// <seealso href="https://learn.microsoft.com/powershell/" />
/// <seealso href="https://github.com/EvotecIT/Globalping" />
[Cmdlet(VerbsLifecycle.Start, "GlobalpingTraceroute")]
[OutputType(typeof(TracerouteHopResult))]
[OutputType(typeof(string))]
[OutputType(typeof(MeasurementResponse))]
public class StartGlobalpingTracerouteCommand : StartGlobalpingBaseCommand
{
    /// <summary>Return the full measurement response.</summary>
    /// <para>The object is not converted to individual hops.</para>
    [Parameter]
    [Alias("AsRaw")]
    public SwitchParameter Raw { get; set; }

    /// <summary>Output traceroute in classic text form.</summary>
    /// <para>Returns the text produced by the traceroute utility.</para>
    [Parameter]
    public SwitchParameter Classic { get; set; }

    /// <summary>Additional traceroute options for the request.</summary>
    /// <para>Allows packet size or protocol customization.</para>
    [Parameter]
    public TracerouteOptions? Options { get; set; }

    /// <inheritdoc/>
    protected override MeasurementType Type => MeasurementType.Traceroute;

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
            foreach (var hop in result.GetTracerouteHops())
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
