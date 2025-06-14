Import-Module $PSScriptRoot\..\Globalping.psd1 -Force

Start-Globalping -Type Ping -Target "evotec.xyz" -Verbose | Format-Table

Start-Globalping -Type Ping -Target "evotec.xyz" -Verbose -SimpleLocations "DE" | Format-Table

Start-Globalping -Type Ping -Target "evotec.xyz" -Verbose -SimpleLocations "DE", "US", "UK" | Format-Table

Start-Globalping -Type Ping -Target "evotec.xyz" -Verbose -Locations "DE", "US", "UK", "PL", "FR", "IT", "ES", "NL", "SE", "CH" | Format-Table