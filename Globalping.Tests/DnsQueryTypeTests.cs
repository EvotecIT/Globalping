using System.Text.Json;
using System.Text.Json.Serialization;
using Xunit;

namespace Globalping.Tests;

public sealed class DnsQueryTypeTests
{
    [Fact]
    public void SerializesInCamelCase()
    {
        var options = new JsonSerializerOptions();
        options.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
        var json = JsonSerializer.Serialize(DnsQueryType.SVCB, options);
        Assert.Equal("\"svcb\"", json);
    }
}
