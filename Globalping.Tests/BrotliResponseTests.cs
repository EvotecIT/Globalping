using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
#if !NETFRAMEWORK

namespace Globalping.Tests;

public class BrotliResponseTests
{
    private static byte[] Compress(string json)
    {
        using var ms = new MemoryStream();
        using (var brotli = new BrotliStream(ms, CompressionLevel.Optimal, true))
        using (var writer = new StreamWriter(brotli, Encoding.UTF8))
        {
            writer.Write(json);
        }
        return ms.ToArray();
    }

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

    private sealed class DecompressionHandler : DelegatingHandler
    {
        public DecompressionHandler(HttpMessageHandler inner) : base(inner) { }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var response = await base.SendAsync(request, cancellationToken);
            if (response.Content.Headers.ContentEncoding.Contains("br"))
            {
                var input = await response.Content.ReadAsStreamAsync(
#if NETFRAMEWORK
                    );
#else
                    cancellationToken);
#endif
                var output = new MemoryStream();
#if NETFRAMEWORK
                using (var brotli = new System.IO.Compression.BrotliStream(input, CompressionMode.Decompress))
#else
                using (var brotli = new BrotliStream(input, CompressionMode.Decompress))
#endif
                {
#if NETFRAMEWORK
                    await brotli.CopyToAsync(output);
#else
                    await brotli.CopyToAsync(output, cancellationToken);
#endif
                }
                output.Position = 0;
                response.Content = new StreamContent(output);
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                response.Content.Headers.ContentEncoding.Clear();
            }
            return response;
        }
    }

    [Fact]
    public async Task GetMeasurementByIdAsync_ThrowsWithoutDecompression()
    {
        const string json = "{\"id\":\"1\",\"type\":\"ping\",\"status\":\"finished\",\"target\":\"example.com\",\"probesCount\":0}";
        var compressed = Compress(json);
        var response = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new ByteArrayContent(compressed)
        };
        response.Content.Headers.ContentEncoding.Add("br");
        response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

        var client = new HttpClient(new MockHandler(response));
        var measurementClient = new MeasurementClient(client);
        await Assert.ThrowsAsync<System.Text.Json.JsonException>(() => measurementClient.GetMeasurementByIdAsync("1"));
    }

    [Fact]
    public async Task GetMeasurementByIdAsync_ParsesBrotliResponse()
    {
        const string json = "{\"id\":\"1\",\"type\":\"ping\",\"status\":\"finished\",\"target\":\"example.com\",\"probesCount\":0}";
        var compressed = Compress(json);
        var response = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new ByteArrayContent(compressed)
        };
        response.Content.Headers.ContentEncoding.Add("br");
        response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

        var handler = new DecompressionHandler(new MockHandler(response));
        var client = new HttpClient(handler);
        var measurementClient = new MeasurementClient(client);
        var result = await measurementClient.GetMeasurementByIdAsync("1");
        Assert.NotNull(result);
        Assert.Equal("example.com", result!.Target);
    }
}
#endif
