using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text.Json;
using Xunit;

namespace Globalping.Tests;

public class ParsingHelpersTests
{
    private static HttpProtocolVersion InvokeParseHttpVersion(string version)
    {
        var method = typeof(MeasurementResponseExtensions).GetMethod(
            "ParseHttpVersion",
            BindingFlags.NonPublic | BindingFlags.Static);
        Assert.NotNull(method);
        return (HttpProtocolVersion)method!.Invoke(null, new object[] { version })!;
    }

    private static object? InvokeParseJson(string value)
    {
        var method = typeof(MeasurementResponseExtensions).GetMethod(
            "ParseJson",
            BindingFlags.NonPublic | BindingFlags.Static);
        Assert.NotNull(method);
        return method!.Invoke(null, new object[] { value });
    }

    private static object? InvokeConvertElement(JsonElement element)
    {
        var method = typeof(MeasurementResponseExtensions).GetMethod(
            "ConvertElement",
            BindingFlags.NonPublic | BindingFlags.Static);
        Assert.NotNull(method);
        return method!.Invoke(null, new object[] { element });
    }

    [Theory]
    [InlineData("HTTP/1.0", HttpProtocolVersion.Http10)]
    [InlineData("HTTP/1.1", HttpProtocolVersion.Http11)]
    [InlineData("HTTP/2", HttpProtocolVersion.Http20)]
    [InlineData("HTTP/2.0", HttpProtocolVersion.Http20)]
    [InlineData("HTTP/3", HttpProtocolVersion.Http30)]
    [InlineData("HTTP/3.0", HttpProtocolVersion.Http30)]
    [InlineData("HTTP/0.9", HttpProtocolVersion.Unknown)]
    [InlineData("INVALID", HttpProtocolVersion.Unknown)]
    public void ParseHttpVersion_ReturnsExpected(string input, HttpProtocolVersion expected)
    {
        Assert.Equal(expected, InvokeParseHttpVersion(input));
    }

    [Fact]
    public void ParseJson_Object_ReturnsDictionary()
    {
        var result = InvokeParseJson("{\"a\":1}");
        var dict = Assert.IsType<Dictionary<string, object?>>(result);
        Assert.True(dict.TryGetValue("a", out var value) && value is long l && l == 1);
    }

    [Fact]
    public void ParseJson_SingleElementArray_ReturnsItem()
    {
        var result = InvokeParseJson("[1]");
        Assert.Equal(1L, result);
    }

    [Fact]
    public void ParseJson_MultipleElementArray_ReturnsList()
    {
        var result = InvokeParseJson("[1,2]");
        var list = Assert.IsType<List<object?>>(result);
        Assert.Equal(new object?[] { 1L, 2L }, list);
    }

    [Fact]
    public void ParseJson_InvalidInput_ReturnsOriginal()
    {
        var result = InvokeParseJson("{ invalid }");
        Assert.Equal("{ invalid }", result);
    }

    [Fact]
    public void ConvertElement_ParsesPrimitiveValues()
    {
        using var doc = JsonDocument.Parse("[true, null, 1.5]");
        var array = doc.RootElement;
        var list = Assert.IsType<List<object?>>(InvokeConvertElement(array));
        Assert.Equal(new object?[] { true, null, 1.5 }, list);
    }
}
