using System.Collections.Generic;

namespace Globalping;

public class MeasurementRequest {
    public MeasurementType Type { get; set; }
    public string Target { get; set; } = string.Empty;
    public bool InProgressUpdates { get; set; } = false;
    public List<LocationRequest>? Locations { get; set; }
    public int Limit { get; set; } = 1;
    public IMeasurementOptions? MeasurementOptions { get; set; }
}

