using System.Dynamic;
using Globalping;


Console.WriteLine("Hello, World!");



var probeService = new ProbeService(new HttpClient());
var probes = await probeService.GetOnlineProbesAsync();
Console.WriteLine(probes);

var measurementRequestCountry = new MeasurementRequest {
    Type = MeasurementType.Ping,
    Target = "cdn.jsdelivr.net",
    Locations = new List<LocationRequest>
    {
        new LocationRequest { Country = "DE" },
        new LocationRequest { Country = "PL" }
    }
};


var measurementRequestCountryAndLimit = new MeasurementRequest {
    Type = MeasurementType.Ping,
    Target = "cdn.jsdelivr.net",
    Locations = new List<LocationRequest>
    {
        new LocationRequest { Country = "DE", Limit = 4 },
        new LocationRequest { Country = "PL", Limit = 2 }
    }
};

var measurementRequestMagic = new MeasurementRequest {
    Type = MeasurementType.Ping,
    Target = "cdn.jsdelivr.net",
    Locations = new List<LocationRequest>
    {
        new LocationRequest { Magic = "FR" },
        // Add other "magic" locations...
    }
};


var measurementRequestCustomOptions = new MeasurementRequest {
    Type = MeasurementType.Ping,
    Target = "cdn.jsdelivr.net",
    MeasurementOptions = new PingOptions {
        Packets = 6
    }
};


var measurementRequestPreviousManagementId = new MeasurementRequest {
    Type = MeasurementType.Ping,
    Target = "cdn.jsdelivr.net",
    Locations = "1wzMrzLBZfaPoT1c" // Previous measurement ID
};
