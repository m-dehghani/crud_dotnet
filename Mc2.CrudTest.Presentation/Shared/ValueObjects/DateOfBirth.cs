namespace Mc2.CrudTest.Presentation.Shared.ValueObjects
{
    public record DateOfBirth
    {
        public DateTime Value { get; }

        public DateOfBirth(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Invalid Date format.", nameof(value));

            Value = DateTime.Parse(value,
                System.Globalization.CultureInfo.InvariantCulture);
        }


    }
}