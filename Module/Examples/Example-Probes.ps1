Import-Module $PSScriptRoot\..\Globalping.psd1 -Force

# Display flattened probe information
Get-GlobalpingProbe | Select-Object -First 5 | Format-Table *

# Retrieve the raw probe objects
Get-GlobalpingProbe -Raw | Select-Object -First 5 | Format-Table *
