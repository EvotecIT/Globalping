using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Globalping.Examples;

public static class ExecuteMeasurementExample
{
    public static async Task RunAsync()
    {
        var builder = new MeasurementRequestBuilder()
            .WithType(MeasurementType.Ping)
            .WithTarget("cdn.jsdelivr.net")
            .AddMagic("Europe")
            .WithMeasurementOptions(new PingOptions { Packets = 2 });

        var request = builder.Build();

        using var httpClient = new HttpClient();
        var probeService = new ProbeService(httpClient);
        var measurementId = await probeService.CreateMeasurementAsync(request);

        var client = new MeasurementClient(httpClient);
        var result = await client.GetMeasurementByIdAsync(measurementId);

        var options = new JsonSerializerOptions { WriteIndented = true };
        Console.WriteLine(JsonSerializer.Serialize(result, options));
    }
}
