Import-Module $PSScriptRoot\..\Globalping.psd1 -Force

$loc = @(
    [Globalping.LocationRequest]@{ Country = "DE"; Limit = 1 },
    [Globalping.LocationRequest]@{ Country = "US"; Limit = 1 },
    [Globalping.LocationRequest]@{ Country = "GB"; Limit = 1 },
    [Globalping.LocationRequest]@{ Country = "PL"; Limit = 1 },
    [Globalping.LocationRequest]@{ Country = "FR"; Limit = 1 },
    [Globalping.LocationRequest]@{ Country = "IT"; Limit = 1 },
    [Globalping.LocationRequest]@{ Country = "ES"; Limit = 1 },
    [Globalping.LocationRequest]@{ Country = "NL"; Limit = 1 },
    [Globalping.LocationRequest]@{ Country = "SE"; Limit = 1 },
    [Globalping.LocationRequest]@{ Country = "CH"; Limit = 1 }
)
$locResult = Start-Globalping -Type Ping -Target "evotec.xyz" -Verbose -Locations $loc -ApiKey $env:GLOBALPING_TOKEN
$locResult | Format-Table
$locResult.Results | Format-Table
$locResult.Results.Data | Select-Object -ExcludeProperty RawOutput | Format-Table