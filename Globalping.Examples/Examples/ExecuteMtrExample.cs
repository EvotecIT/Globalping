using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Globalping.Examples;

public static class ExecuteMtrExample
{
    public static async Task RunAsync()
    {
        var builder = new MeasurementRequestBuilder()
            .WithType(MeasurementType.Mtr)
            .WithTarget("cdn.jsdelivr.net")
            .AddMagic("Europe")
            .WithOptions(new MtrOptions { Packets = 3 });

        var request = builder.Build();
        request.InProgressUpdates = false;

        using var httpClient = new HttpClient(new HttpClientHandler
        {
            AutomaticDecompression = DecompressionMethods.All
        });
        var apiKey = Environment.GetEnvironmentVariable("GLOBALPING_TOKEN");
        var probeService = new ProbeService(httpClient, apiKey);
        var measurement = await probeService.CreateMeasurementAsync(request);

        ConsoleHelpers.WriteHeading($"MTR example (ID: {measurement.Id})");

        var client = new MeasurementClient(httpClient, apiKey);
        var result = await client.GetMeasurementByIdAsync(measurement.Id);

        ConsoleHelpers.WriteJson(request, $"Request sent (MTR ID: {measurement.Id})");

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
