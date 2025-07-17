using System.Text.Json.Serialization;

namespace Globalping;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum TestStatus
{
    InProgress,
    Finished,
    Failed,
    Offline
}
