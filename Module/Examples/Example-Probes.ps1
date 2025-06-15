Import-Module $PSScriptRoot\..\Globalping.psd1 -Force

# Display flattened probe information
Get-GlobalpingProbe | Format-Table

# Retrieve the raw probe objects
Get-GlobalpingProbe -Raw | Format-Table
