using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

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
            r.Data?.Timings?.Select((t, idx) => new PingTimingResult {
                Target = response.Target,
                Continent = r.Probe.Continent,
                City = r.Probe.City,
                Country = r.Probe.Country,
                IcmpSequence = idx + 1,
                TTL = t.Ttl,
                Time = t.Rtt,
                State = r.Probe.State,
                Asn = r.Probe.Asn,
                Network = r.Probe.Network,
                ResolvedAddress = r.Data?.ResolvedAddress,
                ResolvedHostname = r.Data?.ResolvedHostname,
                Status = r.Data?.Status,
            }) ?? Enumerable.Empty<PingTimingResult>());
    }
}
