Ever wondered how to run network diagnostics like Ping, Traceroute, or DNS queries from probes scattered across the globe? Enter **Globalping.NET**, a powerful library that makes it effortless to interact with the Globalping API using C#. Whether you're debugging latency issues or exploring packet routes, this library has you covered.

In this blog, we'll dive into the features of Globalping.NET, explore real-world examples, and show you how to get started with this amazing tool. If you're a PowerShell enthusiast, check out our companion blog on [Globalping PowerShell Module](https://evotec.xyz/automating-network-diagnostics-with-globalping-powershell-module/) to see how these tools complement each other.


## Free API with Generous Limits

Globalping is a free API that requires no registration to get started. Here are the limits:

- **Unregistered Users**:
  - 50 probes per measurement
  - 250 free tests per hour
- **Registered Users**:
  - 500 probes per measurement
  - 500 free tests per hour

Higher limits are available for members, making it ideal for both casual and professional use.


## Available Methods

Globalping.NET provides a rich set of methods to interact with the Globalping API:

- **MeasurementRequestBuilder**: Fluent API for building measurement requests.
- **ProbeService**: Submit measurements and retrieve probe information.
- **MeasurementClient**: Poll for results and handle caching.
- **GetPingTimings**: Extract timing results from Ping measurements.
- **GetTracerouteHops**: Retrieve hop details from Traceroute measurements.
- **GetDnsRecords**: Parse DNS results into structured records.
- **HttpOptions**: Customize HTTP requests with headers, methods, and query strings.

With these methods, you can create, execute, and analyze network diagnostics with ease, all while leveraging advanced features like Brotli compression and ETag caching.


## Why Globalping.NET?

Globalping.NET is your gateway to running network diagnostics from hundreds of probes worldwide. With support for Ping, Traceroute, MTR, DNS, and HTTP measurements, it simplifies complex tasks into a few lines of code. Here's why you should consider it:

- **Ease of Use**: Fluent APIs for building requests.
- **Flexibility**: Control probe locations, limits, and measurement options.
- **Rich Features**: Built-in caching, Brotli compression, and ETag support.


## Getting Started

### Installation

Add the package from NuGet:

```shell
> dotnet add package Globalping
```

Alternatively, reference the project directly if you're working within the repository.


## Real-World Examples

### Running Measurements Without API Key

You can start using Globalping without an API key. Here's how to run a Ping from Germany:

```csharp
using var httpClient = new HttpClient();
var builder = new MeasurementRequestBuilder()
    .WithType(MeasurementType.Ping)
    .WithTarget("cdn.jsdelivr.net")
    .AddCountry("DE")
    .Build();

var probeService = new ProbeService(httpClient);
var measurement = await probeService.CreateMeasurementAsync(builder);
Console.WriteLine($"Measurement ID: {measurement.Id}");
```

### Running Measurements With API Key

For higher limits and more probes, use the `apiKey` parameter:

```csharp
using var httpClient = new HttpClient();
var builder = new MeasurementRequestBuilder()
    .WithType(MeasurementType.Ping)
    .WithTarget("cdn.jsdelivr.net")
    .AddCountry("DE")
    .Build();

var probeService = new ProbeService(httpClient, "your-api-key");
var measurement = await probeService.CreateMeasurementAsync(builder);
Console.WriteLine($"Measurement ID: {measurement.Id}");
```


## Advanced HTTP Options

For HTTP measurements, you can specify methods, paths, and query strings. Here's an example:

```csharp
var builder = new MeasurementRequestBuilder()
    .WithType(MeasurementType.Http)
    .WithTarget("https://example.com/api/data?x=1")
    .WithMeasurementOptions(new HttpOptions
    {
        Request = new HttpRequestOptions
        {
            Method = HttpRequestMethod.GET,
            Path = "/api/data",
            Query = "x=1"
        },
        Protocol = HttpProtocol.HTTPS,
        IpVersion = IpVersion.Six
    });

var request = builder.Build();
using var httpClient = new HttpClient();
var probeService = new ProbeService(httpClient);
var measurement = await probeService.CreateMeasurementAsync(request);

Console.WriteLine($"HTTP Measurement ID: {measurement.Id}");
```


## Monitoring Limits

Keep an eye on your API usage with the `GetLimitsAsync` method:

```csharp
using var httpClient = new HttpClient();
var probeService = new ProbeService(httpClient, "your-api-key");
var limits = await probeService.GetLimitsAsync();

Console.WriteLine($"Credits Remaining: {limits.CreditsRemaining}");
```


## Cross-Platform Diagnostics

If you're working in both C# and PowerShell, the Globalping ecosystem has you covered. Use [Globalping PowerShell Module](https://evotec.xyz/automating-network-diagnostics-with-globalping-powershell-module/) for quick scripting and automation, and leverage Globalping.NET for robust application development. Together, they form a powerful toolkit for network diagnostics.


## Conclusion

Globalping.NET is a game-changer for network diagnostics. With its intuitive API and powerful features, you can run measurements from diverse locations, control probe behavior, and handle caching with ease. Explore the example project in this repository for more scenarios and advanced usages.

Ready to supercharge your diagnostics? Install Globalping.NET today and start exploring the world of network measurements!