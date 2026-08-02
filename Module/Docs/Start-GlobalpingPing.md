---
external help file: Globalping-help.xml
Module Name: Globalping
online version: https://github.com/EvotecIT/Globalping
schema: 2.0.0
---
# Start-GlobalpingPing
## SYNOPSIS
Start a ping measurement using Globalping.

## SYNTAX
### __AllParameterSets
```powershell
Start-GlobalpingPing -Target <string[]> [-Raw] [-Classic] [-Options <PingOptions>] [-Locations <LocationRequest[]>] [-ReuseLocationsFromId <string>] [-SimpleLocations <string[]>] [-Limit <int>] [-InProgressUpdates] [-WaitTime <int>] [-ApiKey <string>] [<CommonParameters>]
```

## DESCRIPTION
Instructs remote probes to send ICMP echo requests to the specified target.

The cmdlet outputs PingTimingResult objects, raw results or classic text depending on the selected parameters.

## EXAMPLES

### EXAMPLE 1
```powershell
PS> Start-GlobalpingPing -Target "evotec.xyz" -SimpleLocations "DE", "US"
```

Runs ping from probes in Germany and the United States.

### EXAMPLE 2
```powershell
PS> Start-GlobalpingPing -Target "example.com" -SimpleLocations "PL" -Options @{ Packets = 5 }
```

Uses PingOptions to set the packet count.

### EXAMPLE 3
```powershell
PS> Start-GlobalpingPing -Target "example.com" -Classic
```

Displays the raw ping output returned by the probe.

## PARAMETERS

### -ApiKey
{{ Fill ApiKey Description }}

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
Each probe returns the textual output of its ping utility.

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
{{ Fill InProgressUpdates Description }}

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
{{ Fill Limit Description }}

```yaml
Type: Nullable`1
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
{{ Fill Locations Description }}

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
Use this to configure packet count or other low level settings.

```yaml
Type: PingOptions
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
When set the cmdlet outputs the MeasurementResponse object returned by the API.

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
{{ Fill ReuseLocationsFromId Description }}

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
{{ Fill SimpleLocations Description }}

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
{{ Fill Target Description }}

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
{{ Fill WaitTime Description }}

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

- `Globalping.PingTimingResult`
- `System.String`
- `Globalping.MeasurementResponse`

## RELATED LINKS

- [https://learn.microsoft.com/powershell/](https://learn.microsoft.com/powershell/)
- [https://github.com/EvotecIT/Globalping](https://github.com/EvotecIT/Globalping)

## NOTES

### Note

Some networks block ICMP traffic which may affect results.
