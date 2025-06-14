namespace Globalping;

public class DnsRecordResult
{
    public string Target { get; set; } = string.Empty;
    public string Flags { get; set; } = string.Empty;
    public string QuestionName { get; set; } = string.Empty;
    public string QuestionType { get; set; } = string.Empty;
    public int QueryCount { get; set; }
    public int AnswerCount { get; set; }
    public int AuthorityCount { get; set; }
    public int AdditionalCount { get; set; }
    public string Opcode { get; set; } = string.Empty;
    public string HeaderStatus { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public int Ttl { get; set; }
    public string Type { get; set; } = string.Empty;
    public string Data { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string Network { get; set; } = string.Empty;
    public int Asn { get; set; }
    public string State { get; set; } = string.Empty;
    public string Continent { get; set; } = string.Empty;
    public string? Status { get; set; }
}
