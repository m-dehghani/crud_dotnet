using System.Text.Json.Serialization;
using System.Text.Json;

namespace Mc2.CrudTest.Presentation.Shared.Helper
{
    public sealed class DateOnlyJsonConverter : JsonConverter<DateOnly>
    {
        public override DateOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return DateOnly.FromDateTime(reader.GetDateTime());
        }

        public override void Write(Utf8JsonWriter writer, DateOnly value, JsonSerializerOptions options)
        {
            string? isoDate = value.ToString("O");
            writer.WriteStringValue(isoDate);
        }
    }
}
