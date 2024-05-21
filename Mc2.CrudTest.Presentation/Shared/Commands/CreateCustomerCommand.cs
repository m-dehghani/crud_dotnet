using Mc2.CrudTest.Presentation.Shared.Entities;
using MediatR;

namespace Mc2.CrudTest.Presentation.Shared.Commands;

public class CreateCustomerCommand: IRequest, INotification
{
    public CreateCustomerCommand(string firstName, string lastName, string phoneNumber, string email, string bankAccount, string dateOfBirth)
    {
        CustomerId = new Guid();
        FirstName = firstName;
        LastName = lastName;
        PhoneNumber = phoneNumber;
        Email = email;
        BankAccount = bankAccount;
        DateOfBirth = DateTime.Parse(dateOfBirth);
    }
    public Guid CustomerId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string PhoneNumber { get; set; }
    public string Email { get; set; }
    public string BankAccount { get; set; }
    public DateTime DateOfBirth { get; set; } 
}