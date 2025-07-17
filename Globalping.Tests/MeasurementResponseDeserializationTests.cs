using System.Text.Json;
using System.Text.Json.Serialization;
using Xunit;

namespace Globalping.Tests;

public sealed class MeasurementResponseDeserializationTests
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
    };

    static MeasurementResponseDeserializationTests()
    {
        JsonOptions.Converters.Add(new JsonStringEnumConverter<MeasurementStatus>(JsonNamingPolicy.KebabCaseLower));
        JsonOptions.Converters.Add(new JsonStringEnumConverter<TestStatus>(JsonNamingPolicy.KebabCaseLower));
        JsonOptions.Converters.Add(new CountryCodeConverter());
        JsonOptions.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
    }
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

        var response = JsonSerializer.Deserialize<MeasurementResponse>(json, JsonOptions);
        Assert.NotNull(response);
        Assert.NotNull(response!.Locations);
        Assert.Single(response.Locations!);
        Assert.Equal(CountryCode.Germany, response.Locations![0].Country);
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

        var response = JsonSerializer.Deserialize<MeasurementResponse>(json, JsonOptions);
        Assert.NotNull(response);
        Assert.NotNull(response!.Results);
        Assert.Single(response.Results!);
        Assert.NotNull(response.Results![0].Data.Tls);
        Assert.Equal(TlsKeyType.RSA, response.Results![0].Data.Tls!.KeyType);
    }

    [Fact]
    public void ParsesInProgressStatus()
    {
        var json = """
        {
            "id": "1",
            "type": "ping",
            "status": "in-progress",
            "target": "example.com",
            "probesCount": 1
        }
        """;

        var response = JsonSerializer.Deserialize<MeasurementResponse>(json, JsonOptions);
        Assert.NotNull(response);
        Assert.Equal(MeasurementStatus.InProgress, response!.Status);
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

        var response = JsonSerializer.Deserialize<MeasurementResponse>(json, JsonOptions);
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

        var response = JsonSerializer.Deserialize<MeasurementResponse>(json, JsonOptions);
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

        var response = JsonSerializer.Deserialize<MeasurementResponse>(json, JsonOptions);
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

        var response = JsonSerializer.Deserialize<MeasurementResponse>(json, JsonOptions);
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

        var response = JsonSerializer.Deserialize<MeasurementResponse>(json, JsonOptions);
        var opts = Assert.IsType<HttpOptions>(response!.MeasurementOptions);
        Assert.Equal(8080, opts.Port);
        Assert.Equal(HttpProtocol.HTTP, opts.Protocol);
    }

    [Fact]
    public void DeserializesProbeVersion()
    {
        var json = """
        {
            "id": "1",
            "type": "ping",
            "status": "finished",
            "target": "example.com",
            "probesCount": 1,
            "results": [
                {
                    "probe": {
                        "continent": "EU",
                        "country": "DE",
                        "city": "Berlin",
                        "asn": 1,
                        "longitude": 0,
                        "latitude": 0,
                        "network": "test",
                        "tags": [],
                        "resolvers": [],
                        "version": "1.2.3"
                    },
                    "result": {
                        "status": "finished",
                        "timings": [ { "ttl": 64, "rtt": 1.0 } ]
                    }
                }
            ]
        }
        """;

        var response = JsonSerializer.Deserialize<MeasurementResponse>(json, JsonOptions);
        Assert.NotNull(response);
        Assert.NotNull(response!.Results);
        Assert.Single(response.Results!);
        Assert.Equal("1.2.3", response.Results![0].Probe.Version);

        var timings = response.GetPingTimings();
        Assert.Single(timings);
        Assert.Equal("1.2.3", timings[0].Version);
    }

    [Fact]
    public void DeserializesDnsResolverAndTimings()
    {
        var json = """
        {
            "id": "1",
            "type": "dns",
            "status": "finished",
            "target": "example.com",
            "probesCount": 1,
            "results": [
                {
                    "probe": {
                        "continent": "OC",
                        "country": "AU",
                        "city": "Melbourne",
                        "asn": 1,
                        "longitude": 0,
                        "latitude": 0,
                        "network": "test",
                        "tags": [],
                        "resolvers": []
                    },
                    "result": {
                        "status": "finished",
                        "resolver": "8.8.8.8",
                        "statusCode": 0,
                        "statusCodeName": "NOERROR",
                        "timings": { "total": 42.0 },
                        "answers": [
                            { "name": "example.com", "type": "A", "ttl": 60, "class": "IN", "value": "1.1.1.1" }
                        ]
                    }
                }
            ]
        }
        """;

        var response = JsonSerializer.Deserialize<MeasurementResponse>(json, JsonOptions);
        Assert.NotNull(response);
        var records = response!.GetDnsRecords();
        Assert.Single(records);
        Assert.Equal("8.8.8.8", records[0].Resolver);
        Assert.Equal(0, records[0].StatusCode);
        Assert.Equal("NOERROR", records[0].StatusCodeName);
        Assert.NotNull(records[0].Timings);
        Assert.Equal(42.0, records[0].Timings!.Total);
    }

    [Fact]
    public void DeserializesStatsWithNullableValues()
    {
        var json = """
        {
            "id": "1",
            "type": "ping",
            "status": "finished",
            "target": "example.com",
            "probesCount": 1,
            "results": [
                {
                    "probe": {
                        "continent": "EU",
                        "country": "DE",
                        "city": "Berlin",
                        "asn": 1,
                        "longitude": 0,
                        "latitude": 0,
                        "network": "test",
                        "tags": [],
                        "resolvers": []
                    },
                    "result": {
                        "status": "finished",
                        "stats": { "min": null, "max": 2.0, "avg": 1.5, "loss": null }
                    }
                }
            ]
        }
        """;

        var response = JsonSerializer.Deserialize<MeasurementResponse>(json, JsonOptions);
        Assert.NotNull(response);
        var stats = response!.Results![0].Data.Stats;
        Assert.NotNull(stats);
        Assert.Null(stats!.Min);
        Assert.Equal(2.0, stats.Max);
        Assert.Equal(1.5, stats.Avg);
        Assert.Null(stats.Loss);
    }
}
