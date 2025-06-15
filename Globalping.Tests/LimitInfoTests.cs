using System.Collections.Generic;
using System.Text.Json;
using Xunit;

namespace Globalping.Tests;

public class LimitInfoTests
{
    [Fact]
    public void FlattenProducesLimitInfo()
    {
        var json = """
        {
            "rateLimit": {
                "measurements": {
                    "create": { "type": "ip", "limit": 10, "remaining": 8, "reset": 1000 }
                }
            },
            "credits": { "remaining": 42 }
        }
        """;

        var limits = JsonSerializer.Deserialize<Limits>(json);
        Assert.NotNull(limits);
        var infos = new List<LimitInfo>(limits!.Flatten());
        Assert.Single(infos);
        var info = infos[0];
        Assert.Equal("measurements", info.Category);
        Assert.Equal("create", info.Action);
        Assert.Equal("ip", info.Type);
        Assert.Equal(10, info.Limit);
        Assert.Equal(8, info.Remaining);
        Assert.Equal(1000, info.Reset);
        Assert.Equal(42, info.CreditsRemaining);
    }
}
