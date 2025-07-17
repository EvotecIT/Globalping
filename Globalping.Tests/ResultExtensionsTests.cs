using System;
using System.Collections.Generic;
using System.Text.Json;
using Xunit;

namespace Globalping.Tests;

public class ResultExtensionsTests
{
    [Fact]
    public void ToPingTimings_ReturnsParsedTimings()
    {
        var timingsJson = JsonSerializer.SerializeToElement(new[]
        {
            new Timing { Ttl = 64, Rtt = 1.2 },
            new Timing { Ttl = 64, Rtt = 1.5 }
        });

        var result = new Result
        {
            Probe = new Probe { Country = "DE", City = "Berlin", Network = "net", Asn = 1, State = "BE", Continent = "EU", Version = "1" },
            Data = new ResultDetails { Timings = timingsJson, ResolvedAddress = "1.1.1.1", ResolvedHostname = "host" }
        };

        var list = result.ToPingTimings("example.com");
        Assert.Equal(2, list.Count);
        Assert.Equal("example.com", list[0].Target);
        Assert.Equal(1, list[0].IcmpSequence);
        Assert.Equal(64, list[0].TTL);
        Assert.Equal(1.2, list[0].Time);
        Assert.Equal("DE", list[0].Country);
        Assert.Equal("host", list[0].ResolvedHostname);
    }

    [Fact]
    public void ToHttpResponse_ParsesRawOutput()
    {
        var details = new ResultDetails
        {
            RawOutput = "HTTP/1.1 200 OK\nContent-Type: text/plain\n\nhello"
        };
        var result = new Result
        {
            Probe = new Probe(),
            Data = details
        };

        var http = result.ToHttpResponse("example.com");
        Assert.NotNull(http);
        Assert.Equal(HttpProtocolVersion.Http11, http!.Protocol);
        Assert.Equal(200, http.StatusCode);
        Assert.Equal("hello", http.Body);
        Assert.True(http.Headers.ContainsKey("Content-Type"));
    }
}
