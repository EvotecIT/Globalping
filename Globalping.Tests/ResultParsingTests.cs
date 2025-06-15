using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using Xunit;

namespace Globalping.Tests;

public class ResultParsingTests
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
    };

    static ResultParsingTests()
    {
        JsonOptions.Converters.Add(new JsonStringEnumConverter<MeasurementStatus>(JsonNamingPolicy.KebabCaseLower));
        JsonOptions.Converters.Add(new JsonStringEnumConverter<TestStatus>(JsonNamingPolicy.KebabCaseLower));
        JsonOptions.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
    }

    [Fact]
    public void ParsesTracerouteHopsFromJson()
    {
        var json = """
            {
                "id": "1",
                "type": "traceroute",
                "status": "finished",
                "target": "cdn.jsdelivr.net",
                "probesCount": 1,
                "results": [
                    {
                        "probe": {
                            "continent": "OC",
                            "region": "ANZ",
                            "country": "AU",
                            "state": null,
                            "city": "Sydney",
                            "asn": 1,
                            "longitude": 0,
                            "latitude": 0,
                            "network": "test",
                            "tags": [],
                            "resolvers": []
                        },
                        "result": {
                            "status": "finished",
                            "resolvedAddress": "104.16.85.20",
                            "resolvedHostname": "104.16.85.20",
                            "hops": [
                                { "resolvedHostname": "172.68.208.3", "resolvedAddress": "172.68.208.3", "timings": [ { "rtt": 1.0 }, { "rtt": 2.0 } ] },
                                { "resolvedHostname": "104.16.85.20", "resolvedAddress": "104.16.85.20", "timings": [ { "rtt": 0.5 } ] }
                            ]
                        }
                    }
                ]
            }
            """;

        var resp = JsonSerializer.Deserialize<MeasurementResponse>(json, JsonOptions);
        Assert.NotNull(resp);
        var hops = resp!.GetTracerouteHops();
        Assert.Equal(2, hops.Count);
        Assert.Equal(1, hops[0].Hop);
        Assert.Equal("172.68.208.3", hops[0].Host);
        Assert.Equal(1.0, hops[0].Time1);
    }

    [Fact]
    public void ParsesDnsAnswersFromJson()
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
                            "region": "ANZ",
                            "country": "AU",
                            "state": null,
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
                            "answers": [
                                { "name": "example.com", "type": "A", "ttl": 60, "class": "IN", "value": "1.1.1.1" }
                            ]
                        }
                    }
                ]
            }
            """;

        var resp = JsonSerializer.Deserialize<MeasurementResponse>(json, JsonOptions);
        Assert.NotNull(resp);
        var records = resp!.GetDnsRecords();
        Assert.Single(records);
        Assert.Equal("example.com", records[0].Name);
        Assert.Equal("A", records[0].Type);
    }

    [Fact]
    public void ParsesHttpResponseFromJson()
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
                        "probe": {
                            "continent": "EU",
                            "region": "EU",
                            "country": "DE",
                            "state": null,
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
                            "rawHeaders": "HTTP/1.1 200 OK\nContent-Type: text/plain\n",
                            "rawBody": "hello",
                            "truncated": false,
                            "headers": { "content-type": "text/plain" },
                            "statusCode": 200,
                            "statusCodeName": "OK",
                            "timings": { "dns": 10 },
                            "tls": { "keyType": "RSA" }
                        }
                    }
                ]
            }
            """;

        var resp = JsonSerializer.Deserialize<MeasurementResponse>(json, JsonOptions);
        Assert.NotNull(resp);
        var http = resp!.GetHttpResponses();
        Assert.Single(http);
        Assert.Equal(200, http[0].StatusCode);
        Assert.Equal("hello", http[0].Body);
        Assert.True(http[0].Headers.ContainsKey("content-type"));
        Assert.Single(http[0].Headers["content-type"]);
        Assert.Equal("text/plain", http[0].Headers["content-type"][0]);
        Assert.NotNull(http[0].Timings);
        Assert.Equal(10, http[0].Timings!.Dns);
        Assert.NotNull(http[0].Tls);
        Assert.Equal(TlsKeyType.RSA, http[0].Tls!.KeyType);
    }

    [Fact]
    public void ParsesMtrAsnListFromJson()
    {
        var json = """
            {
                "id": "1",
                "type": "mtr",
                "status": "finished",
                "target": "example.com",
                "probesCount": 1,
                "results": [
                    {
                        "probe": {
                            "continent": "EU",
                            "region": "EU",
                            "country": "DE",
                            "state": null,
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
                            "hops": [
                                { "resolvedHostname": "h1", "resolvedAddress": "1.1.1.1", "asn": [ 64500, 64501 ] },
                                { "resolvedHostname": "h2", "resolvedAddress": "2.2.2.2", "asn": [ 64502 ] }
                            ]
                        }
                    }
                ]
            }
            """;

        var resp = JsonSerializer.Deserialize<MeasurementResponse>(json, JsonOptions);
        Assert.NotNull(resp);
        var hops = resp!.GetMtrHops();
        Assert.Equal(2, hops.Count);
        Assert.Equal(new List<int> { 64500, 64501 }, Assert.IsType<List<int>>(hops[0].Asn));
        Assert.Equal(64502, Assert.IsType<int>(hops[1].Asn));
    }

    [Fact]
    public void ParsesMtrAsnNumberFromJson()
    {
        var json = """
            {
                "id": "1",
                "type": "mtr",
                "status": "finished",
                "target": "example.com",
                "probesCount": 1,
                "results": [
                    {
                        "probe": {
                            "continent": "EU",
                            "region": "EU",
                            "country": "DE",
                            "state": null,
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
                            "hops": [
                                { "resolvedHostname": "h1", "resolvedAddress": "1.1.1.1", "asn": 64500 }
                            ]
                        }
                    }
                ]
            }
            """;

        var resp = JsonSerializer.Deserialize<MeasurementResponse>(json, JsonOptions);
        Assert.NotNull(resp);
        var hops = resp!.GetMtrHops();
        Assert.Single(hops);
        Assert.Equal(64500, Assert.IsType<int>(hops[0].Asn));
    }

    [Fact]
    public void SummaryIncludesProbeVersion()
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
                            "region": "EU",
                            "country": "DE",
                            "state": null,
                            "city": "Berlin",
                            "asn": 1,
                            "longitude": 0,
                            "latitude": 0,
                            "network": "test",
                            "tags": [],
                            "resolvers": [],
                            "version": "2.0.0"
                        },
                        "result": {
                            "status": "finished"
                        }
                    }
                ]
            }
            """;

        var resp = JsonSerializer.Deserialize<MeasurementResponse>(json, JsonOptions);
        Assert.NotNull(resp);
        var summaries = resp!.GetSummaries();
        Assert.Single(summaries);
        Assert.Equal("2.0.0", summaries[0].Version);
    }
}
