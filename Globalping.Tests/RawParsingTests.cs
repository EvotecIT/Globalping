using System.Collections.Generic;
using Xunit;

namespace Globalping.Tests;

public class RawParsingTests
{
    [Fact]
    public void ParsesTracerouteRawOutput()
    {
        var raw = "traceroute to example.com (93.184.216.34), 64 hops max\n" +
                  "1  router (192.168.0.1)  1.0 ms  2.0 ms  3.0 ms\n" +
                  "2  example.com (93.184.216.34)  5.0 ms  6.0 ms  7.0 ms\n";
        var resp = new MeasurementResponse
        {
            Target = "example.com",
            Results = new List<Result>
            {
                new()
                {
                    Probe = new Probe(),
                    Data = new ResultDetails { RawOutput = raw }
                }
            }
        };

        var hops = resp.GetTracerouteHops();
        Assert.Equal(2, hops.Count);
        Assert.Equal(1, hops[0].Hop);
        Assert.Equal("router", hops[0].Host);
        Assert.Equal("93.184.216.34", hops[1].IpAddress);
    }

    [Fact]
    public void ParsesDnsRawOutput()
    {
        var raw = ";; ->>HEADER<<- opcode: QUERY, status: NOERROR, id: 1\n" +
                  ";; flags: qr rd ra; QUERY: 1, ANSWER: 1, AUTHORITY: 0, ADDITIONAL: 0\n\n" +
                  ";; QUESTION SECTION:\n" +
                  ";example.com. IN A\n\n" +
                  ";; ANSWER SECTION:\n" +
                  "example.com. 60 IN A 93.184.216.34\n";
        var resp = new MeasurementResponse
        {
            Target = "example.com",
            Results = new List<Result>
            {
                new()
                {
                    Probe = new Probe(),
                    Data = new ResultDetails { RawOutput = raw }
                }
            }
        };

        var records = resp.GetDnsRecords();
        Assert.Single(records);
        Assert.Equal("example.com", records[0].Name);
        Assert.Equal("A", records[0].Type);
        Assert.Equal("93.184.216.34", records[0].Data);
    }

    [Fact]
    public void ParsesHttpRawOutput()
    {
        var raw = "HTTP/1.1 200 OK\n" +
                  "Content-Type: text/plain\n" +
                  "X-Test: 1\n" +
                  "\n" +
                  "hello";
        var resp = new MeasurementResponse
        {
            Target = "example.com",
            Results = new List<Result>
            {
                new()
                {
                    Probe = new Probe(),
                    Data = new ResultDetails { RawOutput = raw }
                }
            }
        };

        var http = resp.GetHttpResponses();
        Assert.Single(http);
        Assert.Equal(200, http[0].StatusCode);
        Assert.Equal("hello", http[0].Body);
        Assert.True(http[0].Headers.ContainsKey("Content-Type"));
    }

    [Fact]
    public void ParsesMtrRawOutput()
    {
        var raw = "header\n" +
                  "Host\n" +
                  "1. AS100 router (192.168.0.1) 0.0% 1 1 1.0 1.0 1.0\n" +
                  "2. AS101 example.com (93.184.216.34) 0.0% 1 1 2.0 2.0 2.0\n" +
                  "3  waiting for reply\n";
        var resp = new MeasurementResponse
        {
            Target = "example.com",
            Results = new List<Result>
            {
                new()
                {
                    Probe = new Probe(),
                    Data = new ResultDetails { RawOutput = raw }
                }
            }
        };

        var hops = resp.GetMtrHops();
        Assert.Equal(3, hops.Count);
        Assert.Equal("router", hops[0].Host);
        Assert.Equal("192.168.0.1", hops[0].IpAddress);
        Assert.Equal("waiting for reply", hops[2].Host);
    }
}
