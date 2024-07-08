using MediatR;

namespace Mc2.CrudTest.Presentation.Shared.Commands;

public class UpdateCustomerCommand: IRequest, INotification
{
    public UpdateCustomerCommand(Guid customerId, string firstName, string lastName, string phoneNumber, string email, string bankAccount, string dateOfBirth)
    {
        CustomerId = customerId;
        FirstName = firstName;
        LastName = lastName;
        PhoneNumber = phoneNumber;
        Email = email;
        BankAccount = bankAccount;
        DateOfBirth = dateOfBirth;
    }
    public Guid CustomerId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string PhoneNumber { get; set; }
    public string Email { get; set; }
    public string BankAccount { get; set; }

    public string DateOfBirth { get; set; }
}

