Import-Module $PSScriptRoot\..\Globalping.psd1 -Force

Start-GlobalpingTraceroute -Target "evotec.xyz" -Verbose | Format-Table