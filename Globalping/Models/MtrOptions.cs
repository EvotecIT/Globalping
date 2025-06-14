namespace Globalping;

public class MtrOptions : IMeasurementOptions
{
    public int Port { get; set; } = 80;
    public string Protocol { get; set; } = "ICMP";
    public int IpVersion { get; set; } = 4;
    public int Packets { get; set; } = 3;
}
