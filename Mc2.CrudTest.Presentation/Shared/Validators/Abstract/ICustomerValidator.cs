using Mc2.CrudTest.Presentation.Shared.Validators.Concrete;

namespace Mc2.CrudTest.Presentation.Shared.Validators.Abstract;

public interface ICustomerValidator
{
    ValidationResult ValidateCustomer(string firstName, string lastName, string phoneNumber, string email, string bankAccount, string dateOfBirth);
}