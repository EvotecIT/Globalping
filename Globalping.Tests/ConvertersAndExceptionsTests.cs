using System;
using System.Text.Json;
using Xunit;

namespace Globalping.Tests;

public class ConvertersAndExceptionsTests
{
    [Fact]
    public void CountryCodeConverter_ReadsAndWrites()
    {
        var options = new JsonSerializerOptions();
        options.Converters.Add(new CountryCodeConverter());
        var json = JsonSerializer.Serialize(CountryCode.Germany, options);
        Assert.Equal("\"DE\"", json);
        var value = JsonSerializer.Deserialize<CountryCode>("\"US\"", options);
        Assert.Equal(CountryCode.UnitedStates, value);
    }

    [Fact]
    public void IpVersionConverter_ReadsAndWrites()
    {
        var options = new JsonSerializerOptions();
        options.Converters.Add(new IpVersionConverter());
        var json = JsonSerializer.Serialize(IpVersion.Six, options);
        Assert.Equal("6", json);
        var value = JsonSerializer.Deserialize<IpVersion>("6", options);
        Assert.Equal(IpVersion.Six, value);
    }

    [Fact]
    public void GlobalpingApiException_PopulatesProperties()
    {
        var err = new ErrorDetails { Message = "oops", Type = "bad" };
        var usage = new ApiUsageInfo { CreditsRemaining = 1, RateLimitRemaining = 2 };
        var ex = new GlobalpingApiException(400, err, usage);
        Assert.Equal(400, ex.StatusCode);
        Assert.Equal("oops", ex.Message);
        Assert.Equal("bad", ex.Error.Type);
        Assert.Equal(1, ex.UsageInfo.CreditsRemaining);
        Assert.Equal(2, ex.UsageInfo.RateLimitRemaining);
    }
}
