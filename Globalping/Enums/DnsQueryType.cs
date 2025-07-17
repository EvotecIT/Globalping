using System.Text.Json.Serialization;

namespace Globalping;

/// <summary>
/// Common DNS record types that can be queried.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum DnsQueryType
{
    /// <summary>IPv4 host address record.</summary>
    A,

    /// <summary>IPv6 host address record.</summary>
    AAAA,

    /// <summary>Wildcard query for all records.</summary>
    ANY,

    /// <summary>Canonical name record.</summary>
    CNAME,

    /// <summary>DNSKEY record.</summary>
    DNSKEY,

    /// <summary>Delegation signer record.</summary>
    DS,

    /// <summary>HTTPS service binding.</summary>
    HTTPS,

    /// <summary>Mail exchanger record.</summary>
    MX,

    /// <summary>Name server record.</summary>
    NS,

    /// <summary>NSEC record.</summary>
    NSEC,

    /// <summary>Pointer record.</summary>
    PTR,

    /// <summary>Resource record signature.</summary>
    RRSIG,

    /// <summary>Start of authority record.</summary>
    SOA,

    /// <summary>Text record.</summary>
    TXT,

    /// <summary>Service locator.</summary>
    SRV,

    /// <summary>Service binding.</summary>
    SVCB
}