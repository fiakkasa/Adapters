using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Adapters.Json;

public class NullableTimeOnlyJsonConverter : JsonConverter<TimeOnly?>
{
    public override TimeOnly? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) =>
        TimeOnly.TryParse(reader.GetString(), CultureInfo.InvariantCulture, DateTimeStyles.None, out var dt)
            ? dt
            : default(TimeOnly?);

    public override void Write(Utf8JsonWriter writer, TimeOnly? value, JsonSerializerOptions options)
    {
        if (value is { } v)
            writer.WriteStringValue(v.ToString(CultureInfo.InvariantCulture));
    }
}
