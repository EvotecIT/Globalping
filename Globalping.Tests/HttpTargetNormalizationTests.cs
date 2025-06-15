using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Globalping.Tests;

public sealed class HttpTargetNormalizationTests
{
    private sealed class CaptureHandler : HttpMessageHandler
    {
        public string? Payload { get; private set; }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            Payload = await request.Content!.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
            var response = new HttpResponseMessage(HttpStatusCode.Accepted)
            {
                Content = new StringContent("{\"id\":\"1\",\"probesCount\":1}", Encoding.UTF8, "application/json")
            };
            return response;
        }
    }

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
    };

    static HttpTargetNormalizationTests()
    {
        JsonOptions.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
    }

    [Fact]
    public async Task CreateMeasurementAsync_NormalizesUrlTarget()
    {
        var handler = new CaptureHandler();
        var client = new HttpClient(handler);
        var service = new ProbeService(client);

        var request = new MeasurementRequestBuilder()
            .WithType(MeasurementType.Http)
            .WithTarget("https://example.com/test")
            .Build();

        await service.CreateMeasurementAsync(request);

        Assert.NotNull(handler.Payload);
        var json = handler.Payload!;
        using var doc = JsonDocument.Parse(json);
        var root = doc.RootElement;
        Assert.Equal("example.com", root.GetProperty("target").GetString());
        Assert.True(root.TryGetProperty("measurementOptions", out var opts));
        Assert.Equal(JsonValueKind.Object, opts.ValueKind);
    }
}
