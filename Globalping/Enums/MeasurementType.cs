using System.Text.Json.Serialization;
namespace Globalping;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum MeasurementType {
    Ping,
    Traceroute,
    Dns,
    Mtr,
    Http
}
