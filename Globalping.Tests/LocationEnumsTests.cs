using System.Text.Json;
using Xunit;

namespace Globalping.Tests;

public sealed class LocationEnumsTests
{
    [Fact]
    public void ContinentCodeSerializes()
    {
        var json = JsonSerializer.Serialize(ContinentCode.EU);
        Assert.Equal("\"EU\"", json);
    }

    [Fact]
    public void RegionNameSerializes()
    {
        var json = JsonSerializer.Serialize(RegionName.AustraliaAndNewZealand);
        Assert.Equal("\"Australia and New Zealand\"", json);
    }
}
