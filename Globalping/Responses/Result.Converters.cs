using System.Collections.Generic;

namespace Globalping;

public partial class Result
{
    public List<PingTimingResult> ToPingTimings(string target)
        => ResultExtensions.ToPingTimings(this, target);

    public List<PingTimingResult> ToPingTimings()
        => ResultExtensions.ToPingTimings(this, Target);

    public List<TracerouteHopResult> ToTracerouteHops(string target)
        => ResultExtensions.ToTracerouteHops(this, target);

    public List<TracerouteHopResult> ToTracerouteHops()
        => ResultExtensions.ToTracerouteHops(this, Target);

    public List<MtrHopResult> ToMtrHops(string target)
        => ResultExtensions.ToMtrHops(this, target);

    public List<MtrHopResult> ToMtrHops()
        => ResultExtensions.ToMtrHops(this, Target);

    public List<DnsRecordResult> ToDnsRecords(string target)
        => ResultExtensions.ToDnsRecords(this, target);

    public List<DnsRecordResult> ToDnsRecords()
        => ResultExtensions.ToDnsRecords(this, Target);

    public HttpResponseResult? ToHttpResponse(string target)
        => ResultExtensions.ToHttpResponse(this, target);

    public HttpResponseResult? ToHttpResponse()
        => ResultExtensions.ToHttpResponse(this, Target);
}
