namespace Globalping;

public class MeasurementResponse {
    public string Id { get; set; }
    public string Type { get; set; }
    public string Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public string Target { get; set; }
    public int ProbesCount { get; set; }
    public MeasurementOptions MeasurementOptions { get; set; } // Assuming this matches the existing MeasurementOptions class
    public List<Result> Results { get; set; }
}