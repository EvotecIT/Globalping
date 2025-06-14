using System.Collections.Generic;

namespace Globalping;

public class Probe {
    public string Continent { get; set; }
    public string Region { get; set; }
    public string Country { get; set; }
    public string State { get; set; }
    public string City { get; set; }
    public int Asn { get; set; }
    public double Longitude { get; set; }
    public double Latitude { get; set; }
    public string Network { get; set; }
    public List<string> Tags { get; set; }
    public List<string> Resolvers { get; set; }
}
