using System.Text.Json;
using System.Text.Json.Serialization;

namespace BBMDown.JsonConverters
{
    public sealed class BooleanNumberConverter : JsonConverter<bool>
    {
        public override bool Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return reader.GetInt32() switch
            {
                0 => false,
                1 => true,
                _ => throw new JsonException()
            };
        }

        public override void Write(Utf8JsonWriter writer, bool value, JsonSerializerOptions options)
        {
            writer.WriteNumberValue(value ? 1 : 0);
        }
    }
}
