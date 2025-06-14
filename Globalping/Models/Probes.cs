using System.Collections.Generic;

namespace Globalping;

public class Probes {
    public string Version { get; set; } = string.Empty;
    public Location? Location { get; set; }
    public List<string>? Tags { get; set; }
    public List<string>? Resolvers { get; set; }
}
