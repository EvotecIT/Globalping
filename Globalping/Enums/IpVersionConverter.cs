using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Globalping;

public sealed class IpVersionConverter : JsonConverter<IpVersion>
{
    public override IpVersion Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var value = reader.GetInt32();
        return value == 6 ? IpVersion.Six : IpVersion.Four;
    }

    public override void Write(Utf8JsonWriter writer, IpVersion value, JsonSerializerOptions options)
    {
        writer.WriteNumberValue((int)value);
    }
}
