using System;
using System.Text.Json;
using Xunit;

namespace Globalping.Tests;

public sealed class EnumCoverageTests
{
    [Fact]
    public void CountryCodeConverter_InvalidString_Throws()
    {
        var options = new JsonSerializerOptions();
        options.Converters.Add(new CountryCodeConverter());
        Assert.Throws<JsonException>(() => JsonSerializer.Deserialize<CountryCode>("\"XX\"", options));
    }

    [Fact]
    public void CountryCodeConverter_Null_Throws()
    {
        var options = new JsonSerializerOptions();
        options.Converters.Add(new CountryCodeConverter());
        Assert.Throws<JsonException>(() => JsonSerializer.Deserialize<CountryCode>("null", options));
    }

    [Fact]
    public void CountryCodeExtensions_TryParse_WorksCaseInsensitive()
    {
        var success = CountryCodeExtensions.TryParse("de", out var code);
        Assert.True(success);
        Assert.Equal(CountryCode.Germany, code);
    }

    [Fact]
    public void CountryCodeExtensions_TryParse_InvalidReturnsFalse()
    {
        var success = CountryCodeExtensions.TryParse("nope", out var code);
        Assert.False(success);
        Assert.Equal(default, code);
    }

    [Fact]
    public void CountryCodeExtensions_TryParse_EnumName()
    {
        var success = CountryCodeExtensions.TryParse(nameof(CountryCode.Germany), out var code);
        Assert.True(success);
        Assert.Equal(CountryCode.Germany, code);
    }

    [Fact]
    public void CountryCodeExtensions_ToValueString_ReturnsExpected()
    {
        var value = CountryCode.UnitedStates.ToValueString();
        Assert.Equal("US", value);
    }

    [Fact]
    public void RegionNameConverter_InvalidString_Throws()
    {
        Assert.Throws<JsonException>(() => JsonSerializer.Deserialize<RegionName>("\"Nowhere\""));
    }

    [Fact]
    public void RegionNameConverter_InvalidValue_ThrowsOnWrite()
    {
        RegionName invalid = (RegionName)999;
        Assert.Throws<JsonException>(() => JsonSerializer.Serialize(invalid));
    }

    [Fact]
    public void RegionNameConverter_Null_Throws()
    {
        Assert.Throws<JsonException>(() => JsonSerializer.Deserialize<RegionName>("null"));
    }

    [Fact]
    public void RegionNameConverter_Read_ValidString()
    {
        var region = JsonSerializer.Deserialize<RegionName>("\"Northern Europe\"");
        Assert.Equal(RegionName.NorthernEurope, region);
    }

    [Fact]
    public void RegionNameExtensions_TryParse_InvalidReturnsFalse()
    {
        var success = RegionNameExtensions.TryParse("Unknown", out var region);
        Assert.False(success);
        Assert.Equal(default, region);
    }

    [Fact]
    public void RegionNameExtensions_ToValueString_InvalidValueThrows()
    {
        var invalid = (RegionName)1234;
        Assert.Throws<ArgumentOutOfRangeException>(() => invalid.ToValueString());
    }

    [Fact]
    public void IpVersionConverter_ReadsIPv4()
    {
        var options = new JsonSerializerOptions();
        options.Converters.Add(new IpVersionConverter());
        var json = JsonSerializer.Serialize(IpVersion.Four, options);
        Assert.Equal("4", json);
        var value = JsonSerializer.Deserialize<IpVersion>("4", options);
        Assert.Equal(IpVersion.Four, value);
    }
}
