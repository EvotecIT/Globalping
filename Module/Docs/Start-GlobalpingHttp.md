---
external help file: Globalping-help.xml
Module Name: Globalping
online version: https://github.com/EvotecIT/Globalping
schema: 2.0.0
---
# Start-GlobalpingHttp
## SYNOPSIS
Start an HTTP request using Globalping.

## SYNTAX
### __AllParameterSets
```powershell
Start-GlobalpingHttp -Target <string[]> [-Raw] [-Classic] [-HeadersOnly] [-Options <HttpOptions>] [-Locations <LocationRequest[]>] [-ReuseLocationsFromId <string>] [-SimpleLocations <string[]>] [-Limit <Int32>] [-InProgressUpdates] [-WaitTime <int>] [-ApiKey <string>] [<CommonParameters>]
```

## DESCRIPTION
Sends an HTTP request from remote probes and returns HttpResponseResult objects.

## EXAMPLES

### EXAMPLE 1
```powershell
PS> Start-GlobalpingHttp -Target "evotec.xyz" -SimpleLocations "Krakow+PL"
```

Returns HTTP response details from the Krakow probe.

### EXAMPLE 2
```powershell
PS> Start-GlobalpingHttp -Target "https://example.com" -HeadersOnly
```

Outputs only the HTTP headers from each probe.

## PARAMETERS

### -ApiKey
Anonymous requests may be rate limited.

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

### -Classic
Each probe returns the textual output of the underlying HTTP tool.

```yaml
Type: SwitchParameter
Parameter Sets: __AllParameterSets
Aliases: None
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -HeadersOnly
Ignores the body content from the probes.

```yaml
Type: SwitchParameter
Parameter Sets: __AllParameterSets
Aliases: None
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -InProgressUpdates
When set the API streams partial results that are written as they arrive.

```yaml
Type: SwitchParameter
Parameter Sets: __AllParameterSets
Aliases: None
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Limit
If omitted the cmdlet estimates a value based on provided locations.

```yaml
Type: Int32
Parameter Sets: __AllParameterSets
Aliases: None
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Locations
Each LocationRequest may specify city, country, ASN or provider details.

```yaml
Type: LocationRequest[]
Parameter Sets: __AllParameterSets
Aliases: None
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Options
Allows setting request method, headers or body.

```yaml
Type: HttpOptions
Parameter Sets: __AllParameterSets
Aliases: None
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Raw
The full MeasurementResponse is emitted without conversion.

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

### -ReuseLocationsFromId
Reuse probe locations from a previous measurement.

```yaml
Type: String
Parameter Sets: __AllParameterSets
Aliases: None
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -SimpleLocations
Two-letter strings are treated as ISO country codes. Longer values map to the "magic" location syntax used by the API.

```yaml
Type: String[]
Parameter Sets: __AllParameterSets
Aliases: None
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Target
Each value is passed verbatim to the underlying measurement API.

```yaml
Type: String[]
Parameter Sets: __AllParameterSets
Aliases: None
Possible values:

Required: True
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -WaitTime
Only applies when InProgressUpdates is specified.

```yaml
Type: Int32
Parameter Sets: __AllParameterSets
Aliases: None
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

- `Globalping.HttpResponseResult`
- `System.String`
- `Globalping.MeasurementResponse`

## RELATED LINKS

- [https://learn.microsoft.com/powershell/](https://learn.microsoft.com/powershell/)
- [https://github.com/EvotecIT/Globalping](https://github.com/EvotecIT/Globalping)

## NOTES

### Note

Requests are executed from remote probes and may trigger security alerts on the target.
