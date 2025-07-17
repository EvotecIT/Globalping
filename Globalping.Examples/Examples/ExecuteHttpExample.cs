using System;
using Globalping;
using System.Threading;
using System.Threading.Tasks;

namespace Globalping.Examples;

public static class ExecuteHttpExample {
    public static async Task RunAsync() {
        var builder = new MeasurementRequestBuilder()
            .WithType(MeasurementType.Http)
            .WithTarget("cdn.jsdelivr.net")
            .AddMagic("Europe")
            .WithMeasurementOptions(new HttpOptions {
                Request = new HttpRequestOptions {
                    Method = HttpRequestMethod.GET,
                    Path = "/"
                }
            });

        var request = builder.Build();
        request.InProgressUpdates = true;

        using var httpClient = new HttpClient(new HttpClientHandler {
            AutomaticDecompression = System.Net.DecompressionMethods.All
        });
        var apiKey = Environment.GetEnvironmentVariable("GLOBALPING_TOKEN");
        var probeService = new ProbeService(httpClient, apiKey);
        using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(30));
        var measurement = await probeService.CreateMeasurementAsync(request, 60, cts.Token);

        ConsoleHelpers.WriteHeading($"HTTP example (ID: {measurement.Id})");

        var client = new MeasurementClient(httpClient, apiKey);
        var result = await client.GetMeasurementByIdAsync(measurement.Id, cancellationToken: cts.Token);

        ConsoleHelpers.WriteJson(request, $"Request sent (HTTP ID: {measurement.Id})");

        if (result != null) {
            ConsoleHelpers.WriteJson(result, "Measurement result");

            if (result.Results != null) {
                foreach (var item in result.Results) {
                    ConsoleHelpers.WriteTable(item.Probe, "Probe");
                    ConsoleHelpers.WriteTable(item.Data, "Result details");
                    var http = item.ToHttpResponse();
                    if (http != null)
                    {
                        ConsoleHelpers.WriteHeading($"Protocol version: {http.Protocol}");
                    }
                }
            }
        }
    }
}
