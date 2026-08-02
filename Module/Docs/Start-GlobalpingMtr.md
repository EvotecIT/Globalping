---
external help file: Globalping-help.xml
Module Name: Globalping
online version: https://github.com/EvotecIT/Globalping
schema: 2.0.0
---
# Start-GlobalpingMtr
## SYNOPSIS
Start an MTR (My Traceroute) measurement using Globalping.

## SYNTAX
### __AllParameterSets
```powershell
Start-GlobalpingMtr -Target <string[]> [-Raw] [-Classic] [-Options <MtrOptions>] [-Locations <LocationRequest[]>] [-ReuseLocationsFromId <string>] [-SimpleLocations <string[]>] [-Limit <int>] [-InProgressUpdates] [-WaitTime <int>] [-ApiKey <string>] [<CommonParameters>]
```

## DESCRIPTION
Combines ping and traceroute information from multiple probes.

## EXAMPLES

### EXAMPLE 1
```powershell
PS> Start-GlobalpingMtr -Target "evotec.xyz"
```

Produces hop statistics for the target.

### EXAMPLE 2
```powershell
PS> Start-GlobalpingMtr -Target "example.com" -Options @{ Packets = 3 }
```

Configures the probe to send three packets per hop.

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
The text output mirrors the behaviour of the MTR command line tool.

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
Use to configure packet count or other low level parameters.

```yaml
Type: MtrOptions
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
Useful when converting the result to custom objects.

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

- `Globalping.MtrHopResult`
- `System.String`
- `Globalping.MeasurementResponse`

## RELATED LINKS

- [https://learn.microsoft.com/powershell/](https://learn.microsoft.com/powershell/)
- [https://github.com/EvotecIT/Globalping](https://github.com/EvotecIT/Globalping)

## NOTES

### Note

Some paths may change during the test, affecting hop statistics.
