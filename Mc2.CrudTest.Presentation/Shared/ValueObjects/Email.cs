using System.Net.Mail;
using System.Text.Json.Serialization;

namespace Mc2.CrudTest.Presentation.Shared.ValueObjects
{
    public record Email
    {
        public string Value { get; set; }

        [JsonConstructor]
        public Email(string value)
        {
            if (string.IsNullOrWhiteSpace(value) || !value.Contains("@") /*|| !Helper.RegexPatterns.EmailIsValid.IsMatch(value)*/)
                throw new ArgumentException("Invalid email format.", nameof(value));
           
            Value = value.Trim().ToLowerInvariant();;
        }
    }
}