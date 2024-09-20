using Mc2.CrudTest.Presentation.Shared.Entities;
using Mc2.CrudTest.Presentation.Shared.Validators.Abstract;

namespace Mc2.CrudTest.Presentation.Shared.Factories;

public class CustomerFactory(ICustomerValidator validator): ICustomerFactory
{
    private readonly ICustomerValidator _validator = validator 
                                                     ?? throw new ArgumentNullException(nameof(validator));

    public Customer CreateCustomer(string firstName, string lastName, string phoneNumber, string email
        , string bankAccount, string dateOfBirth)
    {
        return new Customer(firstName, lastName, phoneNumber, email, bankAccount, dateOfBirth, _validator);
        
    }
}
