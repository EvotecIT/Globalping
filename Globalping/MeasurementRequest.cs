namespace Globalping;

public enum MeasurementType {
    Ping,
    Traceroute,
    Dns,
    Mtr,
    Http
}


public class MeasurementRequest {
    public MeasurementType Type { get; set; }
    public string Target { get; set; }
    public bool InProgressUpdates { get; set; } = false;
    public object Locations { get; set; } // Keep as object to support various formats
    public int Limit { get; set; } = 1;
    public IMeasurementOptions MeasurementOptions { get; set; }
}

public class LocationRequest {
    public string Country { get; set; }
    public int? Limit { get; set; } // Optional limit for probes
    public string Magic { get; set; } // For "magic" location requests
}

public class MeasurementOptions {
    public int? Packets { get; set; } // Example custom option
}

public interface IMeasurementOptions { }

public class PingOptions : IMeasurementOptions {
    public int Packets { get; set; } = 3;
    public int IpVersion { get; set; } = 4;
}

public class TracerouteOptions : IMeasurementOptions {
    public int Port { get; set; } = 80;
    public string Protocol { get; set; } = "ICMP";
    public int IpVersion { get; set; } = 4;
}

public class DnsOptions : IMeasurementOptions {
    public DnsQuery Query { get; set; } = new DnsQuery();
    public string Resolver { get; set; } // Can be IPv4, IPv6, or hostname
    public int Port { get; set; } = 53;
    public string Protocol { get; set; } = "UDP";
    public int IpVersion { get; set; } = 4;
    public bool Trace { get; set; } = false;
}

public enum DnsQueryType {
    A,
    AAAA,
    ANY,
    CNAME,
    DNSKEY,
    DS,
    HTTPS,
    MX,
    NS,
    NSEC,
    PTR,
    RRSIG,
    SOA,
    TXT,
    SRV
}


public class MtrOptions : IMeasurementOptions {
    public int Port { get; set; } = 80;
    public string Protocol { get; set; } = "ICMP";
    public int IpVersion { get; set; } = 4;
    public int Packets { get; set; } = 3;
}

public class DnsQuery {
    public DnsQueryType Type { get; set; } = DnsQueryType.A;
}

public class HttpOptions : IMeasurementOptions {
    public HttpRequest Request { get; set; }
    public string Resolver { get; set; }
    public int Port { get; set; } = 80;
    public string Protocol { get; set; } = "HTTPS";
    public int IpVersion { get; set; } = 4;
}

public class HttpRequest {
    // Define properties for the HTTP request here
    // This is a placeholder for the actual request properties you need
}