using System;
using System.Text.Json.Serialization;

namespace Globalping;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ContinentCode
{
    AF,
    AN,
    AS,
    EU,
    NA,
    OC,
    SA
}

public static class ContinentCodeExtensions
{
    public static bool TryParse(string? value, out ContinentCode code)
    {
        return Enum.TryParse(value, true, out code);
    }
}
