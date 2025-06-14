using System;
using System.Net.Http;
using System.Threading.Tasks;
using Spectre.Console;

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

        ConsoleHelpers.WriteJson(request, "Request sent");
        ConsoleHelpers.WriteJson(result, "Measurement result");

        if (result.Results != null)
        {
            foreach (var item in result.Results)
            {
                ConsoleHelpers.WriteTable(item.Probe, "Probe");
                ConsoleHelpers.WriteJson(item.Data, "Result details");
            }
        }
    }
}
