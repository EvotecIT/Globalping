using System.Text.Json;
using Xunit;

namespace Globalping.Tests;

public sealed class LimitsDeserializationTests
{
    [Fact]
    public void DeserializesRateLimits()
    {
        var json = """
        {
            "rateLimit": {
                "measurement": {
                    "ping": { "type": "ip", "limit": 10, "remaining": 8, "reset": 1000 }
                }
            }
        }
        """;

        var limits = JsonSerializer.Deserialize<Limits>(json);
        Assert.NotNull(limits);
        Assert.True(limits!.RateLimit.ContainsKey("measurement"));
        var measurement = limits.RateLimit["measurement"];
        Assert.True(measurement.ContainsKey("ping"));
        var ping = measurement["ping"];
        Assert.Equal("ip", ping.Type);
        Assert.Equal(10, ping.Limit);
        Assert.Equal(8, ping.Remaining);
        Assert.Equal(1000, ping.Reset);
    }

    [Fact]
    public void DeserializesCredits()
    {
        var json = """
        {
            "rateLimit": {},
            "credits": { "remaining": 42 }
        }
        """;

        var limits = JsonSerializer.Deserialize<Limits>(json);
        Assert.NotNull(limits);
        Assert.NotNull(limits!.Credits);
        Assert.Equal(42, limits.Credits!.Remaining);
    }
}
