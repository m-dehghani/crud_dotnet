using PhoneNumbers;
using System.Text.Json.Serialization;

namespace Mc2.CrudTest.Presentation.Shared.ValueObjects
{
    public class PhoneNumber
    {
        public string Value { get; set; }

        [JsonConstructor]
        public PhoneNumber(string value)
        {
            char plusSign = '\u002B';
           // value = value.Trim(['\"']);
            String temp = value.StartsWith("\\u002B") ?  (char)plusSign + value.Substring(6, value.Length - 6) : value;
            if (!IsValidNumber(temp))
                throw new ArgumentException("102", nameof(value));

            Value = temp;
        }

        private bool IsValidNumber(string value)
        {
            // using google's libPhoneNumber package for validating phone numbers
            PhoneNumberUtil? phoneNumberUtil = PhoneNumbers.PhoneNumberUtil.GetInstance();
            try
            {
                PhoneNumbers.PhoneNumber? numberProto = phoneNumberUtil.Parse(value,"");
                int countryCode = numberProto.CountryCode;
                PhoneNumbers.PhoneNumber? temp = new PhoneNumbers.PhoneNumber();
                bool result =  phoneNumberUtil.IsValidNumber(numberProto);
                PhoneNumberType phoneNumberType = phoneNumberUtil.GetNumberType(numberProto);
                return result || phoneNumberType == PhoneNumberType.MOBILE;
                //return result;
            }
            catch (PhoneNumbers.NumberParseException)
            {
                return false;
            }
        }

    }
}