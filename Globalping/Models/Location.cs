namespace Globalping;

public class Location {
    public string Continent { get; set; } = string.Empty;
    public string Region { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public int Asn { get; set; }
    public string Network { get; set; } = string.Empty;
    public double Latitude { get; set; }
    public double Longitude { get; set; }
}

