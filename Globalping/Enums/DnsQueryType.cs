using System.Text.Json.Serialization;
namespace Globalping;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum DnsQueryType
{
    A,
    AAAA,
    ANY,
    CNAME,
    DNSKEY,
    DS,
    HTTPS,
    MX,
    NS,
    NSEC,
    PTR,
    RRSIG,
    SOA,
    TXT,
    SRV
}