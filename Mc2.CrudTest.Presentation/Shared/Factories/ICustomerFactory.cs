using Mc2.CrudTest.Presentation.Shared.Entities;

namespace Mc2.CrudTest.Presentation.Shared.Factories;

public interface ICustomerFactory
{
    public Customer CreateCustomer(string firstName, string lastName, string phoneNumber, string email
        , string bankAccount, string dateOfBirth);
    
}