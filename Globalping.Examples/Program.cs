using System.Text.Json;
using Globalping;

var options = new JsonSerializerOptions { WriteIndented = true };

var builder = new MeasurementRequestBuilder()
    .WithType(MeasurementType.Ping)
    .WithTarget("cdn.jsdelivr.net")
    .AddCountry("DE")
    .AddCountry("PL");
var request1 = builder.Build();
Console.WriteLine(JsonSerializer.Serialize(request1, options));

builder = new MeasurementRequestBuilder()
    .WithType(MeasurementType.Ping)
    .WithTarget("cdn.jsdelivr.net")
    .AddCountry("DE", 4)
    .AddCountry("PL", 2);
var request2 = builder.Build();
Console.WriteLine(JsonSerializer.Serialize(request2, options));

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
var request3 = builder.Build();
Console.WriteLine(JsonSerializer.Serialize(request3, options));

builder = new MeasurementRequestBuilder()
    .WithType(MeasurementType.Ping)
    .WithTarget("cdn.jsdelivr.net")
    .WithMeasurementOptions(new PingOptions { Packets = 6 });
var request4 = builder.Build();
Console.WriteLine(JsonSerializer.Serialize(request4, options));

