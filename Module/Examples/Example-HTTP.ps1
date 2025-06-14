Import-Module $PSScriptRoot\..\Globalping.psd1 -Force

$Output = Start-GlobalpingHttp -Target "evotec.xyz" -Verbose -SimpleLocations "Krakow+PL"
$Output | Format-Table
$Output.Headers | Format-Table
$Output.Headers['expires']
$Output.Headers['cache-control']

Start-GlobalpingHttp -Target "evotec.xyz" -Verbose -Classic | Format-Table

$OutputHttp = Start-GlobalpingHttp -Target "evotec.xyz" -Verbose -Raw
$OutputHttp | Format-Table
$OutputHttp.Results | Format-Table
$OutputHttp.Results.Probe | Format-Table
$OutputHttp.Results.Data | Format-List
$OutputHttp.Results[0].Data.RawOutput
$OutputHttp.Results[0].Data.Headers | Format-Table