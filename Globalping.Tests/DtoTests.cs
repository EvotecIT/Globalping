using System.Text.Json;
using System.Text.Json.Serialization;
using Xunit;

namespace Globalping.Tests;

public class DtoTests
{
    [Fact]
    public void DnsQuery_SerializesWithDefaultType()
    {
        var query = new DnsQuery();
        Assert.Equal(DnsQueryType.A, query.Type);
        var json = JsonSerializer.Serialize(query);
        Assert.Contains("\"type\":\"A\"", json);
    }

    [Fact]
    public void TlsCertificateIssuer_RoundTrips()
    {
        var issuer = new TlsCertificateIssuer { C = "US", O = "Org", CN = "cn" };
        var json = JsonSerializer.Serialize(issuer);
        var clone = JsonSerializer.Deserialize<TlsCertificateIssuer>(json);
        Assert.Equal("US", clone!.C);
        Assert.Equal("Org", clone.O);
        Assert.Equal("cn", clone.CN);
    }

    [Fact]
    public void TlsCertificateSubject_RoundTrips()
    {
        var subj = new TlsCertificateSubject { CN = "cn", Alt = "alt" };
        var json = JsonSerializer.Serialize(subj);
        var clone = JsonSerializer.Deserialize<TlsCertificateSubject>(json);
        Assert.Equal("cn", clone!.CN);
        Assert.Equal("alt", clone.Alt);
    }

    [Fact]
    public void HttpTimings_RoundTrips()
    {
        var timings = new HttpTimings
        {
            Total = 1,
            Dns = 2,
            Tcp = 3,
            Tls = 4,
            FirstByte = 5,
            Download = 6
        };
        var json = JsonSerializer.Serialize(timings);
        var clone = JsonSerializer.Deserialize<HttpTimings>(json);
        Assert.Equal(1, clone!.Total);
        Assert.Equal(2, clone.Dns);
        Assert.Equal(3, clone.Tcp);
        Assert.Equal(4, clone.Tls);
        Assert.Equal(5, clone.FirstByte);
        Assert.Equal(6, clone.Download);
    }

    [Fact]
    public void Stats_RoundTrips()
    {
        var stats = new Stats
        {
            Min = 1,
            Max = 2,
            Avg = 1.5,
            Total = 10,
            Loss = 0.1,
            Rcv = 9,
            Drop = 1,
            StDev = 0.5,
            JMin = 0.2,
            JAvg = 0.3,
            JMax = 0.4
        };
        var json = JsonSerializer.Serialize(stats);
        var clone = JsonSerializer.Deserialize<Stats>(json);
        Assert.Equal(1, clone!.Min);
        Assert.Equal(2, clone.Max);
        Assert.Equal(1.5, clone.Avg);
        Assert.Equal(10, clone.Total);
        Assert.Equal(0.1, clone.Loss);
        Assert.Equal(9, clone.Rcv);
        Assert.Equal(1, clone.Drop);
        Assert.Equal(0.5, clone.StDev);
        Assert.Equal(0.2, clone.JMin);
        Assert.Equal(0.3, clone.JAvg);
        Assert.Equal(0.4, clone.JMax);
    }
}
