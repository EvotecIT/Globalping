Import-Module $PSScriptRoot\..\Globalping.psd1 -Force

Start-GlobalpingTraceroute -Target "evotec.xyz" -Verbose | Format-Table *

Start-GlobalpingTraceroute -Target "evotec.xyz" -Verbose -Classic | Format-Table

$OutputTrace = Start-GlobalpingTraceroute -Target "evotec.xyz" -Verbose -Raw
$OutputTrace | Format-Table
$OutputTrace.Results | Format-Table
$OutputTrace.Results.Probe | Format-Table
$OutputTrace.Results.Data | Format-List
$OutputTrace