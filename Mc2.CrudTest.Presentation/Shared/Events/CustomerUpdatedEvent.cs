namespace Mc2.CrudTest.Presentation.Shared.Events;

public class CustomerUpdatedEvent: EventBase
{
    public Guid CustomerId { get; }
    public string FirstName { get; }
    public string LastName { get; }
    public string PhoneNumber { get; }
    public string BankAccount { get; }
    public string Email { get; }
    public DateTime  DateOfBirth { get; set; }

    public CustomerUpdatedEvent(Guid customerId, string firstName, string lastName, string email, string phoneNumber, string bankAccount,  DateTime dateOfBirth)
    {
        CustomerId = customerId;
        FirstName = firstName;
        LastName = lastName;
        PhoneNumber = phoneNumber;
        Email = email;
        BankAccount = bankAccount;
        CreatedAt = DateTime.Now;
        DateOfBirth = dateOfBirth;
    }
    
    public CustomerUpdatedEvent(){}
}