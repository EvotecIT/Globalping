Describe "Globalping Cmdlets" {
    BeforeAll {
        Import-Module "$PSScriptRoot/../Globalping.psd1" -Force
    }

    It "Start-GlobalpingDns returns records" {
        $results = Start-GlobalpingDns -Target "evotec.xyz" -Limit 1 -ErrorAction Stop
        $results | Should -Not -BeNullOrEmpty
        $record = $results | Select-Object -First 1
        $record.Country | Should -Not -BeNullOrEmpty
        $record.City | Should -Not -BeNullOrEmpty
        $record.QuestionName | Should -Not -BeNullOrEmpty
    }

    It "Start-GlobalpingPing returns output" {
        $results = Start-GlobalpingPing -Target "evotec.xyz" -Limit 1 -ErrorAction Stop
        $results | Should -Not -BeNullOrEmpty
    }

    It "Start-GlobalpingHttp returns output" {
        $results = Start-GlobalpingHttp -Target "https://evotec.xyz" -Limit 1 -ErrorAction Stop
        $results | Should -Not -BeNullOrEmpty
    }

    It "Start-GlobalpingTraceroute returns output" {
        $results = Start-GlobalpingTraceroute -Target "evotec.xyz" -Limit 1 -ErrorAction Stop
        $results | Should -Not -BeNullOrEmpty
    }

    It "Start-GlobalpingMtr returns output" {
        $results = Start-GlobalpingMtr -Target "evotec.xyz" -Limit 1 -ErrorAction Stop
        $results | Should -Not -BeNullOrEmpty
    }

    It "Get-GlobalpingLimit returns output" {
        $limits = Get-GlobalpingLimit -ErrorAction Stop
        $limits | Should -Not -BeNullOrEmpty
    }
}
