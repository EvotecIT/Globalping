Import-Module $PSScriptRoot\..\Globalping.psd1 -Force


Start-Globalping -Type Ping -Target "evotec.xyz" -Verbose

Start-Globalping -Type Ping -Target "evotec.xyz" -Verbose -Locations "DE"