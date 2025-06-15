using System.Text.Json.Serialization;
namespace Globalping;
public class TlsCertificateSubject
{
    [JsonPropertyName("CN")]
    public string? CN { get; set; }

    [JsonPropertyName("alt")]
    public string? Alt { get; set; }
}
