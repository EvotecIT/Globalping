using System;
using Xunit;

namespace Globalping.Tests;

public class ExtensionParsingTests
{
    [Fact]
    public void RegionNameTryParse_ValidString_ReturnsTrue()
    {
        var success = RegionNameExtensions.TryParse("Northern Europe", out var region);
        Assert.True(success);
        Assert.Equal(RegionName.NorthernEurope, region);
    }

    [Fact]
    public void RegionNameTryParse_InvalidString_ReturnsFalse()
    {
        var success = RegionNameExtensions.TryParse("Unknown Region", out var region);
        Assert.False(success);
        Assert.Equal(default, region);
    }

    [Fact]
    public void RegionNameToValueString_ReturnsExpected()
    {
        var str = RegionName.NorthernEurope.ToValueString();
        Assert.Equal("Northern Europe", str);
    }

    [Fact]
    public void ContinentCodeTryParse_ParsesCaseInsensitive()
    {
        var success = ContinentCodeExtensions.TryParse("eu", out var code);
        Assert.True(success);
        Assert.Equal(ContinentCode.EU, code);
    }
}
