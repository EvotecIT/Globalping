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
}
