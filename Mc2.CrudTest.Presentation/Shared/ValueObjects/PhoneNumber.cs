using System.Text.Json.Serialization;

namespace Mc2.CrudTest.Presentation.Shared.ValueObjects
{
    public class PhoneNumber
    {
        public string Value { get; set; }

        [JsonConstructor]
        public PhoneNumber(string value)
        {
            value = value.Trim(['\"']);
            String temp = (char)'\u002B' + value.Substring(6, value.Length - 6);
            if (!IsValidNumber(temp))
                throw new ArgumentException("Invalid phone number format. phone numbers should start with country code like +1", nameof(value));

            Value = temp;
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