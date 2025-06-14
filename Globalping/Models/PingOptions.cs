namespace Globalping;

public class PingOptions : IMeasurementOptions {
    public int Packets { get; set; } = 3;
    public int IpVersion { get; set; } = 4;
}
