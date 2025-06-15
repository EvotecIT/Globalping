using System.Collections.Generic;

namespace Globalping;

/// <summary>
/// Extension helpers for <see cref="Probes"/>.
/// </summary>
public static class ProbeExtensions
{
    /// <summary>Converts a <see cref="Probes"/> object into <see cref="ProbeInfo"/>.</summary>
    /// <param name="probe">Raw probe information.</param>
    /// <returns>Flattened probe data.</returns>
    public static ProbeInfo ToProbeInfo(this Probes probe)
    {
        var loc = probe.Location;
        return new ProbeInfo
        {
            Continent = loc?.Continent ?? string.Empty,
            Region = loc?.Region ?? string.Empty,
            Country = loc?.Country ?? string.Empty,
            State = loc?.State ?? string.Empty,
            City = loc?.City ?? string.Empty,
            Asn = loc?.Asn ?? 0,
            Longitude = loc?.Longitude ?? 0,
            Latitude = loc?.Latitude ?? 0,
            Network = loc?.Network ?? string.Empty,
            Tags = ConvertList(probe.Tags),
            Resolvers = ConvertList(probe.Resolvers),
            Version = probe.Version
        };
    }

    private static object? ConvertList(List<string>? items)
    {
        if (items is null || items.Count == 0)
        {
            return null;
        }

        if (items.Count == 1)
        {
            return items[0];
        }

        return items.ToArray();
    }
}
