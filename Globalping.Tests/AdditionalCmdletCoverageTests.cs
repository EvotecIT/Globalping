using System.Reflection;
using System.Runtime.InteropServices;
using System.Net.Http;
using Globalping.PowerShell;
using Xunit;

namespace Globalping.Tests;

public class AdditionalCmdletCoverageTests
{
    [Fact]
    public void GetGlobalpingLimitCommand_ProcessRecord_DoesNotThrow()
    {
        var cmd = new GetGlobalpingLimitCommand
        {
            CommandRuntime = new TestCommandRuntime()
        };
        var method = typeof(GetGlobalpingLimitCommand)
            .GetMethod("ProcessRecord", BindingFlags.Instance | BindingFlags.NonPublic)!;
        try
        {
            method.Invoke(cmd, null);
        }
        catch (TargetInvocationException ex) when (ex.InnerException is HttpRequestException)
        {
            // ignore network failures
        }
    }

    [Fact]
    public void GetGlobalpingProbeCommand_ProcessRecord_DoesNotThrow()
    {
        var cmd = new GetGlobalpingProbeCommand
        {
            CommandRuntime = new TestCommandRuntime()
        };
        var method = typeof(GetGlobalpingProbeCommand)
            .GetMethod("ProcessRecord", BindingFlags.Instance | BindingFlags.NonPublic)!;
        try
        {
            method.Invoke(cmd, null);
        }
        catch (TargetInvocationException ex) when (ex.InnerException is HttpRequestException)
        {
            // ignore network failures
        }
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

        var desc = RuntimeInformation.FrameworkDescription;
        bool expectedFramework = desc.StartsWith(".NET Framework", StringComparison.OrdinalIgnoreCase);
        bool expectedCore = desc.StartsWith(".NET Core", StringComparison.OrdinalIgnoreCase);
        bool expectedModern = desc.StartsWith(".NET 5", StringComparison.OrdinalIgnoreCase)
            || desc.StartsWith(".NET 6", StringComparison.OrdinalIgnoreCase)
            || desc.StartsWith(".NET 7", StringComparison.OrdinalIgnoreCase)
            || desc.StartsWith(".NET 8", StringComparison.OrdinalIgnoreCase)
            || desc.StartsWith(".NET 9", StringComparison.OrdinalIgnoreCase);

        Assert.Equal(expectedFramework, isFramework);
        Assert.Equal(expectedCore, isCore);
        Assert.Equal(expectedModern, isModern);
    }
}
