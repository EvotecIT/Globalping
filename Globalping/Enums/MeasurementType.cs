using System.Text.Json.Serialization;
namespace Globalping;

public enum MeasurementType {
    Ping,
    Traceroute,
    Dns,
    Mtr,
    Http
}
