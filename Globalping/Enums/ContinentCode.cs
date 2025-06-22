using System;
using System.Text.Json.Serialization;

namespace Globalping;

/// <summary>
/// ISO continent codes used in location information.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ContinentCode
{
    /// <summary>Africa.</summary>
    AF,

    /// <summary>Antarctica.</summary>
    AN,

    /// <summary>Asia.</summary>
    AS,

    /// <summary>Europe.</summary>
    EU,

    /// <summary>North America.</summary>
    NA,

    /// <summary>Oceania.</summary>
    OC,

    /// <summary>South America.</summary>
    SA
}

public static class ContinentCodeExtensions
{
    public static bool TryParse(string? value, out ContinentCode code)
    {
        return Enum.TryParse(value, true, out code);
    }
}
