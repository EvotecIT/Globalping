using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Globalization;

namespace Globalping;

public static class MeasurementResponseExtensions {
    internal struct DnsHeaderInfo {
        public string Flags;
        public string QuestionName;
        public string QuestionType;
        public int QueryCount;
        public int AnswerCount;
        public int AuthorityCount;
        public int AdditionalCount;
        public string Opcode;
        public string HeaderStatus;
    }

    private static DnsHeaderInfo ParseDnsHeaderInfo(string? raw) {
        var info = new DnsHeaderInfo();
        if (string.IsNullOrWhiteSpace(raw)) {
            return info;
        }

        var lines = raw!.Split('\n');
        for (var i = 0; i < lines.Length; i++) {
            var t = lines[i].Trim();
            if (t.StartsWith(";; ->>HEADER<<-")) {
                var m = Regex.Match(t, @"opcode:\s*(\S+),\s*status:\s*(\S+),\s*id:\s*(\d+)");
                if (m.Success) {
                    info.Opcode = m.Groups[1].Value;
                    info.HeaderStatus = m.Groups[2].Value;
                }
                continue;
            }
            if (t.StartsWith(";; flags:")) {
                var m = Regex.Match(t, @"flags:\s*([^;]+);\s*QUERY:\s*(\d+),\s*ANSWER:\s*(\d+),\s*AUTHORITY:\s*(\d+),\s*ADDITIONAL:\s*(\d+)");
                if (m.Success) {
                    info.Flags = m.Groups[1].Value.Trim();
                    info.QueryCount = int.Parse(m.Groups[2].Value);
                    info.AnswerCount = int.Parse(m.Groups[3].Value);
                    info.AuthorityCount = int.Parse(m.Groups[4].Value);
                    info.AdditionalCount = int.Parse(m.Groups[5].Value);
                }
                continue;
            }
            if (t.StartsWith(";; QUESTION SECTION")) {
                if (i + 1 < lines.Length) {
                    var q = lines[i + 1].Trim(';').Trim();
                    var qm = Regex.Match(q, @"^(\S+)\.\s+IN\s+(\S+)");
                    if (qm.Success) {
                        info.QuestionName = qm.Groups[1].Value;
                        info.QuestionType = qm.Groups[2].Value;
                    }
                }
                break;
            }
        }

        return info;
    }

    private static Dictionary<int, string> ParseMtrRawHosts(string? raw) {
        var dict = new Dictionary<int, string>();
        if (string.IsNullOrWhiteSpace(raw)) {
            return dict;
        }

        var lines = raw!.Split('\n');
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
                    dict[int.Parse(numMatch.Value)] = "waiting for reply";
                }
                continue;
            }

            var m = Regex.Match(t, "^(\\d+)\\.\\s+(AS[^\\s]+)\\s+([^\\(]+)\\s+\\(([^\\)]+)\\)");
            if (m.Success) {
                var hop = int.Parse(m.Groups[1].Value);
                var host = m.Groups[3].Value.Trim();
                dict[hop] = host;
            }
        }

        return dict;
    }
    public static List<ResultSummary> GetSummaries(this MeasurementResponse response) {
        if (response.Results == null) {
            return new List<ResultSummary>();
        }

        return response.Results.Select(r => new ResultSummary {
            Country = r.Probe.Country,
            City = r.Probe.City,
            State = r.Probe.State,
            Continent = r.Probe.Continent,
            Asn = r.Probe.Asn,
            Network = r.Probe.Network,
            Version = r.Probe.Version,
            ResolvedAddress = r.Data?.ResolvedAddress,
            ResolvedHostname = r.Data?.ResolvedHostname,
            Status = r.Data != null ? r.Data.Status : default,
            Timings = r.Data?.Timings,
            Stats = r.Data?.Stats
        }).ToList();
    }

    public static List<PingTimingResult> GetPingTimings(this MeasurementResponse response) {
        if (response.Results == null) {
            return new List<PingTimingResult>();
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
                        Version = r.Probe.Version,
                        ResolvedAddress = r.Data?.ResolvedAddress,
                        ResolvedHostname = r.Data?.ResolvedHostname,
                        Status = r.Data != null ? r.Data.Status : default,
                    };
                }).ToList();
            }

            return new List<PingTimingResult>();
        }).ToList();
    }

    public static List<TracerouteHopResult> GetTracerouteHops(this MeasurementResponse response) {
        if (response.Results == null) {
            return new List<TracerouteHopResult>();
        }

        return response.Results.SelectMany(r => ParseTraceroute(r.Data).Select(h =>
        {
            h.Target = response.Target;
            h.Continent = r.Probe.Continent;
            h.City = r.Probe.City;
            h.Country = r.Probe.Country;
            h.State = r.Probe.State;
            h.Asn = r.Probe.Asn;
            h.Network = r.Probe.Network;
            h.Version = r.Probe.Version;
            h.ResolvedAddress = r.Data?.ResolvedAddress;
            h.ResolvedHostname = r.Data?.ResolvedHostname;
            h.Status = r.Data != null ? r.Data.Status : default;
            return h;
        })).ToList();
    }

    public static List<MtrHopResult> GetMtrHops(this MeasurementResponse response) {
        if (response.Results == null) {
            return new List<MtrHopResult>();
        }

        return response.Results.SelectMany(r => ParseMtr(r.Data).Select(h =>
        {
            h.Target = response.Target;
            h.Continent = r.Probe.Continent;
            h.City = r.Probe.City;
            h.Country = r.Probe.Country;
            h.State = r.Probe.State;
            h.ProbeAsn = r.Probe.Asn;
            h.Network = r.Probe.Network;
            h.Version = r.Probe.Version;
            h.ResolvedAddress = r.Data?.ResolvedAddress;
            h.ResolvedHostname = r.Data?.ResolvedHostname;
            h.Status = r.Data != null ? r.Data.Status : default;
            return h;
        })).ToList();
    }

    public static List<DnsRecordResult> GetDnsRecords(this MeasurementResponse response) {
        if (response.Results == null) {
            return new List<DnsRecordResult>();
        }

        return response.Results.SelectMany(r => ParseDns(r.Data).Select(h =>
        {
            h.Target = response.Target;
            h.Continent = r.Probe.Continent;
            h.City = r.Probe.City;
            h.Country = r.Probe.Country;
            h.State = r.Probe.State;
            h.Asn = r.Probe.Asn;
            h.Network = r.Probe.Network;
            h.Version = r.Probe.Version;
            h.Status = r.Data != null ? r.Data.Status : default;
            return h;
        })).ToList();
    }

    internal static List<TracerouteHopResult> ParseTraceroute(ResultDetails? data) {
        if (data?.Hops is JsonElement hops && hops.ValueKind == JsonValueKind.Array) {
            var list = new List<TracerouteHopResult>();
            var idx = 1;
            foreach (var hop in hops.EnumerateArray()) {
                var r = new TracerouteHopResult {
                    Hop = idx++,
                    Host = hop.GetProperty("resolvedHostname").GetString() ?? string.Empty,
                    IpAddress = hop.GetProperty("resolvedAddress").GetString() ?? string.Empty
                };
                if (hop.TryGetProperty("timings", out var timings) && timings.ValueKind == JsonValueKind.Array) {
                    var arr = timings.EnumerateArray().Select(e => e.GetProperty("rtt").GetDouble()).ToList();
                    if (arr.Count > 0) r.Time1 = arr[0];
                    if (arr.Count > 1) r.Time2 = arr[1];
                    if (arr.Count > 2) r.Time3 = arr[2];
                }
                list.Add(r);
            }
            return list;
        }

        return ParseTracerouteRaw(data?.RawOutput);
    }

    internal static List<TracerouteHopResult> ParseTracerouteRaw(string? raw) {
        if (string.IsNullOrWhiteSpace(raw)) {
            return new List<TracerouteHopResult>();
        }

        var list = new List<TracerouteHopResult>();
        foreach (var line in raw!.Split('\n')) {
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

    internal static List<MtrHopResult> ParseMtr(ResultDetails? data) {
        if (data?.Hops is JsonElement hops && hops.ValueKind == JsonValueKind.Array) {
            var rawHosts = ParseMtrRawHosts(data.RawOutput);
            var jsonList = new List<MtrHopResult>();
            var idx = 1;
            foreach (var hop in hops.EnumerateArray()) {
                var r = new MtrHopResult {
                    Hop = idx++,
                    Host = hop.GetProperty("resolvedHostname").GetString() ?? string.Empty,
                    IpAddress = hop.GetProperty("resolvedAddress").GetString() ?? string.Empty,
                };
                if (rawHosts.TryGetValue(r.Hop, out var rawHost)) {
                    if (!string.IsNullOrWhiteSpace(rawHost) && rawHost != r.IpAddress) {
                        r.Host = rawHost;
                    }
                }
                if (hop.TryGetProperty("asn", out var asnEl)) {
                    if (asnEl.ValueKind == JsonValueKind.Array) {
                        var asns = asnEl.EnumerateArray()
                            .Where(a => a.ValueKind == JsonValueKind.Number)
                            .Select(a => a.GetInt32())
                            .ToList();
                        r.Asn = asns.Count switch {
                            0 => null,
                            1 => (object)asns[0],
                            _ => asns
                        };
                    } else if (asnEl.ValueKind == JsonValueKind.Number) {
                        r.Asn = asnEl.GetInt32();
                    }
                }
                if (hop.TryGetProperty("stats", out var statsEl)) {
                    var stats = JsonSerializer.Deserialize<Stats>(statsEl.GetRawText());
                    if (stats != null) {
                        r.LossPercent = stats.Loss;
                        r.Drop = stats.Drop;
                        r.Rcv = stats.Rcv;
                        r.Avg = stats.Avg;
                        r.StDev = stats.StDev;
                        r.Javg = stats.JAvg;
                    }
                }
                jsonList.Add(r);
            }
            return jsonList;
        }

        var raw = data?.RawOutput;
        if (string.IsNullOrWhiteSpace(raw)) {
            return new List<MtrHopResult>();
        }

        var list = new List<MtrHopResult>();
        var lines = raw!.Split('\n');
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
                    Host = m.Groups[3].Value.Trim(),
                    IpAddress = m.Groups[4].Value
                };
                var asnText = m.Groups[2].Value;
                if (asnText.StartsWith("AS", StringComparison.OrdinalIgnoreCase)) {
                    asnText = asnText.Substring(2);
                }
                if (int.TryParse(asnText, NumberStyles.Integer, CultureInfo.InvariantCulture, out var asn)) {
                    hop.Asn = asn;
                }
                if (double.TryParse(m.Groups[5].Value, NumberStyles.Float, CultureInfo.InvariantCulture, out var loss)) {
                    hop.LossPercent = loss;
                }
                if (int.TryParse(m.Groups[6].Value, NumberStyles.Integer, CultureInfo.InvariantCulture, out var drop)) {
                    hop.Drop = drop;
                }
                if (int.TryParse(m.Groups[7].Value, NumberStyles.Integer, CultureInfo.InvariantCulture, out var rcv)) {
                    hop.Rcv = rcv;
                }
                if (double.TryParse(m.Groups[8].Value, NumberStyles.Float, CultureInfo.InvariantCulture, out var avg)) {
                    hop.Avg = avg;
                }
                if (double.TryParse(m.Groups[9].Value, NumberStyles.Float, CultureInfo.InvariantCulture, out var stdev)) {
                    hop.StDev = stdev;
                }
                if (double.TryParse(m.Groups[10].Value, NumberStyles.Float, CultureInfo.InvariantCulture, out var javg)) {
                    hop.Javg = javg;
                }
                list.Add(hop);
            }
        }
        return list;
    }

    internal static List<DnsRecordResult> ParseDns(ResultDetails? data) {
        var resolver = data?.Resolver ?? string.Empty;
        var statusCode = data?.StatusCode ?? 0;
        var statusCodeName = data?.StatusCodeName ?? string.Empty;
        DnsTimings? timings = null;
        if (data?.Timings is JsonElement tEl && tEl.ValueKind == JsonValueKind.Object) {
            timings = JsonSerializer.Deserialize<DnsTimings>(tEl.GetRawText());
        }

        if (data?.Answers != null && data.Answers.Count > 0) {
            var header = ParseDnsHeaderInfo(data.RawOutput);
            return data.Answers.Select(a => new DnsRecordResult {
                Name = a.Name,
                Ttl = a.Ttl,
                Type = a.Type,
                Data = a.Value,
                QuestionName = header.QuestionName,
                QuestionType = header.QuestionType,
                Flags = header.Flags,
                Opcode = header.Opcode,
                HeaderStatus = header.HeaderStatus,
                QueryCount = header.QueryCount,
                AnswerCount = header.AnswerCount,
                AuthorityCount = header.AuthorityCount,
                AdditionalCount = header.AdditionalCount,
                Resolver = resolver,
                StatusCode = statusCode,
                StatusCodeName = statusCodeName,
                Timings = timings
            }).ToList();
        }
        if (data?.Hops is JsonElement hops && hops.ValueKind == JsonValueKind.Array) {
            var records = new List<DnsRecordResult>();
            foreach (var hop in hops.EnumerateArray()) {
                if (hop.TryGetProperty("answers", out var ansEl) && ansEl.ValueKind == JsonValueKind.Array) {
                    var ans = JsonSerializer.Deserialize<List<DnsAnswer>>(ansEl.GetRawText());
                    if (ans != null) {
                        var hopResolver = hop.TryGetProperty("resolver", out var resEl) ? resEl.GetString() ?? resolver : resolver;
                        var hopStatusCode = hop.TryGetProperty("statusCode", out var scEl) && scEl.ValueKind == JsonValueKind.Number ? scEl.GetInt32() : statusCode;
                        var hopStatusName = hop.TryGetProperty("statusCodeName", out var scnEl) ? scnEl.GetString() ?? statusCodeName : statusCodeName;
                        DnsTimings? hopTimings = timings;
                        if (hop.TryGetProperty("timings", out var tEl2) && tEl2.ValueKind == JsonValueKind.Object) {
                            hopTimings = JsonSerializer.Deserialize<DnsTimings>(tEl2.GetRawText());
                        }
                        records.AddRange(ans.Select(a => new DnsRecordResult {
                            Name = a.Name,
                            Ttl = a.Ttl,
                            Type = a.Type,
                            Data = a.Value,
                            Resolver = hopResolver,
                            StatusCode = hopStatusCode,
                            StatusCodeName = hopStatusName,
                            Timings = hopTimings
                        }));
                    }
                }
            }
            return records;
        }

        var raw = data?.RawOutput;
        if (string.IsNullOrWhiteSpace(raw)) {
            return new List<DnsRecordResult>();
        }

        var list = new List<DnsRecordResult>();
        var lines = raw!.Split('\n');
        var inAnswer = false;
        var flags = string.Empty;
        var opcode = string.Empty;
        var headerStatus = string.Empty;
        var qName = string.Empty;
        var qType = string.Empty;
        var qCount = 0;
        var aCount = 0;
        var authCount = 0;
        var addCount = 0;
        for (var i = 0; i < lines.Length; i++) {
            var line = lines[i];
            var t = line.Trim();
            if (t.StartsWith(";; ->>HEADER<<-")) {
                var m = Regex.Match(t, @"opcode:\s*(\S+),\s*status:\s*(\S+),\s*id:\s*(\d+)");
                if (m.Success) {
                    opcode = m.Groups[1].Value;
                    headerStatus = m.Groups[2].Value;
                }
                continue;
            }
            if (t.StartsWith(";; flags:")) {
                var m = Regex.Match(t, @"flags:\s*([^;]+);\s*QUERY:\s*(\d+),\s*ANSWER:\s*(\d+),\s*AUTHORITY:\s*(\d+),\s*ADDITIONAL:\s*(\d+)");
                if (m.Success) {
                    flags = m.Groups[1].Value.Trim();
                    qCount = int.Parse(m.Groups[2].Value);
                    aCount = int.Parse(m.Groups[3].Value);
                    authCount = int.Parse(m.Groups[4].Value);
                    addCount = int.Parse(m.Groups[5].Value);
                }
                continue;
            }
            if (t.StartsWith(";; QUESTION SECTION")) {
                if (i + 1 < lines.Length) {
                    var q = lines[i + 1].Trim(';').Trim();
                    var qm = Regex.Match(q, "^(\\S+)\\.\\s+IN\\s+(\\S+)");
                    if (qm.Success) {
                        qName = qm.Groups[1].Value;
                        qType = qm.Groups[2].Value;
                    }
                }
                continue;
            }
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
                        QuestionName = qName,
                        QuestionType = qType,
                        Flags = flags,
                        Opcode = opcode,
                        HeaderStatus = headerStatus,
                        QueryCount = qCount,
                        AnswerCount = aCount,
                        AuthorityCount = authCount,
                        AdditionalCount = addCount,
                        Name = m.Groups[1].Value,
                        Ttl = int.Parse(m.Groups[2].Value),
                        Type = m.Groups[3].Value,
                        Data = m.Groups[4].Value,
                        Resolver = resolver,
                        StatusCode = statusCode,
                        StatusCodeName = statusCodeName,
                        Timings = timings
                    };
                    list.Add(rec);
                }
            }
        }
        return list;
    }

    public static List<HttpResponseResult> GetHttpResponses(this MeasurementResponse response) {
        if (response.Results == null) {
            return new List<HttpResponseResult>();
        }

        return response.Results.SelectMany(r => ParseHttp(r.Data).Select(h =>
        {
            h.Target = response.Target;
            h.Continent = r.Probe.Continent;
            h.City = r.Probe.City;
            h.Country = r.Probe.Country;
            h.State = r.Probe.State;
            h.Asn = r.Probe.Asn;
            h.Network = r.Probe.Network;
            h.Version = r.Probe.Version;
            h.ResolvedAddress = r.Data?.ResolvedAddress;
            h.ResolvedHostname = r.Data?.ResolvedHostname;
            h.Status = r.Data != null ? r.Data.Status : default;
            return h;
        })).ToList();
    }

    internal static List<HttpResponseResult> ParseHttp(ResultDetails? data) {
        if (data == null) {
            return new List<HttpResponseResult>();
        }
        HttpTimings? timings = null;
        if (data.Timings is JsonElement timingsEl &&
            timingsEl.ValueKind != JsonValueKind.Undefined &&
            timingsEl.ValueKind != JsonValueKind.Null)
        {
            timings = JsonSerializer.Deserialize<HttpTimings>(timingsEl.GetRawText());
        }

        if (data.RawHeaders != null) {
            var resp = new HttpResponseResult
            {
                Protocol = string.Empty,
                StatusCode = data.StatusCode ?? 0,
                StatusDescription = data.StatusCodeName ?? string.Empty,
                Body = data.RawBody,
                Timings = timings,
                Tls = data.Tls,
            };
            if (data.Headers != null)
            {
                foreach (var kv in data.Headers)
                {
                    List<string> values = kv.Value.ValueKind == JsonValueKind.Array
                        ? kv.Value.EnumerateArray().Select(e => e.GetString() ?? string.Empty).ToList()
                        : new List<string> { kv.Value.GetString() ?? string.Empty };
                    resp.Headers[kv.Key] = Normalize(values);
                }
            }
            return new List<HttpResponseResult> { resp };
        }

        var raw = data.RawOutput;
        if (string.IsNullOrWhiteSpace(raw)) {
            return new List<HttpResponseResult>();
        }

        var result = new HttpResponseResult();
        var lines = raw!.Split('\n');
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

        var tempHeaders = new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase);
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
                if (!tempHeaders.TryGetValue(key, out var list))
                {
                    list = new List<string>();
                    tempHeaders[key] = list;
                }
                list.Add(value);
            }
        }

        foreach (var kv in tempHeaders)
        {
            result.Headers[kv.Key] = Normalize(kv.Value);
        }

        if (index < lines.Length) {
            result.Body = string.Join("\n", lines.Skip(index));
        }

        result.Timings = timings;
        result.Tls = data.Tls;

        return new List<HttpResponseResult> { result };
    }

    private static object? Normalize(List<string> values)
    {
        if (values.Count == 0)
        {
            return null;
        }

        if (values.Count == 1)
        {
            return ParseJson(values[0]);
        }

        var list = new List<object?>();
        foreach (var v in values)
        {
            list.Add(ParseJson(v));
        }
        return list;
    }

    private static object? ParseJson(string value)
    {
        var trimmed = value.Trim();
        if (!((trimmed.StartsWith("{") && trimmed.EndsWith("}")) ||
              (trimmed.StartsWith("[") && trimmed.EndsWith("]"))))
        {
            return value;
        }

        try
        {
            using var doc = JsonDocument.Parse(trimmed);
            return ConvertElement(doc.RootElement);
        }
        catch (JsonException)
        {
            return value;
        }
    }

    private static object? ConvertElement(JsonElement element)
    {
        switch (element.ValueKind)
        {
            case JsonValueKind.Object:
                var dict = new Dictionary<string, object?>();
                foreach (var prop in element.EnumerateObject())
                {
                    dict[prop.Name] = ConvertElement(prop.Value);
                }
                return dict;
            case JsonValueKind.Array:
                var list = new List<object?>();
                foreach (var item in element.EnumerateArray())
                {
                    list.Add(ConvertElement(item));
                }
                if (list.Count == 1)
                {
                    return list[0];
                }
                return list;
            case JsonValueKind.String:
                return element.GetString();
            case JsonValueKind.Number:
                if (element.TryGetInt64(out var l))
                {
                    return l;
                }
                return element.GetDouble();
            case JsonValueKind.True:
            case JsonValueKind.False:
                return element.GetBoolean();
            case JsonValueKind.Null:
                return null;
            default:
                return element.GetRawText();
        }
    }
}
