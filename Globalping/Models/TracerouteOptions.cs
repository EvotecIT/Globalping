namespace Globalping;

public class TracerouteOptions : IMeasurementOptions {
    public int Port { get; set; } = 80;
    public string Protocol { get; set; } = "ICMP";
    public int IpVersion { get; set; } = 4;
}