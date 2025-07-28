using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using Xunit;

namespace Globalping.Tests;

public sealed class MeasurementResponseSerializationTests
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
    };

    static MeasurementResponseSerializationTests()
    {
        JsonOptions.Converters.Add(new JsonStringEnumConverter<MeasurementStatus>(JsonNamingPolicy.KebabCaseLower));
        JsonOptions.Converters.Add(new JsonStringEnumConverter<TestStatus>(JsonNamingPolicy.KebabCaseLower));
        JsonOptions.Converters.Add(new CountryCodeConverter());
        JsonOptions.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
    }

    [Fact]
    public void MeasurementResponse_RoundTripSerialization()
    {
        var response = new MeasurementResponse
        {
            Id = "1",
            Type = "ping",
            Status = MeasurementStatus.Finished,
            CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            UpdatedAt = new DateTime(2024, 1, 1, 0, 1, 0, DateTimeKind.Utc),
            Target = "example.com",
            ProbesCount = 1,
            MeasurementOptions = new PingOptions { Packets = 4, IpVersion = IpVersion.Six },
            Locations = new List<LocationRequest>
            {
                new() { Country = CountryCode.Germany, Limit = 2 }
            },
            Limit = 5,
            Results = new List<Result>
            {
                new()
                {
                    Probe = new Probe
                    {
                        Continent = "EU",
                        Region = "EU",
                        Country = "DE",
                        City = "Berlin",
                        Asn = 64500,
                        Longitude = 13.4,
                        Latitude = 52.5,
                        Network = "net",
                        Version = "1.0"
                    },
                    Data = new ResultDetails
                    {
                        Status = TestStatus.Finished,
                        Timings = JsonSerializer.SerializeToElement(new[] { new Timing { Ttl = 64, Rtt = 1.23 } })
                    }
                }
            }
        };

        var json = JsonSerializer.Serialize(response, JsonOptions);
        using var doc = JsonDocument.Parse(json);
        var root = doc.RootElement;

        Assert.True(root.TryGetProperty("id", out _));
        Assert.True(root.TryGetProperty("type", out var type));
        Assert.Equal("ping", type.GetString());
        Assert.True(root.TryGetProperty("measurementOptions", out var opts));
        Assert.True(opts.TryGetProperty("packets", out var packets));
        Assert.Equal(4, packets.GetInt32());
        Assert.False(root.TryGetProperty("CreatedAt", out _));

        var clone = JsonSerializer.Deserialize<MeasurementResponse>(json, JsonOptions);
        Assert.NotNull(clone);
        Assert.Equal(response.Id, clone!.Id);
        Assert.Equal(response.Type, clone.Type);
        Assert.Equal(response.Status, clone.Status);
        Assert.Equal(response.CreatedAt, clone.CreatedAt);
        Assert.Equal(response.UpdatedAt, clone.UpdatedAt);
        Assert.Equal(response.Target, clone.Target);
        Assert.Equal(response.ProbesCount, clone.ProbesCount);
        Assert.NotNull(clone.MeasurementOptions);
        var cloneOpts = Assert.IsType<PingOptions>(clone.MeasurementOptions);
        Assert.Equal(4, cloneOpts.Packets);
        Assert.Equal(IpVersion.Six, cloneOpts.IpVersion);
        Assert.NotNull(clone.Locations);
        Assert.Single(clone.Locations!);
        Assert.Equal(CountryCode.Germany, clone.Locations![0].Country);
        Assert.Equal(2, clone.Locations![0].Limit);
        Assert.Equal(5, clone.Limit);
        Assert.NotNull(clone.Results);
        Assert.Single(clone.Results!);
        Assert.Equal(TestStatus.Finished, clone.Results![0].Data.Status);
    }
}
