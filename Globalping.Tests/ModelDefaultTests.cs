using System;
using System.Text.Json;
using Xunit;

namespace Globalping.Tests;

public class ModelDefaultTests
{
    [Fact]
    public void HttpRequestOptions_DefaultsAreSet()
    {
        var opts = new HttpRequestOptions();
        Assert.Equal(HttpRequestMethod.HEAD, opts.Method);
        opts.Headers["content-type"] = "text/plain";
        Assert.True(opts.Headers.ContainsKey("Content-Type"));
    }

    [Fact]
    public void HttpOptions_DefaultsAreSet()
    {
        var opts = new HttpOptions();
        Assert.NotNull(opts.Request);
        Assert.Equal(80, opts.Port);
        Assert.Equal(HttpProtocol.HTTPS, opts.Protocol);
        Assert.Equal(IpVersion.Four, opts.IpVersion);
    }

    [Fact]
    public void DnsOptions_DefaultsAreSet()
    {
        var opts = new DnsOptions();
        Assert.NotNull(opts.Query);
        Assert.Equal(53, opts.Port);
        Assert.Equal(DnsProtocol.UDP, opts.Protocol);
        Assert.Equal(IpVersion.Four, opts.IpVersion);
        Assert.False(opts.Trace);
    }

    [Fact]
    public void TlsCertificate_RoundTripsJson()
    {
        var cert = new TlsCertificate
        {
            Protocol = "TLSv1.3",
            CipherName = "AES",
            Authorized = true,
            CreatedAt = new DateTime(2024,1,1,0,0,0,DateTimeKind.Utc),
            ExpiresAt = new DateTime(2025,1,1,0,0,0,DateTimeKind.Utc),
            SerialNumber = "abc",
            Fingerprint256 = "123"
        };
        var json = JsonSerializer.Serialize(cert);
        var clone = JsonSerializer.Deserialize<TlsCertificate>(json);
        Assert.Equal(cert.Protocol, clone!.Protocol);
        Assert.Equal(cert.SerialNumber, clone.SerialNumber);
        Assert.Equal(cert.Fingerprint256, clone.Fingerprint256);
    }
}
