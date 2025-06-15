using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Globalping;

internal sealed class MeasurementResponseConverter : JsonConverter<MeasurementResponse>
{
    private class RawResponse
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("type")]
        public string? Type { get; set; }

        [JsonPropertyName("status")]
        public MeasurementStatus Status { get; set; }

        [JsonPropertyName("createdAt")]
        public DateTime CreatedAt { get; set; }

        [JsonPropertyName("updatedAt")]
        public DateTime UpdatedAt { get; set; }

        [JsonPropertyName("target")]
        public string Target { get; set; } = string.Empty;

        [JsonPropertyName("probesCount")]
        public int ProbesCount { get; set; }

        [JsonPropertyName("measurementOptions")]
        public JsonElement? MeasurementOptions { get; set; }

        [JsonPropertyName("locations")]
        public List<LocationRequest>? Locations { get; set; }

        [JsonPropertyName("limit")]
        public int? Limit { get; set; }

        [JsonPropertyName("results")]
        public List<Result>? Results { get; set; }
    }

    public override MeasurementResponse? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var raw = JsonSerializer.Deserialize<RawResponse>(ref reader, options);
        if (raw == null)
        {
            return null;
        }

        var response = new MeasurementResponse
        {
            Id = raw.Id,
            Type = raw.Type,
            Status = raw.Status,
            CreatedAt = raw.CreatedAt,
            UpdatedAt = raw.UpdatedAt,
            Target = raw.Target,
            ProbesCount = raw.ProbesCount,
            Locations = raw.Locations,
            Limit = raw.Limit,
            Results = raw.Results
        };

        if (raw.MeasurementOptions.HasValue)
        {
            var elem = raw.MeasurementOptions.Value;
            var type = raw.Type?.ToLowerInvariant();
            response.MeasurementOptions = type switch
            {
                "ping" => elem.Deserialize<PingOptions>(options),
                "traceroute" => elem.Deserialize<TracerouteOptions>(options),
                "dns" => elem.Deserialize<DnsOptions>(options),
                "mtr" => elem.Deserialize<MtrOptions>(options),
                "http" => elem.Deserialize<HttpOptions>(options),
                _ => null
            };
        }

        return response;
    }

    public override void Write(Utf8JsonWriter writer, MeasurementResponse value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        writer.WriteString("id", value.Id);
        if (value.Type != null)
        {
            writer.WriteString("type", value.Type);
        }

        writer.WritePropertyName("status");
        JsonSerializer.Serialize(writer, value.Status, options);
        writer.WriteString("createdAt", value.CreatedAt);
        writer.WriteString("updatedAt", value.UpdatedAt);
        writer.WriteString("target", value.Target);
        writer.WriteNumber("probesCount", value.ProbesCount);

        if (value.MeasurementOptions != null)
        {
            writer.WritePropertyName("measurementOptions");
            JsonSerializer.Serialize(writer, value.MeasurementOptions, value.MeasurementOptions.GetType(), options);
        }

        if (value.Locations != null)
        {
            writer.WritePropertyName("locations");
            JsonSerializer.Serialize(writer, value.Locations, options);
        }
        if (value.Limit.HasValue)
        {
            writer.WriteNumber("limit", value.Limit.Value);
        }
        if (value.Results != null)
        {
            writer.WritePropertyName("results");
            JsonSerializer.Serialize(writer, value.Results, options);
        }

        writer.WriteEndObject();
    }
}
