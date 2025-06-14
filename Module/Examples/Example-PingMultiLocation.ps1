Import-Module $PSScriptRoot\..\Globalping.psd1 -Force

$multi = Start-GlobalpingPing -Target "evotec.xyz" -SimpleLocations "DE", "US", "GB"
$multi | Format-Table *

$multiClassic = Start-GlobalpingPing -Target "evotec.xyz" -SimpleLocations "DE", "US", "GB" -Classic
$multiClassic

$multiRaw = Start-GlobalpingPing -Target "evotec.xyz" -SimpleLocations "DE", "US", "GB" -AsRaw
$multiRaw | Format-Table
$multiRaw.Results | Format-Table
$multiRaw.Results.Probe | Format-Table
$multiRaw.Results.Data | Select-Object -ExcludeProperty RawOutput | Format-Table