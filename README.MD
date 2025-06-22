<p align="center">
  <a href="https://github.com/EvotecIT/Globalping"><img src="https://img.shields.io/github/languages/top/evotecit/Globalping.svg"></a>
  <a href="https://github.com/EvotecIT/Globalping"><img src="https://img.shields.io/github/languages/code-size/evotecit/Globalping.svg"></a>
</p>

<p align="center">
  <a href="https://github.com/EvotecIT/Globalping/actions/workflows/test-dotnet.yml"><img src="https://github.com/EvotecIT/Globalping/actions/workflows/test-dotnet.yml/badge.svg"></a>
  <a href="https://codecov.io/gh/EvotecIT/Globalping"><img src="https://img.shields.io/codecov/c/github/evotecit/Globalping?token=BVO3QKQEHF" /></a>
  <a href="https://www.nuget.org/packages/Globalping"><img src="https://img.shields.io/nuget/v/Globalping"></a>
<a href="https://www.nuget.org/packages/Globalping"><img src="https://img.shields.io/nuget/dt/Globalping?label=nuget%20downloads"></a>
</p>

<p align="center">
  <a href="https://github.com/EvotecIT/Globalping/actions/workflows/test-powershell-module.yml"><img src="https://github.com/EvotecIT/Globalping/actions/workflows/test-powershell-module.yml/badge.svg"></a>
  <a href="https://www.powershellgallery.com/packages/Globalping"><img src="https://img.shields.io/powershellgallery/v/Globalping.svg?label=powershell"></a>
  <a href="https://www.powershellgallery.com/packages/Globalping"><img src="https://img.shields.io/powershellgallery/vpre/Globalping.svg?label=powershell%20preview&colorB=yellow"></a>
  <a href="https://www.powershellgallery.com/packages/Globalping"><img src="https://img.shields.io/powershellgallery/dt/Globalping.svg"></a>
</p>

<p align="center">
  <a href="https://twitter.com/PrzemyslawKlys"><img src="https://img.shields.io/twitter/follow/PrzemyslawKlys.svg?label=Twitter%20%40PrzemyslawKlys&style=social"></a>
  <a href="https://evotec.xyz/hub"><img src="https://img.shields.io/badge/Blog-evotec.xyz-2A6496.svg"></a>
  <a href="https://www.linkedin.com/in/pklys"><img src="https://img.shields.io/badge/LinkedIn-pklys-0077B5.svg?logo=LinkedIn"></a>
</p>

# Globalping

Globalping provides convenient access to the [Globalping API](https://globalping.io/) from .NET applications and PowerShell. It lets you run network diagnostics such as ping, traceroute, DNS or HTTP checks from hundreds of probes around the world.

The repository contains a cross platform .NET library and a PowerShell module built on top of it. Examples under `Globalping.Examples` demonstrate advanced usage.

## Goals

* Make Globalping easy to consume from scripts and applications.
* Offer rich objects for each measurement type while still allowing access to the raw API response.
* Keep dependencies minimal - only the .NET runtime and PowerShell 5.1 or newer are required.

## Requirements

* **.NET** 8 or newer to build and run the library and example projects.
* **PowerShell** 5.1 or newer when using the module. PowerShell 7+ is recommended on non‑Windows platforms.

## PowerShell usage

Build the project and import the module:

```powershell
# After `dotnet build` the module can be loaded
Import-Module ./Module/Globalping.psd1 -Force
```

Run a simple measurement:

```powershell
Start-Globalping -Type Ping -Target "example.com" -SimpleLocations "DE" | Format-Table
```

To use the `Locations` parameter provide `LocationRequest` objects. The cmdlet will automatically run one probe for each location when `Limit` is not specified:

```powershell
$locations = @(
    [Globalping.LocationRequest]@{ Country = "DE"; Limit = 1 },
    [Globalping.LocationRequest]@{ Country = "US"; Limit = 1 }
)
Start-Globalping -Type Ping -Target "example.com" -Locations $locations | Format-Table
```

```powershell
# request three probes from specific countries
$result = Start-Globalping -Type Ping -Target "example.com" -SimpleLocations "DE", "US", "GB"
$result.GetSummaries() | Format-Table
```

Use ISO 3166-1 alpha-2 country codes (e.g. `GB` for United Kingdom).

To easily access key data from the API response use the `GetSummaries()` extension method:

```powershell
$result = Start-Globalping -Type Ping -Target "example.com" -SimpleLocations "DE"
$result.GetSummaries() | Format-Table
```

By default `Start-Globalping` will return individual ping timings flattened with
location information. To get the raw API response pass `-Raw`:

```powershell
Start-Globalping -Type Ping -Target "example.com" -SimpleLocations "DE" | Format-Table
```

```powershell
# return the original `MeasurementResponse`
Start-Globalping -Type Ping -Target "example.com" -SimpleLocations "DE" -Raw
```

Convert a single result to structured objects:

```powershell
$mtr = Start-GlobalpingMtr -Target "example.com" -Raw
$hops = $mtr.Results[0].ToMtrHops()
$hops[0] | Format-Table
```

Use `-Classic` to output the raw ping text similar to the traditional `ping` command:

```powershell
Start-Globalping -Type Ping -Target "example.com" -SimpleLocations "DE" -Classic
```

To fetch only HTTP headers use `Start-GlobalpingHttp` with the `-HeadersOnly` parameter:

```powershell
Start-GlobalpingHttp -Target "https://example.com" -HeadersOnly
```
Both `https://example.com` and `example.com` are accepted when running HTTP measurements. The cmdlet automatically extracts the scheme and path.

Request in-progress updates and customize the wait time:

```powershell
Start-GlobalpingHttp -Target "https://example.com" -Limit 3 -InProgressUpdates -WaitTime 60
```

List currently available probes:

```powershell
Get-GlobalpingProbe | Format-Table
```

Return the raw `Probes` objects:

```powershell
Get-GlobalpingProbe -Raw | Format-Table
```

## C# usage

The .NET library exposes the same functionality for applications. A minimal example to run a ping measurement:

```csharp
using var httpClient = new HttpClient();
var request = new MeasurementRequestBuilder()
    .WithType(MeasurementType.Ping)
    .WithTarget("example.com")
    .AddCountry("DE")
    .Build();

var service = new ProbeService(httpClient);
var create = await service.CreateMeasurementAsync(request);

var client = new MeasurementClient(httpClient);
var result = await client.GetMeasurementByIdAsync(create.Id);
foreach (var ping in result!.GetPingTimings())
{
    Console.WriteLine($"{ping.City}: {ping.Time} ms");
}
```

More advanced requests can customize locations, limits and measurement options:

```csharp
var advanced = new MeasurementRequestBuilder()
    .WithType(MeasurementType.Http)
    .WithTarget("https://example.com/api/data?x=1")
    .AddCountry("DE", 2)
    .AddCity("Berlin")
    .WithLimit(3)
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

For HTTP measurements the target may include the scheme (e.g. `https://example.com`). The library automatically converts it to the correct host, protocol and path.

The API specification is available at [api.globalping.io/v1/spec.yaml](https://api.globalping.io/v1/spec.yaml).

By default both the .NET client and PowerShell module set helpful HTTP headers.
When no `User-Agent` is specified, `Globalping.Net/1.0` is used. If Brotli
compression is available the client sends `Accept-Encoding: br`, otherwise it
falls back to `gzip`.

All API clients expose the most recent usage headers via the `LastResponseInfo`
property. When the service returns values such as `X-RateLimit-Limit` or
`X-Credits-Remaining` they will be parsed into this object for easy inspection.

`MeasurementClient` supports basic caching via `ETag`. Pass the `LastResponseInfo.ETag`
value to `GetMeasurementByIdAsync` or `GetMeasurementById`. When the API returns
`304 Not Modified` the previously retrieved measurement data is reused and parsing
is skipped.
