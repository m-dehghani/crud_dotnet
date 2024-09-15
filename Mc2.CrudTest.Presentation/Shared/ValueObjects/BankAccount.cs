using System.Text.Json.Serialization;
using Mc2.CrudTest.Presentation.Shared.DomainExceptions;

namespace Mc2.CrudTest.Presentation.Shared.ValueObjects
{
    public record BankAccount
    {
        public string Value { get; set; }

        [JsonConstructor]
        public BankAccount(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new InvalidBankAccountNumberException("103", nameof(value));
            }

            Value = value;
        }
      
    }
}