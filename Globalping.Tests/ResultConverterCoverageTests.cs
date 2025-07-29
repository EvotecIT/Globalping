using System.Collections.Generic;
using System.Text.Json;
using Xunit;

namespace Globalping.Tests;

public class ResultConverterCoverageTests
{
    [Fact]
    public void InstanceMethods_DelegateToExtensions()
    {
        var result = new Result
        {
            Target = "example.com",
            Probe = new Probe(),
            Data = new ResultDetails()
        };

        Assert.Equal(ResultExtensions.ToPingTimings(result, "example.com").Count, result.ToPingTimings("example.com").Count);
        Assert.Equal(ResultExtensions.ToPingTimings(result, result.Target).Count, result.ToPingTimings().Count);
        Assert.Equal(ResultExtensions.ToTracerouteHops(result, "example.com").Count, result.ToTracerouteHops("example.com").Count);
        Assert.Equal(ResultExtensions.ToTracerouteHops(result, result.Target).Count, result.ToTracerouteHops().Count);
        Assert.Equal(ResultExtensions.ToMtrHops(result, "example.com").Count, result.ToMtrHops("example.com").Count);
        Assert.Equal(ResultExtensions.ToMtrHops(result, result.Target).Count, result.ToMtrHops().Count);
        Assert.Equal(ResultExtensions.ToDnsRecords(result, "example.com").Count, result.ToDnsRecords("example.com").Count);
        Assert.Equal(ResultExtensions.ToDnsRecords(result, result.Target).Count, result.ToDnsRecords().Count);
        Assert.Equal(ResultExtensions.ToHttpResponse(result, "example.com")?.StatusCode, result.ToHttpResponse("example.com")?.StatusCode);
        Assert.Equal(ResultExtensions.ToHttpResponse(result, result.Target)?.StatusCode, result.ToHttpResponse()?.StatusCode);
    }
}
