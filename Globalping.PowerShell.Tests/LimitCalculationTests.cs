using System.Linq;
using Globalping;
using Globalping.PowerShell;
using Xunit;

namespace Globalping.PowerShell.Tests;

public class LimitCalculationTests
{
    private sealed class LimitTestCommand : StartGlobalpingBaseCommand
    {
        public bool LimitBound { get; set; }

        protected override MeasurementType Type => MeasurementType.Ping;

        public int? CalculateLimit()
        {
            int? limit = Limit;
            var calculateLimit = !LimitBound;
            var hasLocationLimits = ReuseLocationsFromId is null &&
                Locations is not null && Locations.Any(l => l.Limit.HasValue);

            if (ReuseLocationsFromId is null && calculateLimit && !hasLocationLimits)
            {
                limit = 0;

                if (SimpleLocations is not null)
                {
                    limit += SimpleLocations.Length;
                }

                if (Locations is not null)
                {
                    foreach (var loc in Locations)
                    {
                        limit += loc.Limit ?? 1;
                    }
                }

                if (limit == 0)
                {
                    limit = 1;
                }
            }

            return limit;
        }

        protected override void ProcessRecord() => throw new System.NotImplementedException();
    }

    [Fact]
    public void DefaultsToOneWhenNoLocations()
    {
        var cmd = new LimitTestCommand
        {
            Target = new[] { "example.com" },
            LimitBound = false
        };

        Assert.Equal(1, cmd.CalculateLimit());
    }

    [Fact]
    public void ComputesBasedOnLocationsWhenLimitNotSpecified()
    {
        var cmd = new LimitTestCommand
        {
            Target = new[] { "example.com" },
            SimpleLocations = new[] { "DE", "US" },
            Locations = new[] { new LocationRequest { Country = CountryCode.UnitedKingdom } },
            LimitBound = false
        };

        Assert.Equal(3, cmd.CalculateLimit());
    }

    [Fact]
    public void UsesExplicitLimitWhenSpecified()
    {
        var cmd = new LimitTestCommand
        {
            Target = new[] { "example.com" },
            Limit = 5,
            LimitBound = true
        };

        Assert.Equal(5, cmd.CalculateLimit());
    }

    [Fact]
    public void SkipsCalculationWhenLocationLimitsPresent()
    {
        var cmd = new LimitTestCommand
        {
            Target = new[] { "example.com" },
            Locations = new[]
            {
                new LocationRequest { Country = CountryCode.Germany, Limit = 2 },
                new LocationRequest { Country = CountryCode.UnitedStates }
            },
            LimitBound = false
        };

        Assert.Null(cmd.CalculateLimit());
    }
}
