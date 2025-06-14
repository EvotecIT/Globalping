using System;
using System.Text.Json.Serialization;

namespace Globalping;
public class Timing {
    [JsonPropertyName("ttl")]
    public int Ttl { get; set; }

    [JsonPropertyName("rtt")]
    public double Rtt { get; set; }
}
