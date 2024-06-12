using MediatR;

namespace Mc2.CrudTest.Presentation.Shared.Commands;

public class UpdateCustomerCommand: IRequest, INotification
{
    public UpdateCustomerCommand(Guid Id, string firstName, string lastName, string phoneNumber, string email, string bankAccount, string dateOfBirth)
    {
        this.Id = Id;
        FirstName = firstName;
        LastName = lastName;
        PhoneNumber = phoneNumber;
        Email = email;
        BankAccount = bankAccount;
        DateOfBirth = dateOfBirth;
    }
    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string PhoneNumber { get; set; }
    public string Email { get; set; }
    public string BankAccount { get; set; }

    public string DateOfBirth { get; set; }
}

