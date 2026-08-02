---
external help file: Globalping-help.xml
Module Name: Globalping
online version: https://github.com/EvotecIT/Globalping
schema: 2.0.0
---
# Get-GlobalpingProbe
## SYNOPSIS
Retrieve currently online probes.

## SYNTAX
### __AllParameterSets
```powershell
Get-GlobalpingProbe [-ApiKey <string>] [-Raw] [<CommonParameters>]
```

## DESCRIPTION
Calls the Globalping /probes endpoint and returns probe information.

## EXAMPLES

### EXAMPLE 1
```powershell
PS> Get-GlobalpingProbe
```

Outputs flattened probe objects describing each online probe.

### EXAMPLE 2
```powershell
PS> Get-GlobalpingProbe -Raw
```

Displays the unprocessed Probes response.

## PARAMETERS

### -ApiKey
Include when accessing endpoints that require authentication.

```yaml
Type: String
Parameter Sets: __AllParameterSets
Aliases: Token
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Raw
Outputs the API response without conversion to ProbeInfo.

```yaml
Type: SwitchParameter
Parameter Sets: __AllParameterSets
Aliases: AsRaw
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutVariable, -OutBuffer, -PipelineVariable, -Verbose, -WarningAction, and -WarningVariable. For more information, see [about_CommonParameters](http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

- `None`

## OUTPUTS

- `Globalping.ProbeInfo`
- `Globalping.Probes`

## RELATED LINKS

- [https://learn.microsoft.com/powershell/](https://learn.microsoft.com/powershell/)
- [https://github.com/EvotecIT/Globalping](https://github.com/EvotecIT/Globalping)

## NOTES

### Note

Probe availability can change between requests.
