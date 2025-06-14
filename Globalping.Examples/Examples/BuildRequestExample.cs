using System;
using Globalping;
using Spectre.Console;

namespace Globalping.Examples;

public static class BuildRequestExample
{
    public static void Run()
    {
        var builder = new MeasurementRequestBuilder()
            .WithType(MeasurementType.Ping)
            .WithTarget("cdn.jsdelivr.net")
            .AddCountry("DE")
            .AddCountry("PL");
        ConsoleHelpers.WriteTable(builder.Build(), "Request 1");

        builder = new MeasurementRequestBuilder()
            .WithType(MeasurementType.Ping)
            .WithTarget("cdn.jsdelivr.net")
            .AddCountry("DE", 4)
            .AddCountry("PL", 2);
        ConsoleHelpers.WriteTable(builder.Build(), "Request 2");

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
        ConsoleHelpers.WriteTable(builder.Build(), "Request 3");

        builder = new MeasurementRequestBuilder()
            .WithType(MeasurementType.Ping)
            .WithTarget("cdn.jsdelivr.net")
            .WithMeasurementOptions(new PingOptions { Packets = 6 });
        ConsoleHelpers.WriteTable(builder.Build(), "Request 4");
    }
}
