using System.Collections.Generic;
using System.Reflection;
using System.Text.Json;
using Globalping;
using Globalping.PowerShell;
using System.Management.Automation;
using Xunit;

namespace Globalping.Tests;

public class CmdletOutputCoverageTests
{
    [Fact]
    public void PingCommand_HandleOutput_DoesNotThrow()
    {
        var resp = new MeasurementResponse
        {
            Target = "example.com",
            Results = new List<Result>
            {
                new()
                {
                    Probe = new Probe(),
                    Data = new ResultDetails
                    {
                        Timings = JsonSerializer.SerializeToElement(new[]{ new { rtt = 1.0, ttl = 64 } })
                    }
                }
            }
        };
        var runtime = new TestCommandRuntime();
        var cmd = new PingCmd
        {
            CommandRuntime = runtime
        };
        InvokeHandle(cmd, resp);
    }

    [Fact]
    public void DnsCommand_HandleOutput_DoesNotThrow()
    {
        var resp = new MeasurementResponse
        {
            Target = "example.com",
            Results = new List<Result>
            {
                new()
                {
                    Probe = new Probe(),
                    Data = new ResultDetails
                    {
                        Answers = new List<DnsAnswer>
                        {
                            new() { Name = "example.com", Type = "A", Ttl = 60, Class = "IN", Value = "1.1.1.1" }
                        },
                        RawOutput = ";; ->>HEADER<<- opcode: QUERY, status: NOERROR, id: 1\n;; flags: qr rd ra; QUERY: 1, ANSWER: 1, AUTHORITY: 0, ADDITIONAL: 0\n\n;; QUESTION SECTION:\n;example.com. IN A\n"
                    }
                }
            }
        };
        var runtime = new TestCommandRuntime();
        var cmd = new DnsCmd
        {
            CommandRuntime = runtime
        };
        InvokeHandle(cmd, resp);
    }

    [Fact]
    public void HttpCommand_HandleOutput_DoesNotThrow()
    {
        var resp = new MeasurementResponse
        {
            Target = "example.com",
            Results = new List<Result>
            {
                new()
                {
                    Probe = new Probe(),
                    Data = new ResultDetails
                    {
                        RawOutput = "HTTP/1.1 200 OK\nContent-Type: text/plain\n\nHello"
                    }
                }
            }
        };
        var runtime = new TestCommandRuntime();
        var cmd = new HttpCmd
        {
            CommandRuntime = runtime
        };
        InvokeHandle(cmd, resp);
    }

    [Fact]
    public void MtrCommand_HandleOutput_DoesNotThrow()
    {
        var resp = new MeasurementResponse
        {
            Target = "example.com",
            Results = new List<Result>
            {
                new()
                {
                    Probe = new Probe(),
                    Data = new ResultDetails
                    {
                        RawOutput = "Host\n1. AS100 router (1.1.1.1) 0.0% 1 1 1.0 1.0 1.0"
                    }
                }
            }
        };
        var runtime = new TestCommandRuntime();
        var cmd = new MtrCmd
        {
            CommandRuntime = runtime
        };
        InvokeHandle(cmd, resp);
    }

    [Fact]
    public void TracerouteCommand_HandleOutput_DoesNotThrow()
    {
        var resp = new MeasurementResponse
        {
            Target = "example.com",
            Results = new List<Result>
            {
                new()
                {
                    Probe = new Probe(),
                    Data = new ResultDetails
                    {
                        RawOutput = "traceroute to example.com\n1 router (1.1.1.1) 1.0 ms 1.0 ms"
                    }
                }
            }
        };
        var runtime = new TestCommandRuntime();
        var cmd = new TraceCmd
        {
            CommandRuntime = runtime
        };
        InvokeHandle(cmd, resp);
    }

    private static void InvokeHandle(PSCmdlet cmd, MeasurementResponse resp)
    {
        var method = cmd.GetType().GetMethod("HandleOutput", BindingFlags.Instance | BindingFlags.NonPublic);
        method!.Invoke(cmd, new object?[] { resp });
    }

    private sealed class PingCmd : StartGlobalpingPingCommand
    {
    }

    private sealed class DnsCmd : StartGlobalpingDnsCommand
    {
    }

    private sealed class HttpCmd : StartGlobalpingHttpCommand
    {
    }

    private sealed class MtrCmd : StartGlobalpingMtrCommand
    {
    }

    private sealed class TraceCmd : StartGlobalpingTracerouteCommand
    {
    }
}
