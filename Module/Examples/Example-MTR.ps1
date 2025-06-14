Import-Module $PSScriptRoot\..\Globalping.psd1 -Force

Start-GlobalpingMtr -Target "evotec.xyz" -Verbose | Format-Table

Start-GlobalpingMtr -Target "evotec.xyz" -Verbose -Classic | Format-Table

$OutputMtr = Start-GlobalpingMtr -Target "evotec.xyz" -Verbose -Raw
$OutputMtr | Format-Table
$OutputMtr.Results | Format-Table
$OutputMtr.Results.Probe | Format-Table
$OutputMtr.Results.Data | Format-List
$OutputMtr.Results[0].Data.RawOutput
$OutputMtr.Results[0].ToMtrHops() | Format-Table