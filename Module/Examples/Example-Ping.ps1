Import-Module $PSScriptRoot\..\Globalping.psd1 -Force

Start-Globalping -Type Ping -Target "evotec.xyz" -Verbose | Format-Table

Start-Globalping -Type Ping -Target "evotec.xyz" -Verbose -SimpleLocations "DE" | Format-Table

Start-Globalping -Type Ping -Target "evotec.xyz" -Verbose -SimpleLocations "DE", "US", "UK" | Format-Table

$loc = @(
    [Globalping.LocationRequest]@{ Country = "DE"; Limit = 1 },
    [Globalping.LocationRequest]@{ Country = "US"; Limit = 1 },
    [Globalping.LocationRequest]@{ Country = "UK"; Limit = 1 },
    [Globalping.LocationRequest]@{ Country = "PL"; Limit = 1 },
    [Globalping.LocationRequest]@{ Country = "FR"; Limit = 1 },
    [Globalping.LocationRequest]@{ Country = "IT"; Limit = 1 },
    [Globalping.LocationRequest]@{ Country = "ES"; Limit = 1 },
    [Globalping.LocationRequest]@{ Country = "NL"; Limit = 1 },
    [Globalping.LocationRequest]@{ Country = "SE"; Limit = 1 },
    [Globalping.LocationRequest]@{ Country = "CH"; Limit = 1 }
)
Start-Globalping -Type Ping -Target "evotec.xyz" -Verbose -Locations $loc | Format-Table

