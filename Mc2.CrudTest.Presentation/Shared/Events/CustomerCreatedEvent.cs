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
    
    public Guid Id { get; set;}
    public string FirstName { get;  set;}
    public string LastName { get; set;}
    public string PhoneNumber { get; set;}
    public string Email { get;set;}
    public string BankAccount { get; set;}
    public DateTime DateOfBirth { get; set; }

    
}

