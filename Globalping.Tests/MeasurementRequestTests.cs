using System.Text.Json;
using System.Text.Json.Serialization;
using Xunit;

namespace Globalping.Tests;

public sealed class MeasurementRequestTests
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
    };

    static MeasurementRequestTests()
    {
        JsonOptions.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
    }

    [Fact]
    public void SerializesMeasurementIdForLocations()
    {
        var request = new MeasurementRequestBuilder()
            .WithType(MeasurementType.Ping)
            .WithTarget("example.com")
            .ReuseLocationsFromId("previous-id")
            .Build();

        var json = JsonSerializer.Serialize(request, JsonOptions);
        using var doc = JsonDocument.Parse(json);

        var root = doc.RootElement;
        Assert.True(root.TryGetProperty("locations", out var loc));
        Assert.Equal(JsonValueKind.String, loc.ValueKind);
        Assert.Equal("previous-id", loc.GetString());
        Assert.False(root.TryGetProperty("reuseLocationsFromId", out _));
        Assert.False(root.TryGetProperty("locations", out var _1) && _1.ValueKind == JsonValueKind.Array);
    }

    [Fact]
    public void SerializesLocationArray()
    {
        var request = new MeasurementRequestBuilder()
            .WithType(MeasurementType.Ping)
            .WithTarget("example.com")
            .AddCountry("DE")
            .AddCountry("US")
            .Build();

        var json = JsonSerializer.Serialize(request, JsonOptions);
        using var doc = JsonDocument.Parse(json);

        var root = doc.RootElement;
        Assert.True(root.TryGetProperty("locations", out var loc));
        Assert.Equal(JsonValueKind.Array, loc.ValueKind);
        Assert.Equal(2, loc.GetArrayLength());
        Assert.False(root.TryGetProperty("reuseLocationsFromId", out _));
    }

    [Fact]
    public void LocationsOverrideReuseId()
    {
        var request = new MeasurementRequestBuilder()
            .WithType(MeasurementType.Ping)
            .WithTarget("example.com")
            .ReuseLocationsFromId("old")
            .AddCountry("DE")
            .Build();

        Assert.Null(request.ReuseLocationsFromId);
        Assert.NotNull(request.Locations);
    }

    [Fact]
    public void ReuseIdClearsLocations()
    {
        var request = new MeasurementRequestBuilder()
            .WithType(MeasurementType.Ping)
            .WithTarget("example.com")
            .AddCountry("DE")
            .ReuseLocationsFromId("new")
            .Build();

        Assert.Equal("new", request.ReuseLocationsFromId);
        Assert.Null(request.Locations);
    }

    [Fact]
    public void SerializesInProgressUpdates()
    {
        var request = new MeasurementRequestBuilder()
            .WithType(MeasurementType.Ping)
            .WithTarget("example.com")
            .WithInProgressUpdates()
            .Build();

        var json = JsonSerializer.Serialize(request, JsonOptions);
        using var doc = JsonDocument.Parse(json);

        var root = doc.RootElement;
        Assert.True(root.TryGetProperty("inProgressUpdates", out var prop));
        Assert.True(prop.GetBoolean());
    }

    [Fact]
    public void AddMagicWithLimitSetsLimit()
    {
        var request = new MeasurementRequestBuilder()
            .WithType(MeasurementType.Ping)
            .WithTarget("example.com")
            .AddMagic("Europe", 5)
            .Build();

        var loc = Assert.Single(request.Locations!);
        Assert.Equal("Europe", loc.Magic);
        Assert.Equal(5, loc.Limit);
    }

    [Fact]
    public void AddMagicWithoutLimitLeavesLimitNull()
    {
        var request = new MeasurementRequestBuilder()
            .WithType(MeasurementType.Ping)
            .WithTarget("example.com")
            .AddMagic("Europe")
            .Build();

        var loc = Assert.Single(request.Locations!);
        Assert.Equal("Europe", loc.Magic);
        Assert.Null(loc.Limit);
    }

    [Fact]
    public void BuilderAddsVariousLocationTypes()
    {
        var request = new MeasurementRequestBuilder()
            .WithType(MeasurementType.Ping)
            .WithTarget("example.com")
            .AddCity("Berlin", 1)
            .AddState("BY")
            .AddAsn(64500)
            .AddNetwork("1.1.1.0/24")
            .AddTags(new[] { "edge", "test" })
            .Build();

        Assert.Equal(5, request.Locations!.Count);
        Assert.Equal("Berlin", request.Locations[0].City);
        Assert.Equal(1, request.Locations[0].Limit);
        Assert.Equal("BY", request.Locations[1].State);
        Assert.Equal(64500, request.Locations[2].Asn);
        Assert.Equal("1.1.1.0/24", request.Locations[3].Network);
        Assert.Equal(new[] { "edge", "test" }, request.Locations[4].Tags);
    }

    [Fact]
    public void BuilderSetsOptionsAndLimit()
    {
        var request = new MeasurementRequestBuilder()
            .WithType(MeasurementType.Ping)
            .WithTarget("example.com")
            .WithMeasurementOptions(new PingOptions { Packets = 4 })
            .WithLimit(3)
            .Build();

        Assert.Equal(3, request.Limit);
        var options = Assert.IsType<PingOptions>(request.MeasurementOptions);
        Assert.Equal(4, options.Packets);
    }

    [Fact]
    public void BuilderSerializesTypeInCamelCase()
    {
        var request = new MeasurementRequestBuilder()
            .WithType(MeasurementType.Ping)
            .WithTarget("example.com")
            .Build();

        var json = JsonSerializer.Serialize(request, JsonOptions);

        Assert.Contains("\"type\":\"ping\"", json);
    }
}
