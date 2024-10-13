using System.Text.Json;
using System.Text.Json.Serialization;

namespace ICorteApi.Domain.Utils;

// TimeSpan cannot be directly conversion by JSON until .NET v8
public class TimeSpanConverter : JsonConverter<TimeSpan>
{
    public override TimeSpan Read(
        ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) =>
        TimeSpan.Parse(reader.GetString());

    public override void Write(Utf8JsonWriter writer, TimeSpan value, JsonSerializerOptions options) =>
        writer.WriteStringValue(value.ToString());
}
