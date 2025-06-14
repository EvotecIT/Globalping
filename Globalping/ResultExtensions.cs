using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace Globalping;

public static class ResultExtensions
{
    public static IEnumerable<PingTimingResult> ToPingTimings(this Result result, string target)
    {
        if (result.Data?.Timings is JsonElement element && element.ValueKind == JsonValueKind.Array)
        {
            return element.EnumerateArray().Select((t, idx) =>
            {
                var timing = JsonSerializer.Deserialize<Timing>(t.GetRawText());
                return new PingTimingResult
                {
                    Target = target,
                    IcmpSequence = idx + 1,
                    TTL = timing?.Ttl ?? 0,
                    Time = timing?.Rtt ?? 0,
                    Country = result.Probe.Country,
                    City = result.Probe.City,
                    Network = result.Probe.Network,
                    Asn = result.Probe.Asn,
                    State = result.Probe.State,
                    Continent = result.Probe.Continent,
                    ResolvedAddress = result.Data?.ResolvedAddress,
                    ResolvedHostname = result.Data?.ResolvedHostname,
                    Status = result.Data?.Status,
                };
            });
        }
        return Enumerable.Empty<PingTimingResult>();
    }

    public static IEnumerable<TracerouteHopResult> ToTracerouteHops(this Result result, string target)
    {
        return MeasurementResponseExtensions.ParseTraceroute(result.Data?.RawOutput).Select(h =>
        {
            h.Target = target;
            h.Country = result.Probe.Country;
            h.City = result.Probe.City;
            h.Network = result.Probe.Network;
            h.Asn = result.Probe.Asn;
            h.State = result.Probe.State;
            h.Continent = result.Probe.Continent;
            h.ResolvedAddress = result.Data?.ResolvedAddress;
            h.ResolvedHostname = result.Data?.ResolvedHostname;
            h.Status = result.Data?.Status;
            return h;
        });
    }

    public static IEnumerable<MtrHopResult> ToMtrHops(this Result result, string target)
    {
        return MeasurementResponseExtensions.ParseMtr(result.Data?.RawOutput).Select(h =>
        {
            h.Target = target;
            h.Country = result.Probe.Country;
            h.City = result.Probe.City;
            h.Network = result.Probe.Network;
            h.ProbeAsn = result.Probe.Asn;
            h.State = result.Probe.State;
            h.Continent = result.Probe.Continent;
            h.ResolvedAddress = result.Data?.ResolvedAddress;
            h.ResolvedHostname = result.Data?.ResolvedHostname;
            h.Status = result.Data?.Status;
            return h;
        });
    }

    public static IEnumerable<DnsRecordResult> ToDnsRecords(this Result result, string target)
    {
        return MeasurementResponseExtensions.ParseDns(result.Data?.RawOutput).Select(h =>
        {
            h.Target = target;
            h.Country = result.Probe.Country;
            h.City = result.Probe.City;
            h.Network = result.Probe.Network;
            h.Asn = result.Probe.Asn;
            h.State = result.Probe.State;
            h.Continent = result.Probe.Continent;
            h.Status = result.Data?.Status;
            return h;
        });
    }

    public static HttpResponseResult? ToHttpResponse(this Result result, string target)
    {
        var resp = MeasurementResponseExtensions.ParseHttp(result.Data?.RawOutput).FirstOrDefault();
        if (resp is null)
        {
            return null;
        }

        resp.Target = target;
        resp.Country = result.Probe.Country;
        resp.City = result.Probe.City;
        resp.Network = result.Probe.Network;
        resp.Asn = result.Probe.Asn;
        resp.State = result.Probe.State;
        resp.Continent = result.Probe.Continent;
        resp.ResolvedAddress = result.Data?.ResolvedAddress;
        resp.ResolvedHostname = result.Data?.ResolvedHostname;
        resp.Status = result.Data?.Status;
        return resp;
    }
}
