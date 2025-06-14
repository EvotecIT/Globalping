namespace Globalping;

public class MeasurementRequest {
    public MeasurementType Type { get; set; }
    public string Target { get; set; }
    public bool InProgressUpdates { get; set; } = false;
    public object Locations { get; set; } // Keep as object to support various formats
    public int Limit { get; set; } = 1;
    public IMeasurementOptions MeasurementOptions { get; set; }
}
