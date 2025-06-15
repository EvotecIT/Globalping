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
        $results = Start-GlobalpingHttp -Target "evotec.xyz" -Limit 1 -ErrorAction Stop
        $results | Should -Not -BeNullOrEmpty
        $results.Target | Should -Be "evotec.xyz"
        $results.StatusCode | Should -Be 200
    }

    It "Start-GlobalpingHttp handles URL target" {
        $results = Start-GlobalpingHttp -Target "https://evotec.xyz" -Limit 1 -ErrorAction Stop
        $results | Should -Not -BeNullOrEmpty
        $results.Target | Should -Be "evotec.xyz"
    }

    It "Start-GlobalpingHttp returns headers when HeadersOnly is set" {
        $results = Start-GlobalpingHttp -Target "evotec.xyz" -Limit 1 -HeadersOnly -ErrorAction Stop
        $results | Should -Not -BeNullOrEmpty
        $header = $results | Select-Object -First 1
        ($header -is [System.Collections.IDictionary]) | Should -BeTrue
    }

    It "Start-GlobalpingTraceroute returns output" {
        $results = Start-GlobalpingTraceroute -Target "evotec.xyz" -Limit 1 -ErrorAction Stop
        $results | Should -Not -BeNullOrEmpty
    }

    It "Start-GlobalpingMtr returns output" {
        $results = Start-GlobalpingMtr -Target "evotec.xyz" -Limit 1 -ErrorAction Stop
        $results | Should -Not -BeNullOrEmpty
    }

    It "Start-GlobalpingDns classic output returns string" {
        $results = Start-GlobalpingDns -Target "evotec.xyz" -Limit 1 -Classic -ErrorAction Stop
        $results | Should -Not -BeNullOrEmpty
        $output = $results | Select-Object -First 1
        $output | Should -BeOfType [string]
    }

    It "Start-GlobalpingMtr classic output returns string" {
        $results = Start-GlobalpingMtr -Target "evotec.xyz" -Limit 1 -Classic -ErrorAction Stop
        $results | Should -Not -BeNullOrEmpty
        $output = $results | Select-Object -First 1
        $output | Should -BeOfType [string]
    }

    It "Start-GlobalpingPing accepts PingOptions object" {
        $opts = [Globalping.PingOptions]@{ Packets = 2 }
        $results = Start-GlobalpingPing -Target "evotec.xyz" -Limit 1 -Options $opts -ErrorAction Stop
        $results | Should -Not -BeNullOrEmpty
        $result = $results | Select-Object -First 1
        $result | Should -BeOfType [Globalping.PingTimingResult]
    }

    It "Start-GlobalpingHttp accepts HttpOptions object" {
        $opts = [Globalping.HttpOptions]@{ Port = 443 }
        $results = Start-GlobalpingHttp -Target "https://evotec.xyz" -Limit 1 -Options $opts -ErrorAction Stop
        $results | Should -Not -BeNullOrEmpty
        $result = $results | Select-Object -First 1
        $result | Should -BeOfType [Globalping.HttpResponseResult]
    }

    It "Get-GlobalpingLimit returns output" {
        $limits = Get-GlobalpingLimit -ErrorAction Stop
        $limits | Should -Not -BeNullOrEmpty
    }

    It "Rejects limit below valid range" {
        { Start-GlobalpingPing -Target "evotec.xyz" -Limit 0 -ErrorAction Stop } |
            Should -Throw -ErrorId 'ParameterArgumentValidationError,Globalping.PowerShell.StartGlobalpingPingCommand'
    }

    It "Rejects limit above valid range" {
        { Start-GlobalpingPing -Target "evotec.xyz" -Limit 501 -ErrorAction Stop } |
            Should -Throw -ErrorId 'ParameterArgumentValidationError,Globalping.PowerShell.StartGlobalpingPingCommand'
    }
}
