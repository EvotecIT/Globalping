using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Globalping;

/// <summary>
/// Geographical regions used for locating probes.
/// </summary>
[JsonConverter(typeof(RegionNameConverter))]
public enum RegionName
{
    [EnumMember(Value = "Northern Africa")] NorthernAfrica,
    [EnumMember(Value = "Eastern Africa")] EasternAfrica,
    [EnumMember(Value = "Middle Africa")] MiddleAfrica,
    [EnumMember(Value = "Southern Africa")] SouthernAfrica,
    [EnumMember(Value = "Western Africa")] WesternAfrica,
    [EnumMember(Value = "Caribbean")] Caribbean,
    [EnumMember(Value = "Central America")] CentralAmerica,
    [EnumMember(Value = "South America")] SouthAmerica,
    [EnumMember(Value = "Northern America")] NorthernAmerica,
    [EnumMember(Value = "Central Asia")] CentralAsia,
    [EnumMember(Value = "Eastern Asia")] EasternAsia,
    [EnumMember(Value = "South-eastern Asia")] SouthEasternAsia,
    [EnumMember(Value = "Southern Asia")] SouthernAsia,
    [EnumMember(Value = "Western Asia")] WesternAsia,
    [EnumMember(Value = "Eastern Europe")] EasternEurope,
    [EnumMember(Value = "Northern Europe")] NorthernEurope,
    [EnumMember(Value = "Southern Europe")] SouthernEurope,
    [EnumMember(Value = "Western Europe")] WesternEurope,
    [EnumMember(Value = "Australia and New Zealand")] AustraliaAndNewZealand,
    [EnumMember(Value = "Melanesia")] Melanesia,
    [EnumMember(Value = "Micronesia")] Micronesia,
    [EnumMember(Value = "Polynesia")] Polynesia
}

internal sealed class RegionNameConverter : JsonConverter<RegionName>
{
    internal static readonly Dictionary<string, RegionName> _map = new(StringComparer.OrdinalIgnoreCase)
    {
        { "Northern Africa", RegionName.NorthernAfrica },
        { "Eastern Africa", RegionName.EasternAfrica },
        { "Middle Africa", RegionName.MiddleAfrica },
        { "Southern Africa", RegionName.SouthernAfrica },
        { "Western Africa", RegionName.WesternAfrica },
        { "Caribbean", RegionName.Caribbean },
        { "Central America", RegionName.CentralAmerica },
        { "South America", RegionName.SouthAmerica },
        { "Northern America", RegionName.NorthernAmerica },
        { "Central Asia", RegionName.CentralAsia },
        { "Eastern Asia", RegionName.EasternAsia },
        { "South-eastern Asia", RegionName.SouthEasternAsia },
        { "Southern Asia", RegionName.SouthernAsia },
        { "Western Asia", RegionName.WesternAsia },
        { "Eastern Europe", RegionName.EasternEurope },
        { "Northern Europe", RegionName.NorthernEurope },
        { "Southern Europe", RegionName.SouthernEurope },
        { "Western Europe", RegionName.WesternEurope },
        { "Australia and New Zealand", RegionName.AustraliaAndNewZealand },
        { "Melanesia", RegionName.Melanesia },
        { "Micronesia", RegionName.Micronesia },
        { "Polynesia", RegionName.Polynesia }
    };

    public override RegionName Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var value = reader.GetString();
        if (value != null && _map.TryGetValue(value, out var region))
        {
            return region;
        }
        throw new JsonException($"Invalid region name '{value}'");
    }

    public override void Write(Utf8JsonWriter writer, RegionName value, JsonSerializerOptions options)
    {
        foreach (var kv in _map)
        {
            if (kv.Value == value)
            {
                writer.WriteStringValue(kv.Key);
                return;
            }
        }
        throw new JsonException($"Invalid region enum value '{value}'");
    }
}

public static class RegionNameExtensions
{
    public static bool TryParse(string? value, out RegionName region)
    {
        if (value != null && RegionNameConverter._map.TryGetValue(value, out region))
        {
            return true;
        }
        region = default;
        return false;
    }

    public static string ToValueString(this RegionName region)
    {
        foreach (var kv in RegionNameConverter._map)
        {
            if (kv.Value == region)
            {
                return kv.Key;
            }
        }
        throw new ArgumentOutOfRangeException(nameof(region));
    }
}
