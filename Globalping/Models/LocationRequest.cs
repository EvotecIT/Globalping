namespace Globalping;

public class LocationRequest {
    public string Country { get; set; }
    public int? Limit { get; set; } // Optional limit for probes
    public string Magic { get; set; } // For "magic" location requests
}