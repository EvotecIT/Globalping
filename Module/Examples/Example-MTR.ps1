Import-Module $PSScriptRoot\..\Globalping.psd1 -Force

Start-GlobalpingMtr -Target "evotec.xyz" -Verbose | Format-Table

Start-GlobalpingMtr -Target "evotec.xyz" -Verbose -Classic | Format-Table

$OutputMtr = Start-GlobalpingMtr -Target "evotec.xyz" -Verbose -Raw -SimpleLocations "Krakow+PL","Warsaw+PL", "Berlin+DE", "London+UK", "NewYork+US", "SanFrancisco+US", "Tokyo+JP"
$OutputMtr | Format-Table
$OutputMtr.Results | Format-Table
$OutputMtr.Results.Probe | Format-Table
$OutputMtr.Results.Data | Format-List
$OutputMtr.Results[0].Data.RawOutput
$hops = $OutputMtr.Results[0].ToMtrHops()
$hops[0] | Format-Table *
$hops[1] | Format-Table *

