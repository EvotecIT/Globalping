Import-Module $PSScriptRoot\..\Globalping.psd1 -Force

Start-Globalping -Type Ping -Target "evotec.xyz" -Verbose | Format-Table

Start-Globalping -Type Ping -Target "evotec.xyz" -Verbose -SimpleLocations "DE" | Format-Table