# Using Globalping PowerShell Module

Globalping exposes a public API for running network diagnostics from probes distributed across the world. The PowerShell module bundles these capabilities into easy-to-use cmdlets so you can script measurements without dealing with raw HTTP requests.

This article mirrors the C# examples and demonstrates the common scenarios when automating tests with PowerShell.

## Available Cmdlets

The module exports the following cmdlets:

- **Start-GlobalpingPing** – run ICMP ping from remote probes and return timing results or classic text.
- **Start-GlobalpingTraceroute** – display each hop packets take to the target host.
- **Start-GlobalpingMtr** – perform an MTR trace that collects per-hop statistics.
- **Start-GlobalpingDns** – resolve domain names using remote resolvers.
- **Start-GlobalpingHttp** – send HTTP requests or header checks from probes.
- **Get-GlobalpingProbe** – list available probes with location information.
- **Get-GlobalpingLimit** – inspect current API usage limits and remaining credits.

## Installation

Install the module from the PowerShell Gallery and import it into your session:

```powershell
Install-Module Globalping -Scope CurrentUser
Import-Module Globalping
```

When working directly with the GitHub repository clone you need .NET 8 SDK and a recent editor such as Visual Studio Code. After cloning and building the project you can load the local module:

```powershell
# clone from https://github.com/EvotecIT/Globalping
# then build the binaries
cd Globalping
 dotnet build
Import-Module ./Module/Globalping.psd1 -Force
```

## Running Measurements

Each measurement type has its own cmdlet. Provide the target host and optionally specify probe locations. When the `-Limit` parameter is omitted the cmdlet selects one probe per location.

```powershell
# Ping from Germany
Start-GlobalpingPing -Target "example.com" -SimpleLocations "DE"
```

Use multiple countries or cities to run from several locations:

```powershell
Start-GlobalpingPing -Target "example.com" -SimpleLocations "DE", "US", "GB"
```

Detailed location requests can be passed via `LocationRequest` objects:

```powershell
$locations = @(
    [Globalping.LocationRequest]@{ Country = "DE"; Limit = 1 },
    [Globalping.LocationRequest]@{ Country = "US"; Limit = 1 }
)
Start-GlobalpingPing -Target "example.com" -Locations $locations
```

Other measurement types work in the same way:

```powershell
Start-GlobalpingTraceroute -Target "example.com"
Start-GlobalpingMtr        -Target "example.com"
Start-GlobalpingDns        -Target "example.com" -Limit 1
Start-GlobalpingHttp       -Target "https://example.com" -HeadersOnly
```

Monitor progress of longer running requests with in‑progress updates:

```powershell
Start-GlobalpingHttp -Target "https://example.com" -Limit 3 -InProgressUpdates -WaitTime 60
```

List available probes or check current usage limits:

```powershell
Get-GlobalpingProbe | Format-Table
Get-GlobalpingLimit
```

## Conclusion

The Globalping PowerShell module exposes dedicated cmdlets for all measurement types. Install it from the PowerShell Gallery or build the project with the .NET 8 SDK when working from the repository. Use simple location strings or `LocationRequest` objects to control where probes run, fetch HTTP headers or raw ping text and monitor your rate limits with ease. Check the example scripts in this repository for more advanced scenarios.

