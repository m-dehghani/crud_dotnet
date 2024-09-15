using PhoneNumbers;
using System.Text.Json.Serialization;
using Mc2.CrudTest.Presentation.Shared.DomainExceptions;

namespace Mc2.CrudTest.Presentation.Shared.ValueObjects
{
    // using google's libPhoneNumber package for validating phone numbers
    public class PhoneNumber
    {
        public string Value { get; }

        [JsonConstructor]
        public PhoneNumber(string value)
        {
            char plusSign = '\u002B';
           
           string temp = value.StartsWith("\\u002B") 
               ?  plusSign + value.Substring(6, value.Length - 6)
               : value;
          
            if (!IsValidNumber(temp))
                throw new InvalidPhonenumberException("bad phone number"
                    , nameof(value));

            Value = temp;
        }

      
        private static bool IsValidNumber(string value)
        {
            
            PhoneNumberUtil? phoneNumberUtil = PhoneNumberUtil.GetInstance();
            try
            {
                PhoneNumbers.PhoneNumber? numberProto = phoneNumberUtil.Parse(value,"");
            
                bool result =  phoneNumberUtil.IsValidNumber(numberProto);
                
                PhoneNumberType phoneNumberType = phoneNumberUtil.GetNumberType(numberProto);
                
                return result || phoneNumberType == PhoneNumberType.MOBILE;
            }
            catch (NumberParseException)
            {
                return false;
            }
        }

    }
}