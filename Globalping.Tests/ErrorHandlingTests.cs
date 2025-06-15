using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Globalping.Tests;

public sealed class ErrorHandlingTests
{
    private sealed class StubHandler : HttpMessageHandler
    {
        private readonly HttpResponseMessage _response;

        public StubHandler(HttpResponseMessage response)
        {
            _response = response;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return Task.FromResult(_response);
        }
    }

    [Fact]
    public async Task CreateMeasurementAsync_ThrowsOnBadRequest()
    {
        const string json = "{\"error\":{\"type\":\"validation_error\",\"message\":\"Invalid\"}}";
        var resp = new HttpResponseMessage(HttpStatusCode.BadRequest)
        {
            Content = new StringContent(json, Encoding.UTF8, "application/json")
        };

        var client = new HttpClient(new StubHandler(resp));
        var service = new ProbeService(client);
        var request = new MeasurementRequest();

        var ex = await Assert.ThrowsAsync<GlobalpingApiException>(() => service.CreateMeasurementAsync(request));
        Assert.Equal("validation_error", ex.Error.Type);
        Assert.Equal("Invalid", ex.Error.Message);
        Assert.Equal((int)HttpStatusCode.BadRequest, ex.StatusCode);
    }

    [Fact]
    public async Task GetMeasurementByIdAsync_ThrowsOnNotFound()
    {
        const string json = "{\"error\":{\"type\":\"not_found\",\"message\":\"Missing\"}}";
        var resp = new HttpResponseMessage(HttpStatusCode.NotFound)
        {
            Content = new StringContent(json, Encoding.UTF8, "application/json")
        };

        var client = new HttpClient(new StubHandler(resp));
        var measurementClient = new MeasurementClient(client);

        var ex = await Assert.ThrowsAsync<GlobalpingApiException>(() => measurementClient.GetMeasurementByIdAsync("1"));
        Assert.Equal("not_found", ex.Error.Type);
        Assert.Equal("Missing", ex.Error.Message);
        Assert.Equal((int)HttpStatusCode.NotFound, ex.StatusCode);
    }

    [Fact]
    public async Task GetMeasurementByIdAsync_ThrowsOnRateLimit()
    {
        const string json = "{\"error\":{\"type\":\"rate_limit_exceeded\",\"message\":\"Limit\"}}";
        var resp = new HttpResponseMessage((HttpStatusCode)429)
        {
            Content = new StringContent(json, Encoding.UTF8, "application/json")
        };

        var client = new HttpClient(new StubHandler(resp));
        var measurementClient = new MeasurementClient(client);

        var ex = await Assert.ThrowsAsync<GlobalpingApiException>(() => measurementClient.GetMeasurementByIdAsync("1"));
        Assert.Equal("rate_limit_exceeded", ex.Error.Type);
        Assert.Equal("Limit", ex.Error.Message);
        Assert.Equal(429, ex.StatusCode);
    }
}
