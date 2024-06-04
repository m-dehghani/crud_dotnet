namespace Mc2.CrudTest.Presentation.Shared.Events;

public class CustomerUpdatedEvent: EventBase
{
   
    public CustomerUpdatedEvent(Guid customerId, string firstName, string lastName, string email, string phoneNumber, string bankAccount, DateOnly dateOfBirth)
    {
        AggregateId = customerId;
        FirstName = firstName;
        LastName = lastName;
        PhoneNumber = phoneNumber;
        Email = email;
        BankAccount = bankAccount;
      
        
        DateOfBirth = dateOfBirth;
    }
    
    public CustomerUpdatedEvent(){}
}