using System.Text.Json.Serialization;

namespace Globalping;

/// <summary>
/// Current processing state of a measurement.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum MeasurementStatus
{
    /// <summary>Measurement is still running.</summary>
    InProgress,

    /// <summary>Measurement has completed.</summary>
    Finished
}
