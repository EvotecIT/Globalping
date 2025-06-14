using System.Collections.Generic;

namespace Globalping;

public partial class Result
{
    public IEnumerable<PingTimingResult> ToPingTimings(string target)
        => ResultExtensions.ToPingTimings(this, target);

    public IEnumerable<PingTimingResult> ToPingTimings()
        => ResultExtensions.ToPingTimings(this, Target);

    public IEnumerable<TracerouteHopResult> ToTracerouteHops(string target)
        => ResultExtensions.ToTracerouteHops(this, target);

    public IEnumerable<TracerouteHopResult> ToTracerouteHops()
        => ResultExtensions.ToTracerouteHops(this, Target);

    public IEnumerable<MtrHopResult> ToMtrHops(string target)
        => ResultExtensions.ToMtrHops(this, target);

    public IEnumerable<MtrHopResult> ToMtrHops()
        => ResultExtensions.ToMtrHops(this, Target);

    public IEnumerable<DnsRecordResult> ToDnsRecords(string target)
        => ResultExtensions.ToDnsRecords(this, target);

    public IEnumerable<DnsRecordResult> ToDnsRecords()
        => ResultExtensions.ToDnsRecords(this, Target);

    public HttpResponseResult? ToHttpResponse(string target)
        => ResultExtensions.ToHttpResponse(this, target);

    public HttpResponseResult? ToHttpResponse()
        => ResultExtensions.ToHttpResponse(this, Target);
}
