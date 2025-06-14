Import-Module $PSScriptRoot\..\Globalping.psd1 -Force

Start-GlobalpingPing -Target "evotec.xyz" -Verbose | Format-Table

Start-GlobalpingPing -Target "evotec.xyz" -Verbose -SimpleLocations "DE" | Format-Table
