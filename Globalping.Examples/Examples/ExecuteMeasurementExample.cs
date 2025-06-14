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
        request.InProgressUpdates = false;

        using var httpClient = new HttpClient();
        var apiKey = Environment.GetEnvironmentVariable("GLOBALPING_TOKEN");
        var probeService = new ProbeService(httpClient, apiKey);
        var measurementId = await probeService.CreateMeasurementAsync(request);

        var client = new MeasurementClient(httpClient, apiKey);
        var result = await client.GetMeasurementByIdAsync(measurementId);

        var options = new JsonSerializerOptions { WriteIndented = true };
        Console.WriteLine(JsonSerializer.Serialize(result, options));
    }
}
