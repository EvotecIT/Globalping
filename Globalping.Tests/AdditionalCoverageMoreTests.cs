using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using System.Reflection;
using Globalping.Examples;
using Spectre.Console.Rendering;
using Xunit;

namespace Globalping.Tests;

public class AdditionalCoverageMoreTests
{
    [Fact]
    public void IpVersionConverter_ReadsAndWrites()
    {
        var options = new JsonSerializerOptions();
        options.Converters.Add(new IpVersionConverter());
        var json = JsonSerializer.Serialize(IpVersion.Six, options);
        Assert.Equal("6", json);
        var value = JsonSerializer.Deserialize<IpVersion>("6", options);
        Assert.Equal(IpVersion.Six, value);
    }

    [Fact]
    public void ResultExtensions_ToTracerouteHops()
    {
        var hops = JsonSerializer.SerializeToElement(new object[]
        {
            new { resolvedHostname = "h1", resolvedAddress = "1.1.1.1", timings = new[]{ new { rtt = 1.0 } } },
            new { resolvedHostname = "h2", resolvedAddress = "2.2.2.2" }
        });
        var result = new Result
        {
            Probe = new Probe(),
            Data = new ResultDetails { Hops = hops }
        };
        var list = result.ToTracerouteHops("example.com");
        Assert.Equal(2, list.Count);
        Assert.Equal(1, list[0].Hop);
        Assert.Equal("h1", list[0].Host);
    }

    [Fact]
    public void ResultExtensions_ToDnsRecords()
    {
        var result = new Result
        {
            Probe = new Probe(),
            Data = new ResultDetails
            {
                Answers = new List<DnsAnswer>
                {
                    new() { Name = "example.com", Type = "A", Ttl = 60, Class = "IN", Value = "1.1.1.1" }
                },
                RawOutput = ";; ->>HEADER<<- opcode: QUERY, status: NOERROR, id: 1\n;; flags: qr rd ra; QUERY: 1, ANSWER: 1, AUTHORITY: 0, ADDITIONAL: 0\n\n;; QUESTION SECTION:\n;example.com. IN A\n"
            }
        };
        var records = result.ToDnsRecords("example.com");
        Assert.Single(records);
        Assert.Equal("example.com", records[0].Name);
        Assert.Equal("A", records[0].Type);
        Assert.Equal("qr rd ra", records[0].Flags);
    }

    [Fact]
    public void ResultExtensions_ToMtrHops()
    {
        var hops = JsonSerializer.SerializeToElement(new object[]
        {
            new { resolvedHostname = "h1", resolvedAddress = "1.1.1.1", asn = new[]{64500,64501} },
            new { resolvedHostname = "h2", resolvedAddress = "2.2.2.2", asn = 64502 }
        });
        var result = new Result
        {
            Probe = new Probe(),
            Data = new ResultDetails { Hops = hops }
        };
        var list = result.ToMtrHops("example.com");
        Assert.Equal(2, list.Count);
        Assert.Equal(new List<int>{64500,64501}, Assert.IsType<List<int>>(list[0].Asn));
        Assert.Equal(64502, Assert.IsType<int>(list[1].Asn));
    }

    [Fact]
    public void ParseMtr_WithRawHosts_OverridesJson()
    {
        var hops = JsonSerializer.SerializeToElement(new[]
        {
            new { resolvedHostname = "ignored", resolvedAddress = "1.1.1.1" }
        });
        var raw = "header\nHost\n1. AS100 router (1.1.1.1) 0.0% 1 1 1.0 1.0 1.0";
        var resp = new MeasurementResponse
        {
            Target = "example.com",
            Results = new List<Result>
            {
                new()
                {
                    Probe = new Probe(),
                    Data = new ResultDetails { Hops = hops, RawOutput = raw }
                }
            }
        };
        var list = resp.GetMtrHops();
        Assert.Single(list);
        Assert.Equal("router", list[0].Host);
    }

    private sealed class StubHandler : HttpMessageHandler
    {
        private readonly HttpResponseMessage _response;
        public StubHandler(HttpResponseMessage response) { _response = response; }
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return Task.FromResult(_response);
        }
    }

    [Fact]
    public async Task MeasurementClient_AssignsTarget()
    {
        const string json = "{\"id\":\"1\",\"type\":\"ping\",\"status\":\"finished\",\"target\":\"example.com\",\"probesCount\":1,\"results\":[{\"probe\":{},\"result\":{\"status\":\"finished\"}}]}";
        var response = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(json, Encoding.UTF8, "application/json")
        };
        var client = new HttpClient(new StubHandler(response));
        var measurementClient = new MeasurementClient(client);
        var result = await measurementClient.GetMeasurementByIdAsync("1");
        Assert.NotNull(result);
        Assert.NotNull(result!.Results);
        Assert.Equal(result.Target, result.Results![0].Target);
    }

    private static object InvokePrivate(string name, object? arg)
    {
        var method = typeof(ConsoleHelpers).GetMethod(name, BindingFlags.NonPublic | BindingFlags.Static);
        Assert.NotNull(method);
        return method!.Invoke(null, new[] { arg! })!;
    }

    [Fact]
    public void RenderJsonElement_Object_ReturnsRenderable()
    {
        using var doc = JsonDocument.Parse("{\"a\":1}");
        var element = doc.RootElement;
        var renderable = InvokePrivate("RenderJsonElement", element);
        Assert.IsAssignableFrom<IRenderable>(renderable);
    }

    [Fact]
    public void RenderJsonElement_Array_ReturnsRenderable()
    {
        using var doc = JsonDocument.Parse("[1,2,3]");
        var element = doc.RootElement;
        var renderable = InvokePrivate("RenderJsonElement", element);
        Assert.IsAssignableFrom<IRenderable>(renderable);
    }

    [Fact]
    public void WithLocations_OverridesReuseId()
    {
        var builder = new MeasurementRequestBuilder()
            .WithType(MeasurementType.Ping)
            .WithTarget("example.com")
            .ReuseLocationsFromId("old")
            .WithLocations(new[]{ new LocationRequest{ Country="DE" } });
        var request = builder.Build();
        Assert.Null(request.ReuseLocationsFromId);
        Assert.NotNull(request.Locations);
        Assert.Single(request.Locations!);
    }

    [Fact]
    public void ParseHttp_UsesStructuredHeaders()
    {
        var headers = new Dictionary<string, JsonElement>
        {
            ["Content-Type"] = JsonSerializer.SerializeToElement("text/plain"),
            ["X-Test"] = JsonSerializer.SerializeToElement(new[] { "a", "b" })
        };
        var resp = new MeasurementResponse
        {
            Target = "example.com",
            Results = new List<Result>
            {
                new()
                {
                    Probe = new Probe(),
                    Data = new ResultDetails
                    {
                        RawHeaders = string.Empty,
                        Headers = headers,
                        RawBody = "Hello",
                        StatusCode = 200,
                        StatusCodeName = "OK"
                    }
                }
            }
        };
        var list = resp.GetHttpResponses();
        Assert.Single(list);
        var http = list[0];
        Assert.Equal(200, http.StatusCode);
        Assert.Equal("Hello", http.Body);
        Assert.True(http.Headers.ContainsKey("Content-Type"));
        var xTest = Assert.IsType<List<string>>(http.Headers["X-Test"]!);
        Assert.Equal(2, xTest.Count);
    }
}
