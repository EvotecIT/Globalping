using System.Text.Json.Serialization;

namespace Globalping;

/// <summary>
/// Public key algorithms used by TLS certificates.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum TlsKeyType
{
    /// <summary>RSA key.</summary>
    RSA,

    /// <summary>Elliptic curve key.</summary>
    EC
}
