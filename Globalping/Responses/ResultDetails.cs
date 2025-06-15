using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Globalping;

public class ResultDetails {
    [JsonPropertyName("status")]
    public string? Status { get; set; }

    [JsonPropertyName("rawOutput")]
    public string? RawOutput { get; set; }

    [JsonPropertyName("resolvedAddress")]
    public string? ResolvedAddress { get; set; }

    [JsonPropertyName("resolvedHostname")]
    public string? ResolvedHostname { get; set; }

    [JsonPropertyName("timings")]
    public JsonElement? Timings { get; set; }

    [JsonPropertyName("stats")]
    public Stats? Stats { get; set; }

    [JsonPropertyName("hops")]
    public JsonElement? Hops { get; set; }

    [JsonPropertyName("answers")]
    public List<DnsAnswer>? Answers { get; set; }

    [JsonPropertyName("resolver")]
    public string? Resolver { get; set; }

    [JsonPropertyName("tls")]
    public TlsCertificate? Tls { get; set; }

    [JsonPropertyName("rawHeaders")]
    public string? RawHeaders { get; set; }

    [JsonPropertyName("headers")]
    public Dictionary<string, JsonElement>? Headers { get; set; }

    [JsonPropertyName("rawBody")]
    public string? RawBody { get; set; }

    [JsonPropertyName("truncated")]
    public bool? Truncated { get; set; }

    [JsonPropertyName("statusCode")]
    public int? StatusCode { get; set; }

    [JsonPropertyName("statusCodeName")]
    public string? StatusCodeName { get; set; }
}

