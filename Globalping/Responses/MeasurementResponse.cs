namespace Globalping.Models {
    public class MeasurementResponse {
        public string Id { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string Target { get; set; }
        public int ProbesCount { get; set; }
        public MeasurementOptions MeasurementOptions { get; set; } // Assuming this matches the existing MeasurementOptions class
        public List<Result> Results { get; set; }
    }

    public class Result {
        public Probe Probe { get; set; }
        public ResultDetails MeasurementResult { get; set; }
    }


    public class Timing {
        public int Ttl { get; set; }
        public double Rtt { get; set; }
    }

    public class ResultDetails {
        public string Status { get; set; }
        public string RawOutput { get; set; }
        public string ResolvedAddress { get; set; }
        public string ResolvedHostname { get; set; }
        public List<Timing> Timings { get; set; }
        public Stats Stats { get; set; }
    }

    public class Stats {
        public double Min { get; set; }
        public double Max { get; set; }
        public double Avg { get; set; }
        public int Total { get; set; }
        public int Loss { get; set; }
        public int Rcv { get; set; }
        public int Drop { get; set; }
    }
}
