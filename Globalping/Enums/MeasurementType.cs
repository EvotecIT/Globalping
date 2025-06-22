using System.Text.Json.Serialization;
namespace Globalping;

/// <summary>
/// Supported measurement types that can be executed on a probe.
/// </summary>
public enum MeasurementType {
    /// <summary>ICMP ping measurement.</summary>
    Ping,

    /// <summary>Traceroute measurement.</summary>
    Traceroute,

    /// <summary>DNS lookup measurement.</summary>
    Dns,

    /// <summary>My traceroute measurement.</summary>
    Mtr,

    /// <summary>HTTP request measurement.</summary>
    Http
}
