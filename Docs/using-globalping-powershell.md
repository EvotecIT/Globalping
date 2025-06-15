Are you tired of manually running network diagnostics like Ping, Traceroute, or DNS queries? The **Globalping PowerShell Module** is here to save the day! With its easy-to-use cmdlets, you can automate measurements from probes distributed across the globe.

In this blog, we'll explore the features of the Globalping PowerShell Module, showcase real-world examples, and help you get started with scripting your network tests. If you're a developer working in C#, check out our companion blog on [Globalping.NET](https://evotec.xyz/supercharging-your-network-diagnostics-with-globalping-for-net/) to see how these tools complement each other.

## Free API with Generous Limits

Globalping is a free API that requires no registration to get started. Here are the limits:

- **Unregistered Users**:
  - 50 probes per measurement
  - 250 free tests per hour
- **Registered Users**:
  - 500 probes per measurement
  - 500 free tests per hour

Higher limits are available for members, making it ideal for both casual and professional use.

## Available Cmdlets

The Globalping PowerShell Module offers a rich set of cmdlets to cover all your network diagnostic needs:

- **Start-GlobalpingPing**: Run ICMP ping from remote probes and return timing results or classic text.
- **Start-GlobalpingTraceroute**: Display each hop packets take to the target host.
- **Start-GlobalpingMtr**: Perform an MTR trace that collects per-hop statistics.
- **Start-GlobalpingDns**: Resolve domain names using remote resolvers.
- **Start-GlobalpingHttp**: Send HTTP requests or header checks from probes.
- **Get-GlobalpingProbe**: List available probes with location information.
- **Get-GlobalpingLimit**: Inspect current API usage limits and remaining credits.

With these cmdlets, you can script everything from simple pings to advanced HTTP diagnostics, all while monitoring your API usage and probe availability.

## Why Globalping PowerShell?

Globalping PowerShell simplifies network diagnostics by bundling the Globalping API into cmdlets. Here's why it's awesome:

- **Cmdlet Simplicity**: No need to deal with raw HTTP requests.
- **Location Control**: Run diagnostics from specific countries, cities, or even cloud providers.
- **Rich Output**: Retrieve detailed results in table or list formats.

## Getting Started

### Installation

Install the module from the PowerShell Gallery:

```powershell
Install-Module Globalping -Scope CurrentUser
Import-Module Globalping
```

Working directly with the repository? Clone and build it:

```powershell
cd Globalping
 dotnet build
Import-Module ./Module/Globalping.psd1 -Force
```

## Real-World Examples

### Running Measurements Without API Key

You can start using Globalping without an API key. Here's how to run a Ping from Germany:

```powershell
Start-GlobalpingPing -Target "example.com" -SimpleLocations "DE"
```

Want to run from multiple locations? Just add more countries:

```powershell
Start-GlobalpingPing -Target "example.com" -SimpleLocations "DE", "US", "GB"
```

### Running Measurements With API Key

For higher limits and more probes, use the `-ApiKey` parameter:

```powershell
Start-GlobalpingPing -Target "example.com" -SimpleLocations "DE" -ApiKey "your-api-key"
```

### DNS Queries

Resolve domain names with ease:

```powershell
Start-GlobalpingDns -Target "evotec.xyz" -Verbose | Format-Table *
```

Retrieve raw DNS results:

```powershell
$OutputDns = Start-GlobalpingDns -Target "evotec.xyz" -Verbose -Raw
$OutputDns.Results[0].ToDnsRecords() | Format-Table *
```

### HTTP Measurements

Send HTTP requests or check headers from probes:

```powershell
$Output = Start-GlobalpingHttp -Target "evotec.xyz" -Verbose -SimpleLocations "Krakow+PL"
$Output.Headers | Format-Table
```

Retrieve raw HTTP results:

```powershell
$OutputHttp = Start-GlobalpingHttp -Target "evotec.xyz" -Verbose -Raw
$OutputHttp.Results[0].Data.Headers | Format-Table
```

### MTR and Traceroute

Perform MTR traces with detailed hop statistics:

```powershell
$OutputMtr = Start-GlobalpingMtr -Target "evotec.xyz" -Verbose -Raw -SimpleLocations "Krakow+PL", "Berlin+DE"
$OutputMtr.Results[0].ToMtrHops() | Format-Table *
```

Run a Traceroute:

```powershell
Start-GlobalpingTraceroute -Target "evotec.xyz" -Verbose | Format-Table *
```

### Probes and Limits

List available probes:

```powershell
Get-GlobalpingProbe | Select-Object -First 5 | Format-Table *
```

Monitor your API usage:

```powershell
Get-GlobalpingLimit -ApiKey "your-api-key" | Format-Table
```

Retrieve raw limit information:

```powershell
Get-GlobalpingLimit -ApiKey "your-api-key" -Raw | Format-List
```

## Cross-Platform Diagnostics

If you're working in both PowerShell and C#, the Globalping ecosystem has you covered. Use the PowerShell module for quick scripting and automation, and leverage [Globalping.NET](./using-globalping-csharp.md) for robust application development. Together, they form a powerful toolkit for network diagnostics.

## Conclusion

The Globalping PowerShell Module is a must-have for automating network diagnostics. With its intuitive cmdlets and powerful features, you can run measurements from diverse locations, fetch detailed results, and monitor your API limits effortlessly.

Ready to automate your network tests? Install the Globalping PowerShell Module today and start scripting your diagnostics!

