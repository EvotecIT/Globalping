namespace Globalping;

public class ResultDetails {
    public string Status { get; set; }
    public string RawOutput { get; set; }
    public string ResolvedAddress { get; set; }
    public string ResolvedHostname { get; set; }
    public List<Timing> Timings { get; set; }
    public Stats Stats { get; set; }
}
