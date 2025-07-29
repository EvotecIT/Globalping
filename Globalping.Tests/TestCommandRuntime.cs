using System.Collections.Generic;
using System.Management.Automation;
using System.Management.Automation.Host;

namespace Globalping.Tests;

internal sealed class TestCommandRuntime : ICommandRuntime
{
    public List<object?> WrittenObjects { get; } = new();

    public PSHost Host => null!;
    public PSTransactionContext CurrentPSTransaction => null!;

    public bool ShouldContinue(string query, string caption, ref bool yesToAll, ref bool noToAll) => true;
    public bool ShouldContinue(string query, string caption) => true;
    public bool TransactionAvailable() => false;
    public bool ShouldProcess(string verboseDescription, string verboseWarning, string caption, out ShouldProcessReason shouldProcessReason)
    {
        shouldProcessReason = ShouldProcessReason.None;
        return true;
    }
    public bool ShouldProcess(string verboseDescription, string verboseWarning, string caption) => true;
    public bool ShouldProcess(string target, string action) => true;
    public bool ShouldProcess(string target) => true;
    public void ThrowTerminatingError(ErrorRecord errorRecord) { }
    public void WriteCommandDetail(string text) { }
    public void WriteDebug(string text) { }
    public void WriteError(ErrorRecord errorRecord) { }
    public void WriteInformation(object messageData, string[] tags) { }
    public void WriteInformation(InformationRecord informationRecord) { }
    public void WriteObject(object sendToPipeline, bool enumerateCollection) => WrittenObjects.Add(sendToPipeline);
    public void WriteObject(object sendToPipeline) => WrittenObjects.Add(sendToPipeline);
    public void WriteProgress(long sourceId, ProgressRecord progressRecord) { }
    public void WriteProgress(ProgressRecord progressRecord) { }
    public void WriteVerbose(string text) { }
    public void WriteWarning(string text) { }
}
