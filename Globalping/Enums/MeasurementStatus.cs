using System.Text.Json.Serialization;

namespace Globalping;

/// <summary>
/// Current processing state of a measurement.
/// </summary>
public enum MeasurementStatus
{
    /// <summary>Measurement is still running.</summary>
    InProgress,

    /// <summary>Measurement has completed.</summary>
    Finished
}
