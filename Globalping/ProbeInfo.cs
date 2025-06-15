namespace Globalping;

/// <summary>
/// Provides a flattened view of probe information.
/// </summary>
public class ProbeInfo
{
    /// <summary>Continent where the probe is located.</summary>
    public string Continent { get; set; } = string.Empty;

    /// <summary>Region of the probe location.</summary>
    public string Region { get; set; } = string.Empty;

    /// <summary>Country ISO code.</summary>
    public string Country { get; set; } = string.Empty;

    /// <summary>Administrative subdivision.</summary>
    public string State { get; set; } = string.Empty;

    /// <summary>City name.</summary>
    public string City { get; set; } = string.Empty;

    /// <summary>Autonomous system number.</summary>
    public int Asn { get; set; }

    /// <summary>Probe longitude.</summary>
    public double Longitude { get; set; }

    /// <summary>Probe latitude.</summary>
    public double Latitude { get; set; }

    /// <summary>Network provider.</summary>
    public string Network { get; set; } = string.Empty;

    /// <summary>Tags associated with the probe. Can be a string, array or <c>null</c>.</summary>
    public object? Tags { get; set; }

    /// <summary>DNS resolvers used by the probe. Can be a string, array or <c>null</c>.</summary>
    public object? Resolvers { get; set; }

    /// <summary>Probe software version.</summary>
    public string Version { get; set; } = string.Empty;
}
