using System.Text.Json.Serialization;

namespace Mc2.CrudTest.Presentation.Shared.ValueObjects
{
    public record BankAccount
    {
        public string Value { get; set; }

        [JsonConstructor]
        public BankAccount(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("103", nameof(value));

            Value = value;
        }
      
    }
}