using System;
using System.Collections.Generic;
using System.Reflection;
using Xunit;

namespace Globalping.Tests;

public class ParsingExtensionsAdditionalTests {
    private static List<TracerouteHopResult> InvokeParseTracerouteRaw(string raw) {
        var method = typeof(MeasurementResponseExtensions).GetMethod(
            "ParseTracerouteRaw",
            BindingFlags.NonPublic | BindingFlags.Static);
        Assert.NotNull(method);
        return (List<TracerouteHopResult>)method!.Invoke(null, new object?[] { raw })!;
    }

    private static Dictionary<int, string> InvokeParseMtrRawHosts(string raw) {
        var method = typeof(MeasurementResponseExtensions).GetMethod(
            "ParseMtrRawHosts",
            BindingFlags.NonPublic | BindingFlags.Static);
        Assert.NotNull(method);
        return (Dictionary<int, string>)method!.Invoke(null, new object?[] { raw })!;
    }

    [Fact]
    public void ParseTracerouteRaw_ParsesLines() {
        var raw = "traceroute to example.com\n" +
                  "1 router1 (1.1.1.1) 1.0 ms 2.0 ms 3.0 ms\n" +
                  "2 router2 (2.2.2.2) 4.0 ms 5.0 ms\n";
        var list = InvokeParseTracerouteRaw(raw);
        Assert.Equal(2, list.Count);
        Assert.Equal(1, list[0].Hop);
        Assert.Equal("router1", list[0].Host);
        Assert.Equal(3.0, list[0].Time3);
    }

    [Fact]
    public void ParseMtrRawHosts_ParsesHosts() {
        var raw = "header\nHost\n" +
                  "1. AS123 router (1.1.1.1)\n" +
                  "2. ??? waiting for reply\n" +
                  "3. AS456 example (2.2.2.2)\n";
        var dict = InvokeParseMtrRawHosts(raw);
        Assert.Equal(3, dict.Count);
        Assert.Equal("router", dict[1]);
        Assert.Equal("waiting for reply", dict[2]);
        Assert.Equal("example", dict[3]);
    }
}