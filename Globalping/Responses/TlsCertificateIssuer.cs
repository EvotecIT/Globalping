using System.Text.Json.Serialization;
namespace Globalping;

/// <summary>Issuer information for a TLS certificate.</summary>
public class TlsCertificateIssuer
{
    [JsonPropertyName("C")]
    public string? C { get; set; }

    [JsonPropertyName("O")]
    public string? O { get; set; }

    [JsonPropertyName("CN")]
    public string? CN { get; set; }
}
