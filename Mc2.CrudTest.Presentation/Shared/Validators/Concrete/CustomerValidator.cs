using Mc2.CrudTest.Presentation.Shared.Validators.Abstract;
using Mc2.CrudTest.Presentation.Shared.ValueObjects;

namespace Mc2.CrudTest.Presentation.Shared.Validators.Concrete;

public class CustomerValidator : ICustomerValidator
{
    public ValidationResult ValidateCustomer(string firstName, string lastName, string phoneNumber, string email, string bankAccount, string dateOfBirth)
    {
        if (string.IsNullOrEmpty(firstName))
        {
            return new ValidationResult
            {
                IsValid = false,
                ErrorMessage = "First name cannot be null or empty.",
                ErrorCode = 2001
            };
        }

        if (string.IsNullOrEmpty(lastName))
        {
            return new ValidationResult
            {
                IsValid = false,
                ErrorMessage = "Last name cannot be null or empty.",
                ErrorCode = 2002
            };
        }

        var phoneValidationResult = Validator.Validate(new PhoneNumber(phoneNumber));
        if (!phoneValidationResult.IsValid)
        {
            return phoneValidationResult;
        }

        var emailValidationResult = Validator.Validate(new Email(email));
        if (!emailValidationResult.IsValid)
        {
            return emailValidationResult;
        }

        var bankAccountValidationResult = Validator.Validate(new BankAccount(bankAccount));
        if (!bankAccountValidationResult.IsValid)
        {
            return bankAccountValidationResult;
        }

        var dateOfBirthValidationResult = Validator.Validate(new DateOfBirth(dateOfBirth));
        if (!dateOfBirthValidationResult.IsValid)
        {
            return dateOfBirthValidationResult;
        }

        return new ValidationResult { IsValid = true };
    }
}