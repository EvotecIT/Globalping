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
}
