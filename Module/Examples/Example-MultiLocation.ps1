Import-Module $PSScriptRoot\..\Globalping.psd1 -Force

$multi = Start-Globalping -Type Ping -Target "evotec.xyz" -Verbose -SimpleLocations "DE", "US", "GB"
$multi | Format-Table
$multi.Results | Format-Table
$multi.Results.Probe | Format-Table
$multi.Results.Data | Select-Object -ExcludeProperty RawOutput | Format-Table