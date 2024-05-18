namespace Mc2.CrudTest.Presentation.Shared.ValueObjects
{
    public class Email:IEquatable<Email>
    {
        public string Value { get; }

        public Email(string value)
        {
            if (string.IsNullOrWhiteSpace(value) || !value.Contains("@"))
                throw new ArgumentException("Invalid email format.", nameof(value));

            Value = value.Trim().ToLowerInvariant();;
        }

        public override bool Equals(object obj) => Equals(obj as Email);

        public bool Equals(Email other)
        {
            if (other == null)
                return false;

            return string.Equals(Value, other.Value, StringComparison.OrdinalIgnoreCase);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }
    }
}