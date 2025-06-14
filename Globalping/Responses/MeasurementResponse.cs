using System;
using System.Collections.Generic;

namespace Globalping;

public class MeasurementResponse {
    public string Id { get; set; } = string.Empty;
    public string? Type { get; set; }
    public string? Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public string Target { get; set; } = string.Empty;
    public int ProbesCount { get; set; }
    public MeasurementOptions? MeasurementOptions { get; set; }
    public List<Result>? Results { get; set; }
}