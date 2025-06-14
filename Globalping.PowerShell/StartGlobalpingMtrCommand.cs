using System.Management.Automation;
using Globalping;

namespace Globalping.PowerShell;

/// <summary>Start an MTR (My Traceroute) measurement using Globalping.</summary>
/// <para>Combines ping and traceroute information from multiple probes.</para>
/// <example>
///   <summary>Run MTR against a host</summary>
///   <prefix>PS> </prefix>
///   <code>Start-GlobalpingMtr -Target "evotec.xyz"</code>
///   <para>Produces hop statistics for the target.</para>
/// </example>
/// <example>
///   <summary>Limit packets</summary>
///   <prefix>PS> </prefix>
///   <code>Start-GlobalpingMtr -Target "example.com" -Options @{ Packets = 3 }</code>
///   <para>Configures the probe to send three packets per hop.</para>
/// </example>
[Cmdlet(VerbsLifecycle.Start, "GlobalpingMtr")]
[OutputType(typeof(MtrHopResult))]
[OutputType(typeof(string))]
[OutputType(typeof(MeasurementResponse))]
public class StartGlobalpingMtrCommand : StartGlobalpingBaseCommand
{
    /// <para>Return the raw measurement response.</para>
    /// <para>Useful when converting the result to custom objects.</para>
    [Parameter]
    [Alias("AsRaw")]
    public SwitchParameter Raw { get; set; }

    /// <para>Output MTR results in classic text form.</para>
    /// <para>The text output mirrors the behaviour of the MTR command line tool.</para>
    [Parameter]
    public SwitchParameter Classic { get; set; }

    /// <para>Additional MTR options for the request.</para>
    /// <para>Use to configure packet count or other low level parameters.</para>
    [Parameter]
    public MtrOptions? Options { get; set; }

    /// <inheritdoc/>
    protected override MeasurementType Type => MeasurementType.Mtr;

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
            foreach (var hop in result.GetMtrHops())
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
