using MediatR;

namespace Mc2.CrudTest.Presentation.Shared.Events;

public class CustomerCreatedEvent: EventBase, INotification
{
    public CustomerCreatedEvent(Guid id, string firstName, string lastName, string phoneNumber, string email, string bankAccount, DateOnly dateOfBirth, DateTimeOffset? occurredOn = null)
    {
        FirstName = firstName;
        LastName = lastName;
        PhoneNumber = phoneNumber;
        Email = email;
        BankAccount = bankAccount;
        DateOfBirth = dateOfBirth;
        EventId = new Guid();
        AggregateId = id;
        if(occurredOn.HasValue)
        {
            OccurredOn = occurredOn.Value;
        }

    }
   
    public CustomerCreatedEvent(){}
   

    
}

