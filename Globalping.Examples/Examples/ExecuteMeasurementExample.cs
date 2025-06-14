using System;
using System.Net;
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

        using var httpClient = new HttpClient(new HttpClientHandler
        {
            AutomaticDecompression = DecompressionMethods.All
        });
        var apiKey = Environment.GetEnvironmentVariable("GLOBALPING_TOKEN");
        var probeService = new ProbeService(httpClient, apiKey);
        var measurementId = await probeService.CreateMeasurementAsync(request);

        ConsoleHelpers.WriteHeading($"Ping example (ID: {measurementId})");

        var client = new MeasurementClient(httpClient, apiKey);
        var result = await client.GetMeasurementByIdAsync(measurementId);

        ConsoleHelpers.WriteJson(request, $"Request sent (Ping ID: {measurementId})");

        if (result != null)
        {
            ConsoleHelpers.WriteJson(result, "Measurement result");

            if (result.Results != null)
            {
                foreach (var item in result.Results)
                {
                    ConsoleHelpers.WriteTable(item.Probe, "Probe");
                    ConsoleHelpers.WriteTable(item.Data, "Result details");
                }
            }
        }
    }
}
