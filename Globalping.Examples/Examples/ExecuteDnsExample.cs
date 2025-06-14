using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Globalping.Examples;

public static class ExecuteDnsExample
{
    public static async Task RunAsync()
    {
        var builder = new MeasurementRequestBuilder()
            .WithType(MeasurementType.Dns)
            .WithTarget("cloudflare.com")
            .AddMagic("US")
            .WithMeasurementOptions(new DnsOptions
            {
                Query = new DnsQuery { Type = DnsQueryType.A },
                Resolver = "8.8.8.8",
                Trace = false
            });

        var request = builder.Build();
        request.InProgressUpdates = false;

        using var httpClient = new HttpClient(new HttpClientHandler
        {
            AutomaticDecompression = DecompressionMethods.All
        });
        var apiKey = Environment.GetEnvironmentVariable("GLOBALPING_TOKEN");
        var probeService = new ProbeService(httpClient, apiKey);
        var measurementId = await probeService.CreateMeasurementAsync(request);

        ConsoleHelpers.WriteHeading($"DNS example (ID: {measurementId})");

        var client = new MeasurementClient(httpClient, apiKey);
        var result = await client.GetMeasurementByIdAsync(measurementId);

        ConsoleHelpers.WriteJson(request, $"Request sent (DNS ID: {measurementId})");

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
