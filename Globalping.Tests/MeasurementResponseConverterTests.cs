using System.Text.Json;
using System.Text.Json.Serialization;
using Xunit;

namespace Globalping.Tests;

public class MeasurementResponseConverterTests
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
    };

    static MeasurementResponseConverterTests()
    {
        JsonOptions.Converters.Add(new JsonStringEnumConverter<MeasurementStatus>(JsonNamingPolicy.KebabCaseLower));
        JsonOptions.Converters.Add(new JsonStringEnumConverter<TestStatus>(JsonNamingPolicy.KebabCaseLower));
        JsonOptions.Converters.Add(new CountryCodeConverter());
        JsonOptions.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
    }

    [Fact]
    public void UnknownType_YieldsNullOptions()
    {
        var json = """
        {
            "id": "1",
            "type": "unknown",
            "status": "finished",
            "target": "example.com",
            "probesCount": 0,
            "measurementOptions": { "x": 1 }
        }
        """;
        var result = JsonSerializer.Deserialize<MeasurementResponse>(json, JsonOptions);
        Assert.NotNull(result);
        Assert.Null(result!.MeasurementOptions);
    }
}
