using System.Text.Json;
using Xunit;

namespace Globalping.Tests;

public class ProbeInfoTests
{
    [Fact]
    public void ToProbeInfo_HandlesTagAndResolverConversion()
    {
        const string json = "{\"version\":\"1.0\",\"location\":{\"continent\":\"SA\",\"region\":\"South America\",\"country\":\"BR\",\"state\":\"SP\",\"city\":\"Sao Paulo\",\"asn\":31898,\"longitude\":-46.64,\"latitude\":-23.55,\"network\":\"Oracle Corporation\"},\"tags\":[\"datacenter-network\"],\"resolvers\":[\"1.1.1.1\"]}";
        var probe = JsonSerializer.Deserialize<Probes>(json);
        Assert.NotNull(probe);
        var info = probe!.ToProbeInfo();

        Assert.Equal("SA", info.Continent);
        Assert.Equal("South America", info.Region);
        Assert.Equal("BR", info.Country);
        Assert.Equal("SP", info.State);
        Assert.Equal("Sao Paulo", info.City);
        Assert.Equal(31898, info.Asn);
        Assert.Equal(-46.64, info.Longitude);
        Assert.Equal(-23.55, info.Latitude);
        Assert.Equal("Oracle Corporation", info.Network);

        var tag = Assert.IsType<string>(info.Tags);
        Assert.Equal("datacenter-network", tag);

        var resolver = Assert.IsType<string>(info.Resolvers);
        Assert.Equal("1.1.1.1", resolver);

        Assert.Equal("1.0", info.Version);
    }

    [Fact]
    public void ToProbeInfo_HandlesMultipleTags()
    {
        const string json = "{\"version\":\"1\",\"location\":{},\"tags\":[\"a\",\"b\"],\"resolvers\":[\"8.8.8.8\",\"8.8.4.4\"]}";
        var probe = JsonSerializer.Deserialize<Probes>(json);
        Assert.NotNull(probe);
        var info = probe!.ToProbeInfo();

        var tags = Assert.IsType<string[]>(info.Tags);
        Assert.Equal(new[] { "a", "b" }, tags);

        var resolvers = Assert.IsType<string[]>(info.Resolvers);
        Assert.Equal(new[] { "8.8.8.8", "8.8.4.4" }, resolvers);
    }

    [Fact]
    public void ToProbeInfo_HandlesEmptyLists()
    {
        const string json = "{\"version\":\"1\",\"location\":{},\"tags\":[],\"resolvers\":[]}";
        var probe = JsonSerializer.Deserialize<Probes>(json);
        Assert.NotNull(probe);
        var info = probe!.ToProbeInfo();

        Assert.Null(info.Tags);
        Assert.Null(info.Resolvers);
    }
}
