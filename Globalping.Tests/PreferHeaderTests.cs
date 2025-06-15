using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Globalping.Tests;

public sealed class PreferHeaderTests
{
    private sealed class CaptureHandler : HttpMessageHandler
    {
        public HttpRequestMessage? LastRequest { get; private set; }

        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            LastRequest = request;
            var response = new HttpResponseMessage(HttpStatusCode.Accepted)
            {
                Content = new StringContent("{\"id\":\"1\"}", Encoding.UTF8, "application/json")
            };
            return Task.FromResult(response);
        }
    }

    [Fact]
    public async Task CreateMeasurementAsync_SetsPreferHeader()
    {
        var handler = new CaptureHandler();
        var client = new HttpClient(handler);
        var service = new ProbeService(client);

        var request = new MeasurementRequestBuilder()
            .WithType(MeasurementType.Ping)
            .WithTarget("example.com")
            .Build();
        request.InProgressUpdates = true;

        await service.CreateMeasurementAsync(request, 42);

        Assert.NotNull(handler.LastRequest);
        Assert.True(handler.LastRequest!.Headers.TryGetValues("Prefer", out var values));
        Assert.Contains("respond-async, wait=42", values);
    }

    [Fact]
    public async Task CreateMeasurementAsync_ClearsPreferHeader()
    {
        var handler = new CaptureHandler();
        var client = new HttpClient(handler);
        var service = new ProbeService(client);

        var request1 = new MeasurementRequestBuilder()
            .WithType(MeasurementType.Ping)
            .WithTarget("example.com")
            .Build();
        request1.InProgressUpdates = true;

        await service.CreateMeasurementAsync(request1, 10);
        Assert.NotNull(handler.LastRequest);
        Assert.True(handler.LastRequest!.Headers.Contains("Prefer"));

        var request2 = new MeasurementRequestBuilder()
            .WithType(MeasurementType.Ping)
            .WithTarget("example.com")
            .Build();

        await service.CreateMeasurementAsync(request2, 10);

        Assert.NotNull(handler.LastRequest);
        Assert.False(handler.LastRequest!.Headers.Contains("Prefer"));
    }
}
