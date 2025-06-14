using System;
using System.Text.Json;
using Globalping;

namespace Globalping.Examples;

public static class BuildRequestExample
{
    public static void Run()
    {
        var options = new JsonSerializerOptions { WriteIndented = true };

        var builder = new MeasurementRequestBuilder()
            .WithType(MeasurementType.Ping)
            .WithTarget("cdn.jsdelivr.net")
            .AddCountry("DE")
            .AddCountry("PL");
        Console.WriteLine(JsonSerializer.Serialize(builder.Build(), options));

        builder = new MeasurementRequestBuilder()
            .WithType(MeasurementType.Ping)
            .WithTarget("cdn.jsdelivr.net")
            .AddCountry("DE", 4)
            .AddCountry("PL", 2);
        Console.WriteLine(JsonSerializer.Serialize(builder.Build(), options));

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
        Console.WriteLine(JsonSerializer.Serialize(builder.Build(), options));

        builder = new MeasurementRequestBuilder()
            .WithType(MeasurementType.Ping)
            .WithTarget("cdn.jsdelivr.net")
            .WithMeasurementOptions(new PingOptions { Packets = 6 });
        Console.WriteLine(JsonSerializer.Serialize(builder.Build(), options));
    }
}
