using Globalping.PowerShell;
using Globalping;
using Xunit;

namespace Globalping.Tests;

public class AutomaticLimitCalculationTests
{
    [Fact]
    public void CalculatesLimitFromLocations()
    {
        var locations = new[]
        {
            new LocationRequest { Country = CountryCode.Germany },
            new LocationRequest { City = "Berlin" }
        };

        var result = StartGlobalpingBaseCommand.ComputeLimit(null, false, null, null, locations);
        Assert.Equal(2, result);
    }

    [Fact]
    public void DefaultsToOneWhenNoLocations()
    {
        var result = StartGlobalpingBaseCommand.ComputeLimit(null, false, null, null, null);
        Assert.Equal(1, result);
    }

    [Fact]
    public void CalculatesLimitFromSimpleLocations()
    {
        var simple = new[] { "DE", "US", "GB" };

        var result = StartGlobalpingBaseCommand.ComputeLimit(null, false, null, simple, null);
        Assert.Equal(3, result);
    }
}

