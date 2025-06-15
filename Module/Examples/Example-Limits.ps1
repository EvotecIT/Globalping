Import-Module $PSScriptRoot\..\Globalping.psd1 -Force

# Display friendly limit information
Get-GlobalpingLimit | Format-Table

# Retrieve the raw Limits object
Get-GlobalpingLimit -Raw | Format-List

# Retrieve the limits for a specific API key
Get-GlobalpingLimit -ApiKey "ApiKey" | Format-Table