using System.Text.Json.Serialization;
using Mc2.CrudTest.Presentation.Shared.DomainExceptions;
using Mc2.CrudTest.Presentation.Shared.Validators.Concrete;

namespace Mc2.CrudTest.Presentation.Shared.ValueObjects
{
    public record DateOfBirth : IValidatable
    {
        public DateOnly? Value { get; set; }

        [JsonConstructor]
        public DateOfBirth(string value)
        {
            if (!Validate().IsValid)
                throw new InvalidDateOfBirthException("Invalid Date format.", nameof(value));

            Value = DateOnly.Parse(value,
                System.Globalization.CultureInfo.InvariantCulture);
        }
        
        public ValidationResult Validate()
        {
            if (Value == null)
            {
                return new ValidationResult
                {
                    IsValid = false,
                    ErrorMessage = "Date of birth cannot be null or empty.",
                    ErrorCode = 1004
                };
            }

            return new ValidationResult { IsValid = true };
        }

    }
}