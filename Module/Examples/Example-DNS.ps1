Import-Module $PSScriptRoot\..\Globalping.psd1 -Force

Start-GlobalpingDns -Target "evotec.xyz" -Verbose | Format-Table *

Start-GlobalpingDns -Target "evotec.xyz" -Verbose -Classic | Format-Table

$OutputDns = Start-GlobalpingDns -Target "evotec.xyz" -Verbose -Raw
$OutputDns | Format-Table
$OutputDns.Results | Format-Table
$OutputDns.Results.Probe | Format-Table
$OutputDns.Results.Data | Format-List
$OutputDns.Results[0].Data.RawOutput
$OutputDns.Results[0].ToDnsRecords() | Format-Table *