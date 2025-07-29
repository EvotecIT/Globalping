using System.Reflection;
using Globalping.PowerShell;
using Xunit;

namespace Globalping.Tests;

public class AdditionalCmdletCoverageTests
{
    [Fact]
    public void GetGlobalpingLimitCommand_ProcessRecord_DoesNotThrow()
    {
        var cmd = new GetGlobalpingLimitCommand();
        typeof(GetGlobalpingLimitCommand)
            .GetMethod("ProcessRecord", BindingFlags.Instance | BindingFlags.NonPublic)!
            .Invoke(cmd, null);
    }

    [Fact]
    public void GetGlobalpingProbeCommand_ProcessRecord_DoesNotThrow()
    {
        var cmd = new GetGlobalpingProbeCommand();
        typeof(GetGlobalpingProbeCommand)
            .GetMethod("ProcessRecord", BindingFlags.Instance | BindingFlags.NonPublic)!
            .Invoke(cmd, null);
    }

    [Fact]
    public void OnModuleImportAndRemove_RuntimeChecks()
    {
        var obj = new OnModuleImportAndRemove();
        bool isFramework = (bool)typeof(OnModuleImportAndRemove)
            .GetMethod("IsNetFramework", BindingFlags.Instance | BindingFlags.NonPublic)!
            .Invoke(obj, null)!;
        bool isCore = (bool)typeof(OnModuleImportAndRemove)
            .GetMethod("IsNetCore", BindingFlags.Instance | BindingFlags.NonPublic)!
            .Invoke(obj, null)!;
        bool isModern = (bool)typeof(OnModuleImportAndRemove)
            .GetMethod("IsNet5OrHigher", BindingFlags.Instance | BindingFlags.NonPublic)!
            .Invoke(obj, null)!;
        Assert.False(isFramework);
        Assert.False(isCore);
        Assert.True(isModern);
    }
}
