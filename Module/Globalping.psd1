﻿@{
    AliasesToExport      = @()
    Author               = 'Przemyslaw Klys'
    CmdletsToExport      = @('Get-GlobalpingLimit', 'Get-GlobalpingProbe', 'Start-GlobalpingDns', 'Start-GlobalpingHttp', 'Start-GlobalpingMtr', 'Start-GlobalpingPing', 'Start-GlobalpingTraceroute')
    CompanyName          = 'Evotec'
    CompatiblePSEditions = @('Desktop', 'Core')
    Copyright            = '(c) 2011 - 2025 Przemyslaw Klys @ Evotec. All rights reserved.'
    Description          = 'Module using globaling.net API to ping any host globally and return results from multiple locations.'
    FunctionsToExport    = @()
    GUID                 = 'a587d150-7ab5-47da-ae3a-3bb879e2c07f'
    ModuleVersion        = '1.0.2'
    PowerShellVersion    = '5.1'
    PrivateData          = @{
        PSData = @{
            ProjectUri = 'https://github.com/EvotecIT/Globalping'
            Tags       = @('ping', 'globalping', 'global', 'pinging', 'network', 'networking', 'internet')
        }
    }
    RootModule           = 'Globalping.psm1'
}