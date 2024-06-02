using Mc2.CrudTest.Presentation.Shared.Events;
using Mc2.CrudTest.Presentation.Shared.ValueObjects;
using System.Diagnostics;

namespace Mc2.CrudTest.Presentation.Shared.Entities
{
    public class Customer
    {
        public Guid Id { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public BankAccount BankAccount { get; private set; }
        public DateOfBirth DateOfBirth { get; private set; }
        public PhoneNumber PhoneNumber { get; private set; }
        public Email Email { get; private set; }
        public int Version { get; private set; } = 0;
        public bool IsDeleted { get; private set; }

        // Constructor for rehydration
        public Customer(Guid id, string firstName, string lastName, string phoneNumber, string email, string bankAccount, string dateOfBirth)
        {
            Id = id;
            FirstName = firstName ?? throw new ArgumentNullException(nameof(firstName));
            LastName = lastName ?? throw new ArgumentNullException(nameof(lastName));
            PhoneNumber = new PhoneNumber(phoneNumber) ?? throw new ArgumentNullException(nameof(phoneNumber));
            Email = new Email(email) ?? throw new ArgumentNullException(nameof(email));
            BankAccount = new BankAccount(bankAccount) ?? throw new ArgumentException(bankAccount);
            DateOfBirth = new DateOfBirth(dateOfBirth);
            IsDeleted = false;
        }


        public Customer(string firstName, string lastName, string phoneNumber, string email, string bankAccount, string dateOfBirth)
        {
            FirstName = firstName ?? throw new ArgumentNullException(nameof(firstName));
            LastName = lastName ?? throw new ArgumentNullException(nameof(lastName));
            PhoneNumber = new PhoneNumber(phoneNumber) ?? throw new ArgumentNullException(nameof(phoneNumber));
            Email = new Email(email) ?? throw new ArgumentNullException(nameof(email));
            BankAccount = new BankAccount(bankAccount) ?? throw new ArgumentException(bankAccount);
            DateOfBirth = new DateOfBirth(dateOfBirth);
            IsDeleted = false;
        }

        public Customer()
        {

        }

        // Apply methods
        protected void Apply(CustomerCreatedEvent @event)
        {
            Id = @event.AggregateId;
            FirstName = @event.FirstName;
            LastName = @event.LastName;
            Email = new Email(@event.Email);
            PhoneNumber = ParsePhone(@event.PhoneNumber);
            BankAccount = new BankAccount(@event.BankAccount);
            Version = 0;
        }

        private PhoneNumber ParsePhone(string phoneNumber)
        {
            try
            {
                return new PhoneNumber(phoneNumber);
            }
            catch (Exception)
            {
               
                Debug.WriteLine($"An error has been occured while reading the phoneNumber of the customer. Bad format. CustomerId: {Id}");
                
                //TODO: Change this phone into a default valid phone number
                return new PhoneNumber("+989010596159");
            }
        }

        protected void Apply(CustomerUpdatedEvent @event)
        {
            FirstName = @event.FirstName;
            LastName = @event.LastName;
            Email = new Email(@event.Email);
            PhoneNumber = ParsePhone(@event.PhoneNumber);
            BankAccount = new BankAccount(@event.BankAccount);
            Id = @event.AggregateId;
            Version += 1;
        }

        protected void Apply(CustomerDeletedEvent @event)
        {
            Id = @event.AggregateId;
            IsDeleted = true;
            Version += 1;
        }

        // Method to apply events generically
        public void Apply(object @event)
        {
            ((dynamic)this).Apply((dynamic)@event);
            Version += 1;
        }

    }
}