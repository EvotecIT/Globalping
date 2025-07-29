using System;
using Globalping;

namespace Globalping.Examples;

public static class BuildRequestExample
{
    public static void Run()
    {
        ConsoleHelpers.WriteHeading("Build request examples");
        var builder = new MeasurementRequestBuilder()
            .WithType(MeasurementType.Ping)
            .WithTarget("cdn.jsdelivr.net")
            .AddCountry(CountryCode.Germany)
            .AddCountry(CountryCode.Poland);
        ConsoleHelpers.WriteJson(builder.Build(), "Request 1");

        builder = new MeasurementRequestBuilder()
            .WithType(MeasurementType.Ping)
            .WithTarget("cdn.jsdelivr.net")
            .AddCountry(CountryCode.Germany, 4)
            .AddCountry(CountryCode.Poland, 2);
        ConsoleHelpers.WriteJson(builder.Build(), "Request 2");

        builder = new MeasurementRequestBuilder()
            .WithType(MeasurementType.Ping)
            .WithTarget("cdn.jsdelivr.net")
            .AddMagic("FR")
            .AddMagic("Poland")
            .AddMagic("Berlin+Germany")
            .AddMagic("California")
            .AddMagic("Europe")
            .AddMagic("Western Europe")
            .AddMagic("AS13335")
            .AddMagic("aws-us-east-1")
            .AddMagic("Google");
        ConsoleHelpers.WriteJson(builder.Build(), "Request 3");

        builder = new MeasurementRequestBuilder()
            .WithType(MeasurementType.Ping)
            .WithTarget("cdn.jsdelivr.net")
            .WithOptions(new PingOptions { Packets = 6 });
        ConsoleHelpers.WriteJson(builder.Build(), "Request 4");

        builder = new MeasurementRequestBuilder()
            .WithType(MeasurementType.Mtr)
            .WithTarget("cdn.jsdelivr.net")
            .AddMagic("Europe")
            .WithOptions(new MtrOptions { Packets = 3 });
        ConsoleHelpers.WriteJson(builder.Build(), "Request 5");

        builder = new MeasurementRequestBuilder()
            .WithType(MeasurementType.Dns)
            .WithTarget("cloudflare.com")
            .AddMagic("US")
            .WithOptions(new DnsOptions {
                Query = new DnsQuery { Type = DnsQueryType.A },
                Resolver = "8.8.8.8"
            });
        ConsoleHelpers.WriteJson(builder.Build(), "Request 6");

        builder = new MeasurementRequestBuilder()
            .WithType(MeasurementType.Ping)
            .WithTarget("example.com")
            .ReuseLocationsFromId("previous-id");
        ConsoleHelpers.WriteJson(builder.Build(), "Request 7");
    }
}
