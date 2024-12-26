using System;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace API.Helpers;

public class DateTimeOffsetConverter : JsonConverter<DateTimeOffset>
{
    public override DateTimeOffset Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        => DateTimeOffset.Parse(
            reader.GetString()!,
            null,
            DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal
        );

    public override void Write(Utf8JsonWriter writer, DateTimeOffset value, JsonSerializerOptions options)
        => writer.WriteStringValue(value.ToUniversalTime().ToString("o"));
}
