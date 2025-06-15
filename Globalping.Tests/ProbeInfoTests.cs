using System.Text.Json;
using Xunit;

namespace Globalping.Tests;

public class ProbeInfoTests
{
    [Fact]
    public void ToProbeInfo_ConvertsValues()
    {
        const string json = "{\"version\":\"1.0\",\"location\":{\"continent\":\"SA\",\"region\":\"South America\",\"country\":\"BR\",\"state\":\"SP\",\"city\":\"Sao Paulo\",\"asn\":31898,\"longitude\":-46.64,\"latitude\":-23.55,\"network\":\"Oracle Corporation\"},\"tags\":[\"datacenter-network\"]}";
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
        Assert.Equal("datacenter-network", info.Tags);
        Assert.Equal("1.0", info.Version);
    }
}
