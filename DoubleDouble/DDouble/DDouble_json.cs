using System.Text.Json;
using System.Text.Json.Serialization;

namespace DoubleDouble {
    [JsonConverter(typeof(DDoubleJsonConverter))]
    public partial struct ddouble { }

    public class DDoubleJsonConverter : JsonConverter<ddouble> {
        public override ddouble Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
            return ddouble.Parse(reader.GetString()!);
        }

        public override void Write(Utf8JsonWriter writer, ddouble value, JsonSerializerOptions options) {
            writer.WriteStringValue(value.ToString());
        }
    }
}