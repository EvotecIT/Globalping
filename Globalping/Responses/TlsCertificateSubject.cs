using System.Text.Json.Serialization;
namespace Globalping;

/// <summary>Subject information for a TLS certificate.</summary>
public class TlsCertificateSubject
{
    [JsonPropertyName("CN")]
    public string? CN { get; set; }

    [JsonPropertyName("alt")]
    public string? Alt { get; set; }
}
