using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Globalping.Tests;

public class AdditionalCoverageTests
{
    private sealed class StubHandler : HttpMessageHandler
    {
        public HttpRequestMessage? LastRequest { get; private set; }
        private readonly HttpResponseMessage _response;
        public StubHandler(HttpResponseMessage response)
        {
            _response = response;
        }
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            LastRequest = request;
            return Task.FromResult(_response);
        }
    }

    private sealed class CaptureHandler : HttpMessageHandler
    {
        public HttpRequestMessage? LastRequest { get; private set; }
        public string? Payload { get; private set; }
        private readonly HttpResponseMessage _response;
        public CaptureHandler(HttpResponseMessage response)
        {
            _response = response;
        }
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            LastRequest = request;
            if (request.Content != null)
            {
                Payload = await request.Content.ReadAsStringAsync(cancellationToken);
            }
            return _response;
        }
    }

    [Fact]
    public void GetPingTimings_ReturnsEmptyWhenNoResults()
    {
        var resp = new MeasurementResponse { Target = "example.com" };
        var list = resp.GetPingTimings();
        Assert.Empty(list);
    }

    [Fact]
    public void GetTracerouteHops_ReturnsEmptyWhenNoResults()
    {
        var resp = new MeasurementResponse { Target = "example.com" };
        var hops = resp.GetTracerouteHops();
        Assert.Empty(hops);
    }

    [Fact]
    public void GetHttpResponses_ReturnsEmptyWhenNoResults()
    {
        var resp = new MeasurementResponse { Target = "example.com" };
        var list = resp.GetHttpResponses();
        Assert.Empty(list);
    }

    [Fact]
    public void GetDnsRecords_ParsesHeaderInfoFromRaw()
    {
        var raw = ";; ->>HEADER<<- opcode: QUERY, status: NOERROR, id: 1\n" +
                  ";; flags: qr rd ra; QUERY: 1, ANSWER: 2, AUTHORITY: 0, ADDITIONAL: 0\n\n" +
                  ";; QUESTION SECTION:\n" +
                  ";example.com. IN A\n\n" +
                  ";; ANSWER SECTION:\n" +
                  "example.com. 60 IN A 93.184.216.34\n" +
                  "example.com. 60 IN A 93.184.216.35\n";
        var resp = new MeasurementResponse
        {
            Target = "example.com",
            Results = new List<Result>
            {
                new()
                {
                    Probe = new Probe(),
                    Data = new ResultDetails { RawOutput = raw }
                }
            }
        };

        var records = resp.GetDnsRecords();
        Assert.Equal(2, records.Count);
        var rec = records[0];
        Assert.Equal("qr rd ra", rec.Flags);
        Assert.Equal("example.com", rec.QuestionName);
        Assert.Equal("A", rec.QuestionType);
        Assert.Equal(1, rec.QueryCount);
        Assert.Equal(2, rec.AnswerCount);
        Assert.Equal(0, rec.AuthorityCount);
        Assert.Equal(0, rec.AdditionalCount);
        Assert.Equal("QUERY", rec.Opcode);
        Assert.Equal("NOERROR", rec.HeaderStatus);
    }

    [Fact]
    public void GetOnlineProbes_SynchronousReturnsData()
    {
        var json = "[{\"version\":\"1.0\",\"location\":{\"continent\":\"EU\",\"region\":\"EU\",\"country\":\"DE\",\"state\":\"BY\",\"city\":\"Berlin\",\"asn\":1,\"network\":\"net\",\"latitude\":1.2,\"longitude\":3.4},\"tags\":[\"edge\"],\"resolvers\":[\"1.1.1.1\"]}]";
        var response = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(json, Encoding.UTF8, "application/json")
        };
        var handler = new StubHandler(response);
        var client = new HttpClient(handler);
        var service = new ProbeService(client);

        var probes = service.GetOnlineProbes();

        Assert.Single(probes);
        Assert.Equal("Berlin", probes[0].Location!.City);
    }

    [Fact]
    public void GetLimits_SynchronousReturnsData()
    {
        var json = "{\"rateLimit\":{\"measurements\":{\"create\":{\"type\":\"ip\",\"limit\":10,\"remaining\":8,\"reset\":1000}}},\"credits\":{\"remaining\":42}}";
        var response = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(json, Encoding.UTF8, "application/json")
        };
        var handler = new StubHandler(response);
        var client = new HttpClient(handler);
        var service = new ProbeService(client);

        var limits = service.GetLimits();

        Assert.NotNull(limits);
        Assert.Equal(42, limits!.Credits!.Remaining);
        Assert.Equal(10, limits.RateLimit["measurements"]["create"].Limit);
    }

    [Fact]
    public void CreateMeasurement_SynchronousUsesPreferHeader()
    {
        var respMsg = new HttpResponseMessage(HttpStatusCode.Accepted)
        {
            Content = new StringContent("{\"id\":\"1\",\"probesCount\":1}", Encoding.UTF8, "application/json")
        };
        var handler = new CaptureHandler(respMsg);
        var client = new HttpClient(handler);
        var service = new ProbeService(client);

        var request = new MeasurementRequestBuilder()
            .WithType(MeasurementType.Ping)
            .WithTarget("example.com")
            .Build();
        request.InProgressUpdates = true;

        var result = service.CreateMeasurement(request, 33);

        Assert.NotNull(result);
        Assert.NotNull(handler.LastRequest);
        Assert.True(handler.LastRequest!.Headers.Contains("Prefer"));
        Assert.Contains("respond-async, wait=33", handler.LastRequest.Headers.GetValues("Prefer"));
        Assert.NotNull(handler.Payload);
        Assert.Contains("\"target\":\"example.com\"", handler.Payload);
    }

    [Fact]
    public void MeasurementClient_GetMeasurementById_Synchronous()
    {
        const string json = "{\"id\":\"1\",\"type\":\"ping\",\"status\":\"finished\",\"target\":\"example.com\",\"probesCount\":0}";
        var response = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(json, Encoding.UTF8, "application/json")
        };
        var handler = new StubHandler(response);
        var client = new HttpClient(handler);
        var measurementClient = new MeasurementClient(client);

        var result = measurementClient.GetMeasurementById("1");

        Assert.NotNull(result);
        Assert.Equal("example.com", result!.Target);
    }

    [Fact]
    public void MeasurementRequestBuilder_AddsContinentAndRegion()
    {
        var request = new MeasurementRequestBuilder()
            .WithType(MeasurementType.Ping)
            .WithTarget("example.com")
            .AddContinent(ContinentCode.EU)
            .AddRegion(RegionName.NorthernEurope, 2)
            .Build();

        Assert.Equal(2, request.Locations!.Count);
        Assert.Equal(ContinentCode.EU, request.Locations[0].Continent);
        Assert.Null(request.Locations[0].Limit);
        Assert.Equal(RegionName.NorthernEurope, request.Locations[1].Region);
        Assert.Equal(2, request.Locations[1].Limit);
    }

    [Fact]
    public void Flatten_HandlesMultipleCategories()
    {
        var limits = new Limits
        {
            RateLimit = new Dictionary<string, Dictionary<string, RateLimitDetails>>
            {
                ["measurements"] = new Dictionary<string, RateLimitDetails>
                {
                    ["create"] = new RateLimitDetails { Type = RateLimitType.Ip, Limit = 10, Remaining = 9, Reset = 100 }
                },
                ["probes"] = new Dictionary<string, RateLimitDetails>
                {
                    ["list"] = new RateLimitDetails { Type = RateLimitType.Ip, Limit = 5, Remaining = 4, Reset = 200 }
                }
            }
        };

        var infos = new List<LimitInfo>(limits.Flatten());
        Assert.Equal(2, infos.Count);
        var info1 = infos.Find(i => i.Action == "create")!;
        Assert.Equal("measurements", info1.Category);
        Assert.Null(info1.CreditsRemaining);
        var info2 = infos.Find(i => i.Action == "list")!;
        Assert.Equal("probes", info2.Category);
        Assert.Equal(5, info2.Limit);
    }
}
