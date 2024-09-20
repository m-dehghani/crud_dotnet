using System.Text.Json.Serialization;
using Mc2.CrudTest.Presentation.Shared.DomainExceptions;
using Mc2.CrudTest.Presentation.Shared.Validators.Concrete;

namespace Mc2.CrudTest.Presentation.Shared.ValueObjects
{
    public record Email : IValidatable
    {
        public string Value { get; }

        [JsonConstructor]
        public Email(string value)
        {
            if (!Validate().IsValid)
            {
                throw new InvalidEmailException("Bad Email", nameof(value));
            }
           
            Value = value.Trim().ToLowerInvariant();;
        }
        
        public ValidationResult Validate()
        {
            if (string.IsNullOrEmpty(Value) || !Value.Contains('@'))
            {
                return new ValidationResult
                {
                    IsValid = false,
                    ErrorMessage = "Email cannot be null or empty.",
                    ErrorCode = 1003
                };
            }

            return new ValidationResult { IsValid = true };
        }
     
    }
}