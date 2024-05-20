using MediatR;

namespace Mc2.CrudTest.Presentation.Shared.Events;

public class CustomerCreatedEvent: EventBase, INotification
{
    public CustomerCreatedEvent(Guid id, string firstName, string lastName, string phoneNumber, string email, string bankAccount, DateTime dateOfBirth)
    {
        Id = id;
        FirstName = firstName;
        LastName = lastName;
        PhoneNumber = phoneNumber;
        Email = email;
        BankAccount = bankAccount;
        DateOfBirth = dateOfBirth;
        EventId = new Guid();
    }
    public CustomerCreatedEvent(){}
    
    public Guid Id { get; }
    public string FirstName { get;  }
    public string LastName { get; }
    public string PhoneNumber { get; }
    public string Email { get;}
    public string BankAccount { get; }
    public DateTime DateOfBirth { get; set; }

    
}

