using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Globalping.Tests;

public class UsageHeaderTests
{
    private sealed class MockHandler : HttpMessageHandler
    {
        private readonly HttpResponseMessage _response;
        public MockHandler(HttpResponseMessage response)
        {
            _response = response;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return Task.FromResult(_response);
        }
    }

    [Fact]
    public async Task ProbeService_CapturesHeaders()
    {
        var response = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent("[]", Encoding.UTF8, "application/json")
        };
        response.Headers.Add("X-RateLimit-Limit", "10");
        response.Headers.Add("X-RateLimit-Consumed", "1");
        response.Headers.Add("X-RateLimit-Remaining", "9");
        response.Headers.Add("X-RateLimit-Reset", "100");
        response.Headers.Add("X-Credits-Consumed", "2");
        response.Headers.Add("X-Credits-Remaining", "8");
        response.Headers.Add("X-Request-Cost", "2");

        var client = new HttpClient(new MockHandler(response));
        var service = new ProbeService(client);
        var probes = await service.GetOnlineProbesAsync();
        Assert.Empty(probes);
        Assert.Equal(10, service.LastResponseInfo.RateLimitLimit);
        Assert.Equal(1, service.LastResponseInfo.RateLimitConsumed);
        Assert.Equal(9, service.LastResponseInfo.RateLimitRemaining);
        Assert.Equal(100, service.LastResponseInfo.RateLimitReset);
        Assert.Equal(2, service.LastResponseInfo.CreditsConsumed);
        Assert.Equal(8, service.LastResponseInfo.CreditsRemaining);
        Assert.Equal(2, service.LastResponseInfo.RequestCost);
    }

    [Fact]
    public async Task MeasurementClient_CapturesHeaders()
    {
        const string json = "{\"id\":\"1\",\"type\":\"ping\",\"status\":\"finished\",\"target\":\"example.com\",\"probesCount\":0}";
        var response = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(json, Encoding.UTF8, "application/json")
        };
        response.Headers.Add("X-RateLimit-Limit", "5");
        response.Headers.Add("X-RateLimit-Consumed", "1");
        response.Headers.Add("X-RateLimit-Remaining", "4");
        response.Headers.Add("X-RateLimit-Reset", "200");
        response.Headers.Add("X-Credits-Consumed", "3");
        response.Headers.Add("X-Credits-Remaining", "7");
        response.Headers.Add("X-Request-Cost", "1");

        var client = new HttpClient(new MockHandler(response));
        var measurementClient = new MeasurementClient(client);
        var result = await measurementClient.GetMeasurementByIdAsync("1");
        Assert.NotNull(result);
        Assert.Equal(5, measurementClient.LastResponseInfo.RateLimitLimit);
        Assert.Equal(1, measurementClient.LastResponseInfo.RateLimitConsumed);
        Assert.Equal(4, measurementClient.LastResponseInfo.RateLimitRemaining);
        Assert.Equal(200, measurementClient.LastResponseInfo.RateLimitReset);
        Assert.Equal(3, measurementClient.LastResponseInfo.CreditsConsumed);
        Assert.Equal(7, measurementClient.LastResponseInfo.CreditsRemaining);
        Assert.Equal(1, measurementClient.LastResponseInfo.RequestCost);
    }
}
