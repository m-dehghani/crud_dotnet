using System.Text.Json.Serialization;
using Mc2.CrudTest.Presentation.Shared.DomainExceptions;
using Mc2.CrudTest.Presentation.Shared.Validators.Concrete;

namespace Mc2.CrudTest.Presentation.Shared.ValueObjects
{
    public record BankAccount : IValidatable
    {
        public string? Value { get; set; }

        [JsonConstructor]
        public BankAccount(string value)
        {
            if (!Validate().IsValid)
                throw new InvalidBankAccountNumberException("Invalid BankAccount", nameof(value));
            
            Value = value;
            
        }
        
        public ValidationResult Validate()
        {
            if (string.IsNullOrEmpty(Value))
            {
                return new ValidationResult
                {
                    IsValid = false,
                    ErrorMessage = "Bank account cannot be null or empty.",
                    ErrorCode = 1002
                };
            }

            // Add more validation logic here

            return new ValidationResult { IsValid = true };
        }
      
    }
}