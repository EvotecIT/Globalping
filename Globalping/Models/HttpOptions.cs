using HttpRequest = System.Net.Http.HttpRequestMessage;

namespace Globalping;


public class HttpOptions : IMeasurementOptions {
    public HttpRequest Request { get; set; } = new HttpRequest();
    public string? Resolver { get; set; }
    public int Port { get; set; } = 80;
    public string Protocol { get; set; } = "HTTPS";
    public int IpVersion { get; set; } = 4;
}
