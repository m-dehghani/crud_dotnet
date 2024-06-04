namespace Mc2.CrudTest.Presentation.Shared.ValueObjects
{
    public class PhoneNumber
    {
        public string Value { get; }

        public PhoneNumber(string value)
        {
            if (!IsValidNumber(value))
                throw new ArgumentException("Invalid phone number format. phone numbers should start with country code like +1", nameof(value));

            Value = value;
        }

        private bool IsValidNumber(string value)
        {
            
            // using google's libPhoneNumber package for validating phone numbers
            var phoneNumberUtil = PhoneNumbers.PhoneNumberUtil.GetInstance();
            try
            {
                var numberProto = phoneNumberUtil.Parse(value,"");
                var countryCode = numberProto.CountryCode;
                return phoneNumberUtil.IsValidNumber(numberProto);
            }
            catch (PhoneNumbers.NumberParseException)
            {
                return false;
            }
        }

    }
}