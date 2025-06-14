using System.Text.Json.Serialization;

namespace Globalping;


public class HttpOptions : IMeasurementOptions {
    [JsonPropertyName("request")]
    public HttpRequestOptions Request { get; set; } = new HttpRequestOptions();

    [JsonPropertyName("resolver")]
    public string? Resolver { get; set; }

    [JsonPropertyName("port")]
    public int Port { get; set; } = 80;

    [JsonPropertyName("protocol")]
    public HttpProtocol Protocol { get; set; } = HttpProtocol.Https;

    [JsonPropertyName("ipVersion")]
    public IpVersion IpVersion { get; set; } = IpVersion.Four;
}
