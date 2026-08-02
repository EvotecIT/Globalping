---
external help file: Globalping-help.xml
Module Name: Globalping
online version: https://github.com/EvotecIT/Globalping
schema: 2.0.0
---
# Get-GlobalpingLimit
## SYNOPSIS
Retrieve current API rate limits.

## SYNTAX
### __AllParameterSets
```powershell
Get-GlobalpingLimit [-ApiKey <string>] [-Raw] [<CommonParameters>]
```

## DESCRIPTION
Calls the Globalping /limits endpoint and returns limit information.

## EXAMPLES

### EXAMPLE 1
```powershell
PS> Get-GlobalpingLimit
```

Returns rate limit information for the anonymous user or provided API key.

### EXAMPLE 2
```powershell
PS> Get-GlobalpingLimit -Raw
```

Outputs the unprocessed Limits response.

## PARAMETERS

### -ApiKey
Provide this to view limits associated with your account.

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
Outputs the service response without flattening to LimitInfo.

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

- `Globalping.LimitInfo`
- `Globalping.Limits`
- `Globalping.Credits`

## RELATED LINKS

- [https://learn.microsoft.com/powershell/](https://learn.microsoft.com/powershell/)
- [https://github.com/EvotecIT/Globalping](https://github.com/EvotecIT/Globalping)

## NOTES

### Note

Unauthenticated requests may report lower limits.
