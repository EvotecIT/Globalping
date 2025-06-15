using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Globalping.Tests;

public class ProbeServiceCoreTests
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

    [Fact]
    public async Task GetOnlineProbesAsync_ParsesProbeList()
    {
        var json = "[{\"version\":\"1.0\",\"location\":{\"continent\":\"EU\",\"region\":\"EU\",\"country\":\"DE\",\"state\":\"BY\",\"city\":\"Berlin\",\"asn\":1,\"network\":\"net\",\"latitude\":1.2,\"longitude\":3.4},\"tags\":[\"edge\"],\"resolvers\":[\"1.1.1.1\"]}]";
        var response = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(json, Encoding.UTF8, "application/json")
        };
        var handler = new StubHandler(response);
        var client = new HttpClient(handler);
        var service = new ProbeService(client);

        var probes = await service.GetOnlineProbesAsync();

        Assert.Single(probes);
        Assert.Equal("1.0", probes[0].Version);
        Assert.NotNull(probes[0].Location);
        Assert.Equal("Berlin", probes[0].Location!.City);
        Assert.Contains("edge", probes[0].Tags!);
    }

    [Fact]
    public async Task GetLimitsAsync_ParsesLimits()
    {
        var json = "{\"rateLimit\":{\"measurements\":{\"create\":{\"type\":\"ip\",\"limit\":10,\"remaining\":8,\"reset\":1000}}},\"credits\":{\"remaining\":42}}";
        var response = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(json, Encoding.UTF8, "application/json")
        };
        var handler = new StubHandler(response);
        var client = new HttpClient(handler);
        var service = new ProbeService(client);

        var limits = await service.GetLimitsAsync();

        Assert.NotNull(limits);
        Assert.Equal(42, limits!.Credits!.Remaining);
        Assert.Equal(10, limits.RateLimit["measurements"]["create"].Limit);
    }
}
