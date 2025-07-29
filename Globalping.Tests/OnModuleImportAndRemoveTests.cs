using System;
using System.IO;
using System.Reflection;
using Globalping.PowerShell;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Xunit;

namespace Globalping.Tests;

public class OnModuleImportAndRemoveTests
{
    [Fact]
    public void ResolverLoadsAssemblyFromCustomDirectory()
    {
        var initializer = new OnModuleImportAndRemove();
        initializer.OnImport();

        var libDir = Path.GetDirectoryName(typeof(OnModuleImportAndRemove).Assembly.Location)!;
        var tempDir = Path.Combine(libDir, Guid.NewGuid().ToString());
        Directory.CreateDirectory(tempDir);
        var asmName = "TempAssembly" + Guid.NewGuid().ToString("N");
        var asmPath = Path.Combine(tempDir, asmName + ".dll");
        BuildDummyAssembly(asmName, asmPath);

        try
        {
            if (IsNetFramework())
            {
                var asm = Assembly.Load(asmName);
                Assert.Equal(asmName, asm.GetName().Name);
            }
            else
            {
                Assert.Throws<FileNotFoundException>(() => Assembly.Load(asmName));
            }
        }
        finally
        {
            initializer.OnRemove(null!);
            TryDelete(tempDir);
        }
    }

    [Fact]
    public void ResolverIsRemovedAfterOnRemove()
    {
        var initializer = new OnModuleImportAndRemove();
        initializer.OnImport();

        var libDir = Path.GetDirectoryName(typeof(OnModuleImportAndRemove).Assembly.Location)!;
        var tempDir = Path.Combine(libDir, Guid.NewGuid().ToString());
        Directory.CreateDirectory(tempDir);
        var asmName = "TempAssembly" + Guid.NewGuid().ToString("N");
        var asmPath = Path.Combine(tempDir, asmName + ".dll");
        BuildDummyAssembly(asmName, asmPath);
        initializer.OnRemove(null!);

        try
        {
            Assert.Throws<FileNotFoundException>(() => Assembly.Load(asmName));
        }
        finally
        {
            TryDelete(tempDir);
        }
    }

    private static void BuildDummyAssembly(string name, string path)
    {
        var syntaxTree = CSharpSyntaxTree.ParseText("public class Dummy {}");
        var references = new[] { MetadataReference.CreateFromFile(typeof(object).Assembly.Location) };
        var compilation = CSharpCompilation.Create(name, new[] { syntaxTree }, references, new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
        var result = compilation.Emit(path);
        if (!result.Success)
        {
            throw new InvalidOperationException(string.Join(Environment.NewLine, result.Diagnostics));
        }
    }

    private static bool IsNetFramework() => System.Runtime.InteropServices.RuntimeInformation.FrameworkDescription.StartsWith(".NET Framework", StringComparison.OrdinalIgnoreCase);

    private static void TryDelete(string path)
    {
        try
        {
            Directory.Delete(path, true);
        }
        catch (IOException)
        {
        }
        catch (UnauthorizedAccessException)
        {
        }
    }
}
