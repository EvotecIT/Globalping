# Using Globalping.NET Library in C#

Globalping is a public service that lets you run classic network commands from hundreds of probes distributed around the world. Through a simple HTTP API you can measure latency, trace packet routes or resolve DNS records from almost any region.

This article shows how to use the Globalping.NET client library in your C# projects. Before diving into code, let's briefly look at the available measurement types.

## Measurement Types

Globalping exposes five kinds of diagnostics:
- **Ping** – measure round-trip time with ICMP echo requests.
- **Traceroute** – display each hop packets take to the target host.
- **MTR** – run a continuous traceroute that also collects per-hop statistics.
- **DNS** – resolve domain names using remote resolvers.
- **HTTP** – perform HTTP requests or header checks from remote probes.

## Installation

Add the package from NuGet:

```shell
dotnet add package Globalping
```

Alternatively, reference the project directly if you are working within the repository.

## Creating Measurements

The main entry point for measurements is `MeasurementRequestBuilder`. This fluent class allows you to define probe locations, select the measurement type and specify additional options. A simple ping request to run from Germany might look like this:

```csharp
using var httpClient = new HttpClient();
var request = new MeasurementRequestBuilder()
    .WithType(MeasurementType.Ping)
    .WithTarget("example.com")
    .AddCountry("DE")
    .Build();

var service = new ProbeService(httpClient);
var create = await service.CreateMeasurementAsync(request);
```

`CreateMeasurementAsync` submits the job to Globalping and returns the newly created measurement identifier.

## Polling for Results

After creating a measurement, poll the API to retrieve the final results. `MeasurementClient` can be reused across measurements and caches the most recent response headers.

```csharp
var client = new MeasurementClient(httpClient);
var result = await client.GetMeasurementByIdAsync(create.Id);
foreach (var ping in result!.GetPingTimings())
{
    Console.WriteLine($"{ping.City}: {ping.Time} ms");
}
```

The library exposes rich objects for each measurement type. In this example `GetPingTimings` flattens the response and yields the time values directly.

## Combining Locations and Limits

Use `AddCountry`, `AddCity` or other location helpers multiple times when you need probes from specific places. When the `Limit` is not set, the service selects a single probe for each location request.

```csharp
var complex = new MeasurementRequestBuilder()
    .WithType(MeasurementType.Traceroute)
    .WithTarget("example.com")
    .AddCountry("US")
    .AddCity("Berlin")
    .WithLimit(2)
    .Build();
```

The example above asks for two probes total, one located in the United States and one in Berlin.

## HTTP Options

For HTTP measurements, `HttpOptions` offers additional control over the request that Globalping sends from a probe. You can specify the method, path and query string separately while still providing the full URL as the target.

```csharp
var advanced = new MeasurementRequestBuilder()
    .WithType(MeasurementType.Http)
    .WithTarget("https://example.com/api/data?x=1")
    .WithInProgressUpdates()
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
    })
    .Build();
```

## Handling API Usage Information

Each request exposes a `LastResponseInfo` property. It includes parsed headers such as the remaining credits and rate-limit details returned by the API. Inspect this object to track usage and keep an eye on limits:

```csharp
var info = client.LastResponseInfo;
Console.WriteLine($"Credits remaining: {info.CreditsRemaining}");
```

## Caching with ETag

`MeasurementClient` supports ETag-based caching. Pass the previously retrieved `ETag` when fetching a measurement again. If the data is unchanged, the service returns `304 Not Modified` and the library uses the existing response.

```csharp
var cached = await client.GetMeasurementByIdAsync(create.Id);
var next = await client.GetMeasurementByIdAsync(create.Id, cached!.LastResponseInfo.ETag);
```

## Conclusion

The Globalping library streamlines calls to the Globalping API from C# code. It lets you run diagnostics from diverse locations, control how probes behave and handle common features like caching and Brotli compression out of the box. Explore the example project in this repository for more scenarios and advanced usages.

