using System.Text.Json.Serialization;

namespace Globalping;

/// <summary>
/// Options that configure an HTTP measurement.
/// </summary>
public class HttpOptions : IMeasurementOptions {
    /// <summary>Details of the HTTP request to perform.</summary>
    [JsonPropertyName("request")]
    public HttpRequestOptions Request { get; set; } = new HttpRequestOptions();

    /// <summary>Optional resolver to use for DNS lookups.</summary>
    [JsonPropertyName("resolver")]
    public string? Resolver { get; set; }

    /// <summary>Target port for the request.</summary>
    [JsonPropertyName("port")]
    public int Port { get; set; } = 80;

    /// <summary>HTTP protocol version to use.</summary>
    [JsonPropertyName("protocol")]
    public HttpProtocol Protocol { get; set; } = HttpProtocol.HTTPS;

    /// <summary>Preferred IP version when resolving the target.</summary>
    [JsonPropertyName("ipVersion")]
    public IpVersion IpVersion { get; set; } = IpVersion.Four;
}
