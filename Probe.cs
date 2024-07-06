namespace Globalping.Models {
    public class Probe {
        public string Version { get; set; }
        public Location Location { get; set; }
        public List<string> Tags { get; set; }
        public List<string> Resolvers { get; set; }
    }

    public class Location {
        public string Continent { get; set; }
        public string Region { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public int Asn { get; set; }
        public string Network { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
