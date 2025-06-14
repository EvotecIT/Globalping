using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Globalping.Examples;

public static class ExecuteTracerouteExample
{
    public static async Task RunAsync()
    {
        var builder = new MeasurementRequestBuilder()
            .WithType(MeasurementType.Traceroute)
            .WithTarget("cdn.jsdelivr.net")
            .AddMagic("Europe")
            .WithMeasurementOptions(new TracerouteOptions());

        var request = builder.Build();
        request.InProgressUpdates = false;

        using var httpClient = new HttpClient(new HttpClientHandler
        {
            AutomaticDecompression = DecompressionMethods.All
        });
        var apiKey = Environment.GetEnvironmentVariable("GLOBALPING_TOKEN");
        var probeService = new ProbeService(httpClient, apiKey);
        var measurementId = await probeService.CreateMeasurementAsync(request);

        ConsoleHelpers.WriteHeading($"Traceroute example (ID: {measurementId})");

        var client = new MeasurementClient(httpClient, apiKey);
        var result = await client.GetMeasurementByIdAsync(measurementId);

        ConsoleHelpers.WriteJson(request, $"Request sent (Traceroute ID: {measurementId})");

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
