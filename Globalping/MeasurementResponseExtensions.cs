using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace Globalping;

public static class MeasurementResponseExtensions
{
    public static IEnumerable<ResultSummary> GetSummaries(this MeasurementResponse response)
    {
        if (response.Results == null)
        {
            return Enumerable.Empty<ResultSummary>();
        }

        return response.Results.Select(r => new ResultSummary
        {
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
}
