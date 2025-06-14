using HttpRequest = System.Net.Http.HttpRequestMessage;
using System.Text.Json.Serialization;

namespace Globalping;


public class HttpOptions : IMeasurementOptions {
    [JsonPropertyName("request")]
    public HttpRequest Request { get; set; } = new HttpRequest();

    [JsonPropertyName("resolver")]
    public string? Resolver { get; set; }

    [JsonPropertyName("port")]
    public int Port { get; set; } = 80;

    [JsonPropertyName("protocol")]
    public string Protocol { get; set; } = "HTTPS";

    [JsonPropertyName("ipVersion")]
    public int IpVersion { get; set; } = 4;
}
