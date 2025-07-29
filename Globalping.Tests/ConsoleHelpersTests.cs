using System.Collections.Generic;
using System.Text.Json;
using System.Reflection;
#if NET8_0_OR_GREATER
using Globalping.Examples;
using Spectre.Console.Rendering;
#endif
using Xunit;

namespace Globalping.Tests;

#if NET8_0_OR_GREATER
public class ConsoleHelpersTests
{
    [Fact]
    public void CreateTable_AcceptsGenericDictionary()
    {
        using var doc = JsonDocument.Parse("{\"key\":\"value\"}");
        var element = doc.RootElement.GetProperty("key");
        var data = new Dictionary<string, JsonElement> { ["key"] = element };

        var method = typeof(ConsoleHelpers).GetMethod("CreateTable", BindingFlags.NonPublic | BindingFlags.Static);
        Assert.NotNull(method);
        var table = method!.Invoke(null, new object[] { data });

        Assert.IsAssignableFrom<IRenderable>(table);
    }
}
#endif
