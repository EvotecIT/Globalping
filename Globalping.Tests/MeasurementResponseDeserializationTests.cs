using System.Text.Json;
using Xunit;

namespace Globalping.Tests;

public sealed class MeasurementResponseDeserializationTests
{
    [Fact]
    public void DeserializesLocationsAndLimit()
    {
        var json = """
        {
            "id": "123",
            "type": "ping",
            "status": "finished",
            "createdAt": "2024-01-01T00:00:00Z",
            "updatedAt": "2024-01-01T00:01:00Z",
            "target": "example.com",
            "probesCount": 1,
            "locations": [ { "country": "DE", "limit": 1 } ],
            "limit": 1,
            "results": []
        }
        """;

        var response = JsonSerializer.Deserialize<MeasurementResponse>(json);
        Assert.NotNull(response);
        Assert.NotNull(response!.Locations);
        Assert.Single(response.Locations!);
        Assert.Equal("DE", response.Locations![0].Country);
        Assert.Equal(1, response.Locations![0].Limit);
        Assert.Equal(1, response.Limit);
    }

    [Fact]
    public void DeserializesTlsKeyType()
    {
        var json = """
        {
            "id": "1",
            "type": "http",
            "status": "finished",
            "target": "https://example.com",
            "probesCount": 1,
            "results": [
                {
                    "result": {
                        "tls": { "keyType": "RSA" }
                    }
                }
            ]
        }
        """;

        var response = JsonSerializer.Deserialize<MeasurementResponse>(json);
        Assert.NotNull(response);
        Assert.NotNull(response!.Results);
        Assert.Single(response.Results!);
        Assert.NotNull(response.Results![0].Data.Tls);
        Assert.Equal(TlsKeyType.RSA, response.Results![0].Data.Tls!.KeyType);
    }

    [Fact]
    public void MapsPingOptions()
    {
        var json = """
        {
            "id": "1",
            "type": "ping",
            "status": "finished",
            "target": "example.com",
            "probesCount": 1,
            "measurementOptions": { "packets": 5, "ipVersion": 6 }
        }
        """;

        var response = JsonSerializer.Deserialize<MeasurementResponse>(json);
        var opts = Assert.IsType<PingOptions>(response!.MeasurementOptions);
        Assert.Equal(5, opts.Packets);
        Assert.Equal(IpVersion.Six, opts.IpVersion);
    }

    [Fact]
    public void MapsTracerouteOptions()
    {
        var json = """
        {
            "id": "1",
            "type": "traceroute",
            "status": "finished",
            "target": "example.com",
            "probesCount": 1,
            "measurementOptions": { "port": 443, "protocol": "tcp", "ipVersion": 6 }
        }
        """;

        var response = JsonSerializer.Deserialize<MeasurementResponse>(json);
        var opts = Assert.IsType<TracerouteOptions>(response!.MeasurementOptions);
        Assert.Equal(443, opts.Port);
        Assert.Equal(TracerouteProtocol.TCP, opts.Protocol);
        Assert.Equal(IpVersion.Six, opts.IpVersion);
    }

    [Fact]
    public void MapsDnsOptions()
    {
        var json = """
        {
            "id": "1",
            "type": "dns",
            "status": "finished",
            "target": "example.com",
            "probesCount": 1,
            "measurementOptions": { "protocol": "udp", "trace": true }
        }
        """;

        var response = JsonSerializer.Deserialize<MeasurementResponse>(json);
        var opts = Assert.IsType<DnsOptions>(response!.MeasurementOptions);
        Assert.Equal(DnsProtocol.UDP, opts.Protocol);
        Assert.True(opts.Trace);
    }

    [Fact]
    public void MapsMtrOptions()
    {
        var json = """
        {
            "id": "1",
            "type": "mtr",
            "status": "finished",
            "target": "example.com",
            "probesCount": 1,
            "measurementOptions": { "packets": 4, "protocol": "udp" }
        }
        """;

        var response = JsonSerializer.Deserialize<MeasurementResponse>(json);
        var opts = Assert.IsType<MtrOptions>(response!.MeasurementOptions);
        Assert.Equal(4, opts.Packets);
        Assert.Equal(MtrProtocol.UDP, opts.Protocol);
    }

    [Fact]
    public void MapsHttpOptions()
    {
        var json = """
        {
            "id": "1",
            "type": "http",
            "status": "finished",
            "target": "https://example.com",
            "probesCount": 1,
            "measurementOptions": { "port": 8080, "protocol": "http" }
        }
        """;

        var response = JsonSerializer.Deserialize<MeasurementResponse>(json);
        var opts = Assert.IsType<HttpOptions>(response!.MeasurementOptions);
        Assert.Equal(8080, opts.Port);
        Assert.Equal(HttpProtocol.HTTP, opts.Protocol);
    }
}
