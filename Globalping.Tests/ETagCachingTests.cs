using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Globalping.Tests;

public class ETagCachingTests
{
    private sealed class SequenceHandler : HttpMessageHandler
    {
        private readonly Queue<HttpResponseMessage> _responses;
        public SequenceHandler(params HttpResponseMessage[] responses)
        {
            _responses = new Queue<HttpResponseMessage>(responses);
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return Task.FromResult(_responses.Dequeue());
        }
    }

    [Fact]
    public async Task ReturnsCachedMeasurementOnNotModified()
    {
        const string json = "{\"id\":\"1\",\"type\":\"ping\",\"status\":\"finished\",\"target\":\"example.com\",\"probesCount\":0}";
        var first = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(json, Encoding.UTF8, "application/json")
        };
        first.Headers.ETag = new EntityTagHeaderValue("\"abc\"");

        var second = new HttpResponseMessage(HttpStatusCode.NotModified);
        second.Headers.ETag = new EntityTagHeaderValue("\"abc\"");

        var client = new HttpClient(new SequenceHandler(first, second));
        var measurementClient = new MeasurementClient(client);

        var result1 = await measurementClient.GetMeasurementByIdAsync("1");
        Assert.NotNull(result1);

        var result2 = await measurementClient.GetMeasurementByIdAsync("1", measurementClient.LastResponseInfo.ETag);
        Assert.Same(result1, result2);
    }
}
