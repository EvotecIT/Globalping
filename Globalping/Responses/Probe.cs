using System.Collections.Generic;

namespace Globalping;

public class Probe {
    public string Continent { get; set; } = string.Empty;
    public string Region { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public int Asn { get; set; }
    public double Longitude { get; set; }
    public double Latitude { get; set; }
    public string Network { get; set; } = string.Empty;
    public List<string> Tags { get; set; } = new();
    public List<string> Resolvers { get; set; } = new();
}
