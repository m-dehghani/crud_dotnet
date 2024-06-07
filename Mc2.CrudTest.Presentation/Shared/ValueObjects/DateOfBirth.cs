using System.Text.Json.Serialization;

namespace Mc2.CrudTest.Presentation.Shared.ValueObjects
{
    public record DateOfBirth
    {
        public DateOnly Value { get; set; }

        [JsonConstructor]
        public DateOfBirth(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Invalid Date format.", nameof(value));

            Value = DateOnly.Parse(value,
                System.Globalization.CultureInfo.InvariantCulture);
        }
       


    }
}