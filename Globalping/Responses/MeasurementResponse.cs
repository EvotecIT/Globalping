using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Globalping;

public class MeasurementResponse {
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("type")]
    public string? Type { get; set; }

    [JsonPropertyName("status")]
    public MeasurementStatus Status { get; set; }

    [JsonPropertyName("createdAt")]
    public DateTime CreatedAt { get; set; }

    [JsonPropertyName("updatedAt")]
    public DateTime UpdatedAt { get; set; }

    [JsonPropertyName("target")]
    public string Target { get; set; } = string.Empty;

    [JsonPropertyName("probesCount")]
    public int ProbesCount { get; set; }

    [JsonPropertyName("measurementOptions")]
    public MeasurementOptions? MeasurementOptions { get; set; }

    [JsonPropertyName("locations")]
    public List<LocationRequest>? Locations { get; set; }

    [JsonPropertyName("limit")]
    public int? Limit { get; set; }

    [JsonPropertyName("results")]
    public List<Result>? Results { get; set; }
}