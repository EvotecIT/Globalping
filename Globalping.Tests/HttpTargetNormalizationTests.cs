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
            Payload = await request.Content!.ReadAsStringAsync(
#if NETFRAMEWORK
                ).ConfigureAwait(false);
#else
                cancellationToken).ConfigureAwait(false);
#endif
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

    [Fact]
    public async Task CreateMeasurementAsync_ParsesPathQueryAndPort()
    {
        var handler = new CaptureHandler();
        var client = new HttpClient(handler);
        var service = new ProbeService(client);

        var request = new MeasurementRequestBuilder()
            .WithType(MeasurementType.Http)
            .WithTarget("http://example.com:8080/api/data?x=1")
            .Build();

        await service.CreateMeasurementAsync(request);

        Assert.NotNull(handler.Payload);
        var opts = Assert.IsType<HttpOptions>(request.MeasurementOptions);
        Assert.Equal(HttpProtocol.HTTP, opts.Protocol);
        Assert.Equal(8080, opts.Port);
        Assert.Equal("/api/data", opts.Request.Path);
        Assert.Equal("x=1", opts.Request.Query);
    }

    [Fact]
    public async Task CreateMeasurementAsync_DoesNotChangeNonHttpRequest()
    {
        var handler = new CaptureHandler();
        var client = new HttpClient(handler);
        var service = new ProbeService(client);

        var request = new MeasurementRequestBuilder()
            .WithType(MeasurementType.Ping)
            .WithTarget("https://example.com/test")
            .Build();

        await service.CreateMeasurementAsync(request);

        using var doc = JsonDocument.Parse(handler.Payload!);
        var root = doc.RootElement;
        Assert.Equal("https://example.com/test", root.GetProperty("target").GetString());
        Assert.False(root.TryGetProperty("measurementOptions", out _));
    }

    [Fact]
    public async Task CreateMeasurementAsync_AdjustsProtocolAndPortOnHttpUrl()
    {
        var handler = new CaptureHandler();
        var client = new HttpClient(handler);
        var service = new ProbeService(client);

        var request = new MeasurementRequestBuilder()
            .WithType(MeasurementType.Http)
            .WithTarget("http://example.com")
            .Build();

        await service.CreateMeasurementAsync(request);

        var opts = Assert.IsType<HttpOptions>(request.MeasurementOptions);
        Assert.Equal(HttpProtocol.HTTP, opts.Protocol);
        Assert.Equal(80, opts.Port);
    }

    [Fact]
    public async Task CreateMeasurementAsync_SetsDefaultHttpsPort()
    {
        var handler = new CaptureHandler();
        var client = new HttpClient(handler);
        var service = new ProbeService(client);

        var request = new MeasurementRequestBuilder()
            .WithType(MeasurementType.Http)
            .WithTarget("https://example.com")
            .Build();

        await service.CreateMeasurementAsync(request);

        var opts = Assert.IsType<HttpOptions>(request.MeasurementOptions);
        Assert.Equal(HttpProtocol.HTTPS, opts.Protocol);
        Assert.Equal(443, opts.Port);
    }
}
