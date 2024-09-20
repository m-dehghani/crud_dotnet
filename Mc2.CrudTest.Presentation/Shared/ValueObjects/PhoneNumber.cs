using PhoneNumbers;
using System.Text.Json.Serialization;
using Mc2.CrudTest.Presentation.Shared.DomainExceptions;
using Mc2.CrudTest.Presentation.Shared.Validators.Concrete;

namespace Mc2.CrudTest.Presentation.Shared.ValueObjects
{
    // using google's libPhoneNumber package for validating phone numbers
    public class PhoneNumber : IValidatable
    {
        public string Value { get; }

        [JsonConstructor]
        public PhoneNumber(string value)
        {
            char plusSign = '\u002B';
           
           string temp = value.StartsWith("\\u002B") 
               ?  plusSign + value.Substring(6, value.Length - 6)
               : value;
          
            if (!Validate().IsValid)
                throw new InvalidPhonenumberException("bad phone number"
                    , nameof(value));

            Value = temp;
        }
        public ValidationResult Validate()
        {
            if (string.IsNullOrEmpty(Value))
            {
                return new ValidationResult
                {
                    IsValid = false,
                    ErrorMessage = "Phone number cannot be null or empty.",
                    ErrorCode = 1001
                };
            }
            
            return new ValidationResult { IsValid = IsValidNumber(Value) };
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