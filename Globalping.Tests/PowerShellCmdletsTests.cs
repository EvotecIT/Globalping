using Globalping;
using Globalping.PowerShell;
using Xunit;

namespace Globalping.Tests;

public class PowerShellCmdletsTests
{
    [Fact]
    public void ComputeLimit_UsesUserLimit()
    {
        var result = StartGlobalpingBaseCommand.ComputeLimit(5, true, null, null, null);
        Assert.Equal(5, result);
    }

    [Fact]
    public void ComputeLimit_IgnoresLocationsWhenReusing()
    {
        var locations = new[] { new LocationRequest { Country = CountryCode.Germany } };
        var result = StartGlobalpingBaseCommand.ComputeLimit(null, false, "prev", new[] { "DE" }, locations);
        Assert.Null(result);
    }

    [Fact]
    public void PingCommand_TypeAndOptions()
    {
        var opts = new PingOptions { Packets = 2 };
        var cmd = new ExposedPingCommand { Options = opts };
        Assert.Equal(MeasurementType.Ping, cmd.TypeAccessor);
        Assert.Same(opts, cmd.OptionsAccessor);
    }

    [Fact]
    public void DnsCommand_TypeAndOptions()
    {
        var opts = new DnsOptions { Port = 53 };
        var cmd = new ExposedDnsCommand { Options = opts };
        Assert.Equal(MeasurementType.Dns, cmd.TypeAccessor);
        Assert.Same(opts, cmd.OptionsAccessor);
    }

    [Fact]
    public void HttpCommand_TypeAndOptions()
    {
        var opts = new HttpOptions { Port = 80 };
        var cmd = new ExposedHttpCommand { Options = opts };
        Assert.Equal(MeasurementType.Http, cmd.TypeAccessor);
        Assert.Same(opts, cmd.OptionsAccessor);
    }

    [Fact]
    public void MtrCommand_TypeAndOptions()
    {
        var opts = new MtrOptions { Packets = 1 };
        var cmd = new ExposedMtrCommand { Options = opts };
        Assert.Equal(MeasurementType.Mtr, cmd.TypeAccessor);
        Assert.Same(opts, cmd.OptionsAccessor);
    }

    [Fact]
    public void TracerouteCommand_TypeAndOptions()
    {
        var opts = new TracerouteOptions { Port = 33434 };
        var cmd = new ExposedTracerouteCommand { Options = opts };
        Assert.Equal(MeasurementType.Traceroute, cmd.TypeAccessor);
        Assert.Same(opts, cmd.OptionsAccessor);
    }

    private sealed class ExposedPingCommand : StartGlobalpingPingCommand
    {
        public MeasurementType TypeAccessor => base.Type;
        public IMeasurementOptions? OptionsAccessor => base.MeasurementOptions;
        public void InvokeHandle(MeasurementResponse? response) => base.HandleOutput(response);
    }

    private sealed class ExposedDnsCommand : StartGlobalpingDnsCommand
    {
        public MeasurementType TypeAccessor => base.Type;
        public IMeasurementOptions? OptionsAccessor => base.MeasurementOptions;
    }

    private sealed class ExposedHttpCommand : StartGlobalpingHttpCommand
    {
        public MeasurementType TypeAccessor => base.Type;
        public IMeasurementOptions? OptionsAccessor => base.MeasurementOptions;
    }

    private sealed class ExposedMtrCommand : StartGlobalpingMtrCommand
    {
        public MeasurementType TypeAccessor => base.Type;
        public IMeasurementOptions? OptionsAccessor => base.MeasurementOptions;
    }

    private sealed class ExposedTracerouteCommand : StartGlobalpingTracerouteCommand
    {
        public MeasurementType TypeAccessor => base.Type;
        public IMeasurementOptions? OptionsAccessor => base.MeasurementOptions;
    }
}
