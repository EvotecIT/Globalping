using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Globalization;

namespace Globalping;

public static class MeasurementResponseExtensions {
    public static IEnumerable<ResultSummary> GetSummaries(this MeasurementResponse response) {
        if (response.Results == null) {
            return Enumerable.Empty<ResultSummary>();
        }

        return response.Results.Select(r => new ResultSummary {
            Country = r.Probe.Country,
            City = r.Probe.City,
            State = r.Probe.State,
            Continent = r.Probe.Continent,
            Asn = r.Probe.Asn,
            Network = r.Probe.Network,
            ResolvedAddress = r.Data?.ResolvedAddress,
            ResolvedHostname = r.Data?.ResolvedHostname,
            Status = r.Data?.Status,
            Timings = r.Data?.Timings,
            Stats = r.Data?.Stats
        });
    }

    public static IEnumerable<PingTimingResult> GetPingTimings(this MeasurementResponse response) {
        if (response.Results == null) {
            return Enumerable.Empty<PingTimingResult>();
        }

        return response.Results.SelectMany(r =>
        {
            if (r.Data?.Timings is JsonElement element && element.ValueKind == JsonValueKind.Array)
            {
                return element.EnumerateArray().Select((t, idx) =>
                {
                    var timing = JsonSerializer.Deserialize<Timing>(t.GetRawText());
                    return new PingTimingResult
                    {
                        Target = response.Target,
                        Continent = r.Probe.Continent,
                        City = r.Probe.City,
                        Country = r.Probe.Country,
                        IcmpSequence = idx + 1,
                        TTL = timing?.Ttl ?? 0,
                        Time = timing?.Rtt ?? 0,
                        State = r.Probe.State,
                        Asn = r.Probe.Asn,
                        Network = r.Probe.Network,
                        ResolvedAddress = r.Data?.ResolvedAddress,
                        ResolvedHostname = r.Data?.ResolvedHostname,
                        Status = r.Data?.Status,
                    };
                });
            }

            return Enumerable.Empty<PingTimingResult>();
        });
    }

    public static IEnumerable<TracerouteHopResult> GetTracerouteHops(this MeasurementResponse response) {
        if (response.Results == null) {
            return Enumerable.Empty<TracerouteHopResult>();
        }

        return response.Results.SelectMany(r => ParseTraceroute(r.Data?.RawOutput).Select(h =>
        {
            h.Target = response.Target;
            h.Continent = r.Probe.Continent;
            h.City = r.Probe.City;
            h.Country = r.Probe.Country;
            h.State = r.Probe.State;
            h.Asn = r.Probe.Asn;
            h.Network = r.Probe.Network;
            h.ResolvedAddress = r.Data?.ResolvedAddress;
            h.ResolvedHostname = r.Data?.ResolvedHostname;
            h.Status = r.Data?.Status;
            return h;
        }));
    }

    public static IEnumerable<MtrHopResult> GetMtrHops(this MeasurementResponse response) {
        if (response.Results == null) {
            return Enumerable.Empty<MtrHopResult>();
        }

        return response.Results.SelectMany(r => ParseMtr(r.Data?.RawOutput).Select(h =>
        {
            h.Target = response.Target;
            h.Continent = r.Probe.Continent;
            h.City = r.Probe.City;
            h.Country = r.Probe.Country;
            h.State = r.Probe.State;
            h.ProbeAsn = r.Probe.Asn;
            h.Network = r.Probe.Network;
            h.ResolvedAddress = r.Data?.ResolvedAddress;
            h.ResolvedHostname = r.Data?.ResolvedHostname;
            h.Status = r.Data?.Status;
            return h;
        }));
    }

    public static IEnumerable<DnsRecordResult> GetDnsRecords(this MeasurementResponse response) {
        if (response.Results == null) {
            return Enumerable.Empty<DnsRecordResult>();
        }

        return response.Results.SelectMany(r => ParseDns(r.Data?.RawOutput).Select(h =>
        {
            h.Target = response.Target;
            h.Continent = r.Probe.Continent;
            h.City = r.Probe.City;
            h.Country = r.Probe.Country;
            h.State = r.Probe.State;
            h.Asn = r.Probe.Asn;
            h.Network = r.Probe.Network;
            h.ResolvedAddress = r.Data?.ResolvedAddress;
            h.ResolvedHostname = r.Data?.ResolvedHostname;
            h.Status = r.Data?.Status;
            return h;
        }));
    }

    private static IEnumerable<TracerouteHopResult> ParseTraceroute(string? raw) {
        if (string.IsNullOrWhiteSpace(raw)) {
            return Enumerable.Empty<TracerouteHopResult>();
        }

        var list = new List<TracerouteHopResult>();
        foreach (var line in raw.Split('\n')) {
            var t = line.Trim();
            if (string.IsNullOrEmpty(t) || t.StartsWith("traceroute")) {
                continue;
            }
            var match = Regex.Match(t, "^(\\d+)\\s+([^\\s]+)\\s+\\(([^\\)]+)\\)\\s+([0-9.]+)\\s+ms\\s+([0-9.]+)\\s+ms(?:\\s+([0-9.]+)\\s+ms)?");
            if (match.Success) {
                var hop = new TracerouteHopResult {
                    Hop = int.Parse(match.Groups[1].Value),
                    Host = match.Groups[2].Value,
                    IpAddress = match.Groups[3].Value
                };
                if (double.TryParse(match.Groups[4].Value, NumberStyles.Float, CultureInfo.InvariantCulture, out var r1)) {
                    hop.Time1 = r1;
                }
                if (double.TryParse(match.Groups[5].Value, NumberStyles.Float, CultureInfo.InvariantCulture, out var r2)) {
                    hop.Time2 = r2;
                }
                if (match.Groups[6].Success && double.TryParse(match.Groups[6].Value, NumberStyles.Float, CultureInfo.InvariantCulture, out var r3)) {
                    hop.Time3 = r3;
                }
                list.Add(hop);
            }
        }

        return list;
    }

    private static IEnumerable<MtrHopResult> ParseMtr(string? raw) {
        if (string.IsNullOrWhiteSpace(raw)) {
            return Enumerable.Empty<MtrHopResult>();
        }

        var list = new List<MtrHopResult>();
        var lines = raw.Split('\n');
        var start = false;
        foreach (var line in lines) {
            var t = line.Trim();
            if (string.IsNullOrEmpty(t)) {
                continue;
            }
            if (!start) {
                if (t.StartsWith("Host")) {
                    start = true;
                }
                continue;
            }

            if (t.Contains("waiting for reply")) {
                var numMatch = Regex.Match(t, "^(\\d+)");
                if (numMatch.Success) {
                    list.Add(new MtrHopResult { Hop = int.Parse(numMatch.Value), Host = "waiting for reply" });
                }
                continue;
            }

            var m = Regex.Match(t, "^(\\d+)\\.\\s+(AS[^\\s]+)\\s+([^\\(]+)\\s+\\(([^\\)]+)\\)\\s+([0-9.]+)%\\s+(\\d+)\\s+(\\d+)\\s+([0-9.]+)\\s+([0-9.]+)\\s+([0-9.]+)");
            if (m.Success) {
                var hop = new MtrHopResult {
                    Hop = int.Parse(m.Groups[1].Value),
                    Asn = m.Groups[2].Value,
                    Host = m.Groups[3].Value.Trim(),
                    IpAddress = m.Groups[4].Value,
                    LossPercent = double.Parse(m.Groups[5].Value, CultureInfo.InvariantCulture),
                    Drop = int.Parse(m.Groups[6].Value),
                    Rcv = int.Parse(m.Groups[7].Value),
                    Avg = double.Parse(m.Groups[8].Value, CultureInfo.InvariantCulture),
                    StDev = double.Parse(m.Groups[9].Value, CultureInfo.InvariantCulture),
                    Javg = double.Parse(m.Groups[10].Value, CultureInfo.InvariantCulture)
                };
                list.Add(hop);
            }
        }
        return list;
    }

    private static IEnumerable<DnsRecordResult> ParseDns(string? raw) {
        if (string.IsNullOrWhiteSpace(raw)) {
            return Enumerable.Empty<DnsRecordResult>();
        }

        var list = new List<DnsRecordResult>();
        var lines = raw.Split('\n');
        var inAnswer = false;
        foreach (var line in lines) {
            var t = line.Trim();
            if (t.StartsWith(";; ANSWER SECTION")) {
                inAnswer = true;
                continue;
            }
            if (inAnswer) {
                if (t.StartsWith(";;")) {
                    break;
                }
                var m = Regex.Match(t, "^(\\S+)\\.\\s+(\\d+)\\s+IN\\s+(\\S+)\\s+(.+)$");
                if (m.Success) {
                    var rec = new DnsRecordResult {
                        Name = m.Groups[1].Value,
                        Ttl = int.Parse(m.Groups[2].Value),
                        Type = m.Groups[3].Value,
                        Data = m.Groups[4].Value
                    };
                    list.Add(rec);
                }
            }
        }
        return list;
    }

    public static IEnumerable<HttpResponseResult> GetHttpResponses(this MeasurementResponse response) {
        if (response.Results == null) {
            return Enumerable.Empty<HttpResponseResult>();
        }

        return response.Results.SelectMany(r => ParseHttp(r.Data?.RawOutput).Select(h =>
        {
            h.Target = response.Target;
            h.Continent = r.Probe.Continent;
            h.City = r.Probe.City;
            h.Country = r.Probe.Country;
            h.State = r.Probe.State;
            h.Asn = r.Probe.Asn;
            h.Network = r.Probe.Network;
            h.ResolvedAddress = r.Data?.ResolvedAddress;
            h.ResolvedHostname = r.Data?.ResolvedHostname;
            h.Status = r.Data?.Status;
            return h;
        }));
    }

    private static IEnumerable<HttpResponseResult> ParseHttp(string? raw) {
        if (string.IsNullOrWhiteSpace(raw)) {
            return Enumerable.Empty<HttpResponseResult>();
        }

        var result = new HttpResponseResult();
        var lines = raw.Split('\n');
        var index = 0;
        if (lines.Length > 0) {
            var first = lines[0].Trim();
            var m = Regex.Match(first, "^(HTTP/\\S+)\\s+(\\d{3})\\s*(.*)$");
            if (m.Success) {
                result.Protocol = m.Groups[1].Value;
                result.StatusCode = int.Parse(m.Groups[2].Value);
                result.StatusDescription = m.Groups[3].Value.Trim();
            }
            index = 1;
        }

        for (; index < lines.Length; index++) {
            var line = lines[index].TrimEnd();
            if (string.IsNullOrEmpty(line)) {
                index++;
                break;
            }

            var sep = line.IndexOf(':');
            if (sep > 0) {
                var key = line.Substring(0, sep).Trim();
                var value = line.Substring(sep + 1).Trim();
                result.Headers[key] = value;
            }
        }

        if (index < lines.Length) {
            result.Body = string.Join("\n", lines.Skip(index));
        }

        return new[] { result };
    }
}
