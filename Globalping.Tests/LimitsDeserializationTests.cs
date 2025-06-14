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
                "measurements": {
                    "create": { "type": "ip", "limit": 10, "remaining": 8, "reset": 1000 }
                }
            }
        }
        """;

        var limits = JsonSerializer.Deserialize<Limits>(json);
        Assert.NotNull(limits);
        Assert.True(limits!.RateLimit.ContainsKey("measurements"));
        var measurements = limits.RateLimit["measurements"];
        Assert.True(measurements.ContainsKey("create"));
        var create = measurements["create"];
        Assert.Equal("ip", create.Type);
        Assert.Equal(10, create.Limit);
        Assert.Equal(8, create.Remaining);
        Assert.Equal(1000, create.Reset);
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
