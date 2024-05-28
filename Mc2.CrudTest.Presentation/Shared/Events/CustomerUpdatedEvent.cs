namespace Mc2.CrudTest.Presentation.Shared.Events;

public class CustomerUpdatedEvent: EventBase
{
    public Guid CustomerId { get; set;}
    public string FirstName { get; set;}
    public string LastName { get; set;}
    public string PhoneNumber { get; set;}
    public string BankAccount { get; set;}
    public string Email { get; set;}
    public DateTime DateOfBirth { get; set; }

    public CustomerUpdatedEvent(Guid customerId, string firstName, string lastName, string email, string phoneNumber, string bankAccount, DateTime dateOfBirth)
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