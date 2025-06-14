using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Globalping.Examples;

public static class CheckLimitsExample
{
    public static async Task RunAsync()
    {
        using var httpClient = new HttpClient();
        var apiKey = Environment.GetEnvironmentVariable("GLOBALPING_TOKEN");
        var service = new ProbeService(httpClient, apiKey);
        var limits = await service.GetLimitsAsync();

        ConsoleHelpers.WriteHeading("API limits");
        ConsoleHelpers.WriteJson(limits);
    }
}
