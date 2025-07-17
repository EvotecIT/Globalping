Import-Module $PSScriptRoot\..\Globalping.psd1 -Force

$loc = @(
    [Globalping.LocationRequest]@{ Country = [Globalping.CountryCode]::Germany; Limit = 1 },
    [Globalping.LocationRequest]@{ Country = [Globalping.CountryCode]::UnitedStates; Limit = 1 },
    [Globalping.LocationRequest]@{ Country = [Globalping.CountryCode]::UnitedKingdom; Limit = 1 },
    [Globalping.LocationRequest]@{ Country = [Globalping.CountryCode]::Poland; Limit = 1 },
    [Globalping.LocationRequest]@{ Country = [Globalping.CountryCode]::France; Limit = 1 },
    [Globalping.LocationRequest]@{ Country = [Globalping.CountryCode]::Italy; Limit = 1 },
    [Globalping.LocationRequest]@{ Country = [Globalping.CountryCode]::Spain; Limit = 1 },
    [Globalping.LocationRequest]@{ Country = [Globalping.CountryCode]::Netherlands; Limit = 1 },
    [Globalping.LocationRequest]@{ Country = [Globalping.CountryCode]::Sweden; Limit = 1 },
    [Globalping.LocationRequest]@{ Country = [Globalping.CountryCode]::Switzerland; Limit = 1 }
)
$locResult = Start-GlobalpingPing -Target "evotec.xyz" -Verbose -Locations $loc -ApiKey $env:GLOBALPING_TOKEN
$locResult | Format-Table
$locResult.Results | Format-Table
$locResult.Results.Data | Select-Object -ExcludeProperty RawOutput | Format-Table