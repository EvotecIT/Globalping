Import-Module $PSScriptRoot\..\Globalping.psd1 -Force

# Display friendly limit information
Get-GlobalpingLimit | Format-Table

# Retrieve the raw Limits object
Get-GlobalpingLimit -Raw | Format-List
