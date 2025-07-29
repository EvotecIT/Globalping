using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
#if NET8_0_OR_GREATER
using Globalping.Examples;
using Spectre.Console;
using Spectre.Console.Rendering;
#endif
using Xunit;

namespace Globalping.Tests;

#if NET8_0_OR_GREATER
public class AdditionalCoverageExtraTests
{
    [Fact]
    public void ProbeService_CtorWithApiKey_AddsAuthorization()
    {
        var client = new HttpClient(new HttpClientHandler());
        var service = new ProbeService(client, "token123");
        Assert.NotNull(client.DefaultRequestHeaders.Authorization);
        Assert.Equal("Bearer", client.DefaultRequestHeaders.Authorization!.Scheme);
        Assert.Equal("token123", client.DefaultRequestHeaders.Authorization!.Parameter);
    }

    [Fact]
    public void MeasurementClient_CtorWithApiKey_AddsAuthorization()
    {
        var client = new HttpClient(new HttpClientHandler());
        var mc = new MeasurementClient(client, "abc");
        Assert.NotNull(client.DefaultRequestHeaders.Authorization);
        Assert.Equal("Bearer", client.DefaultRequestHeaders.Authorization!.Scheme);
        Assert.Equal("abc", client.DefaultRequestHeaders.Authorization!.Parameter);
    }

    [Fact]
    public void ProbeService_Ctor_SetsDefaultHeaders()
    {
        var client = new HttpClient(new HttpClientHandler());
        _ = new ProbeService(client);
        Assert.True(client.DefaultRequestHeaders.UserAgent.Count > 0);
        Assert.True(client.DefaultRequestHeaders.AcceptEncoding.Count > 0);
    }

    [Fact]
    public void MeasurementClient_Ctor_SetsDefaultHeaders()
    {
        var client = new HttpClient(new HttpClientHandler());
        _ = new MeasurementClient(client);
        Assert.True(client.DefaultRequestHeaders.UserAgent.Count > 0);
        Assert.True(client.DefaultRequestHeaders.AcceptEncoding.Count > 0);
    }

    [Fact]
    public async Task GetMeasurementByIdAsync_ThrowsOnEmptyId()
    {
        var client = new HttpClient(new HttpClientHandler());
        var mc = new MeasurementClient(client);
        await Assert.ThrowsAsync<ArgumentException>(() => mc.GetMeasurementByIdAsync(""));
    }

    [Fact]
    public void GetSummaries_ReturnsEmptyWhenNoResults()
    {
        var resp = new MeasurementResponse { Target = "example.com" };
        var list = resp.GetSummaries();
        Assert.Empty(list);
    }

    [Fact]
    public void AddLocation_AppendsLocation()
    {
        var request = new MeasurementRequestBuilder()
            .WithType(MeasurementType.Ping)
            .WithTarget("example.com")
            .AddLocation(new LocationRequest { Country = CountryCode.Germany, Limit = 1 })
            .Build();
        Assert.Single(request.Locations!);
        Assert.Equal(CountryCode.Germany, request.Locations![0].Country);
        Assert.Equal(1, request.Locations![0].Limit);
    }

    private static object Invoke(string name, object arg)
    {
        var method = typeof(ConsoleHelpers).GetMethod(name, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
        Assert.NotNull(method);
        return method!.Invoke(null, new[] { arg })!;
    }

    [Fact]
    public void RenderValue_String_ReturnsMarkup()
    {
        var renderable = Invoke("RenderValue", "test");
        Assert.IsType<Markup>(renderable);
    }

    [Fact]
    public void RenderValue_List_ReturnsTable()
    {
        var list = new[] { 1, 2, 3 };
        var renderable = Invoke("RenderValue", list);
        Assert.IsType<Table>(renderable);
    }

    [Fact]
    public void WriteJson_DoesNotThrow()
    {
        var obj = new { a = 1 };
        ConsoleHelpers.WriteJson(obj, "title");
    }
}
#endif
