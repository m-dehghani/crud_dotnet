using System.Net.Mail;
using System.Text.Json.Serialization;
using Mc2.CrudTest.Presentation.Shared.DomainExceptions;

namespace Mc2.CrudTest.Presentation.Shared.ValueObjects
{
    public record Email
    {
        public string Value { get; }

        [JsonConstructor]
        public Email(string value)
        {
            if (string.IsNullOrWhiteSpace(value) || !value.Contains('@'))
            {
                throw new InvalidEmailException("Bad Email", nameof(value));
            }
           
            Value = value.Trim().ToLowerInvariant();;
        }
    }
}