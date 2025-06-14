using System.Collections.Generic;

namespace Globalping;

public partial class Result
{
    public IEnumerable<PingTimingResult> ToPingTimings(string target)
        => ResultExtensions.ToPingTimings(this, target);

    public IEnumerable<PingTimingResult> ToPingTimings()
        => ResultExtensions.ToPingTimings(this, string.Empty);

    public IEnumerable<TracerouteHopResult> ToTracerouteHops(string target)
        => ResultExtensions.ToTracerouteHops(this, target);

    public IEnumerable<TracerouteHopResult> ToTracerouteHops()
        => ResultExtensions.ToTracerouteHops(this, string.Empty);

    public IEnumerable<MtrHopResult> ToMtrHops(string target)
        => ResultExtensions.ToMtrHops(this, target);

    public IEnumerable<MtrHopResult> ToMtrHops()
        => ResultExtensions.ToMtrHops(this, string.Empty);

    public IEnumerable<DnsRecordResult> ToDnsRecords(string target)
        => ResultExtensions.ToDnsRecords(this, target);

    public IEnumerable<DnsRecordResult> ToDnsRecords()
        => ResultExtensions.ToDnsRecords(this, string.Empty);

    public HttpResponseResult? ToHttpResponse(string target)
        => ResultExtensions.ToHttpResponse(this, target);

    public HttpResponseResult? ToHttpResponse()
        => ResultExtensions.ToHttpResponse(this, string.Empty);
}
