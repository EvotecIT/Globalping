using System;
using System.Text.Json.Serialization;
namespace Globalping;
public class TlsCertificate
{
    [JsonPropertyName("protocol")]
    public string Protocol { get; set; } = string.Empty;

    [JsonPropertyName("cipherName")]
    public string CipherName { get; set; } = string.Empty;

    [JsonPropertyName("authorized")]
    public bool Authorized { get; set; }

    [JsonPropertyName("error")]
    public string? Error { get; set; }

    [JsonPropertyName("createdAt")]
    public DateTime CreatedAt { get; set; }

    [JsonPropertyName("expiresAt")]
    public DateTime ExpiresAt { get; set; }

    [JsonPropertyName("subject")]
    public TlsCertificateSubject Subject { get; set; } = new();

    [JsonPropertyName("issuer")]
    public TlsCertificateIssuer Issuer { get; set; } = new();

    [JsonPropertyName("keyType")]
    public TlsKeyType? KeyType { get; set; }

    [JsonPropertyName("keyBits")]
    public double? KeyBits { get; set; }

    [JsonPropertyName("serialNumber")]
    public string SerialNumber { get; set; } = string.Empty;

    [JsonPropertyName("fingerprint256")]
    public string Fingerprint256 { get; set; } = string.Empty;

    [JsonPropertyName("publicKey")]
    public string? PublicKey { get; set; }
}
