using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Globalping.Tests;

public class MeasurementClientPollingTests
{
    private sealed class SequenceHandler : HttpMessageHandler
    {
        public int CallCount { get; private set; }
        private readonly Queue<HttpResponseMessage> _responses;

        public SequenceHandler(params HttpResponseMessage[] responses)
        {
            _responses = new Queue<HttpResponseMessage>(responses);
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            CallCount++;
            return Task.FromResult(_responses.Dequeue());
        }
    }

    [Fact]
    public async Task GetMeasurementByIdAsync_RetriesUntilFinished()
    {
        const string firstJson = "{\"id\":\"1\",\"type\":\"ping\",\"status\":\"in-progress\",\"target\":\"example.com\",\"probesCount\":0}";
        const string secondJson = "{\"id\":\"1\",\"type\":\"ping\",\"status\":\"finished\",\"target\":\"example.com\",\"probesCount\":0}";
        var first = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(firstJson, Encoding.UTF8, "application/json")
        };
        var second = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(secondJson, Encoding.UTF8, "application/json")
        };

        var handler = new SequenceHandler(first, second);
        var client = new HttpClient(handler);
        var measurementClient = new MeasurementClient(client);

        var result = await measurementClient.GetMeasurementByIdAsync("1");

        Assert.NotNull(result);
        Assert.Equal(MeasurementStatus.Finished, result!.Status);
        Assert.Equal(2, handler.CallCount);
    }
}
