using Mc2.CrudTest.Presentation.Shared.Events;
using Mc2.CrudTest.Presentation.Shared.ValueObjects;
using System.Text.Json.Serialization;
using System.Diagnostics;
using System.Text.Json;


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
        
        [JsonIgnore]
        public List<string> History { get; private set; } = new();

        
        // Constructor for rehydration(Deserialisation)
        public Customer(Guid id, string firstName, string lastName, PhoneNumber phoneNumber, Email email, BankAccount bankAccount, DateOfBirth dateOfBirth)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            PhoneNumber = phoneNumber;
            Email = email;
            BankAccount = bankAccount;
            DateOfBirth = dateOfBirth;
            History = new();
            IsDeleted = false;
        }

        [JsonConstructor]
        public Customer(Guid id, string firstName, string lastName, PhoneNumber phoneNumber, Email email, BankAccount bankAccount, DateOfBirth dateOfBirth, string[] history,  int version = 0, bool isDeleted = false)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            PhoneNumber = phoneNumber;
            Email = email;
            BankAccount = bankAccount;
            DateOfBirth = dateOfBirth;
            History = new();
            IsDeleted = IsDeleted;
        }

        public Customer(string firstName, string lastName, string phoneNumber, string email, string bankAccount, string dateOfBirth)
        {
            FirstName = firstName ?? throw new ArgumentNullException(nameof(firstName));
            LastName = lastName ?? throw new ArgumentNullException(nameof(lastName));
            PhoneNumber = new PhoneNumber(phoneNumber) ?? throw new ArgumentNullException(nameof(phoneNumber));
            Email = new Email(email) ?? throw new ArgumentNullException(nameof(email));
            BankAccount = new BankAccount(bankAccount) ?? throw new ArgumentException(bankAccount);
            DateOfBirth = new DateOfBirth(dateOfBirth);
            History = new();
            IsDeleted = false;
        }

        public Customer()
        {

        }

        // Apply methods
        protected void Apply(CustomerCreatedEvent @event)
        {
            try
            {
                Id = @event.AggregateId;
                FirstName = @event.FirstName;
                LastName = @event.LastName;
                Email = new Email(@event.Email);
                PhoneNumber = new PhoneNumber(@event.PhoneNumber);
                BankAccount = new BankAccount(@event.BankAccount);
                DateOfBirth = new DateOfBirth(@event.DateOfBirth.ToString());
                History.Add($"Created at {@event.OccurredOn}");
                Version = 0;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }


        protected void Apply(CustomerUpdatedEvent @event)
        {
            Id = @event.AggregateId;
            FirstName = @event.FirstName;
            LastName = @event.LastName;
            Email = new Email(@event.Email);
            PhoneNumber = new PhoneNumber(@event.PhoneNumber);
            BankAccount = new BankAccount(@event.BankAccount);
            DateOfBirth = new DateOfBirth(@event.DateOfBirth.ToString());
            History.Add($"Updated at {@event.OccurredOn}");
            Version += 1;
        }

        protected void Apply(CustomerDeletedEvent @event)
        {
            Id = @event.AggregateId;
            IsDeleted = true;
            History.Add($"Deleted at {@event.OccurredOn}");
            Version += 1;
        }

        // Method to apply events generically
        public void Apply(object @event)
        {
            ((dynamic)this).Apply((dynamic)@event);
            Version += 1;
        }
    }

    public class Test
    {

        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public BankAccount BankAccount { get; set; }
        public DateOfBirth DateOfBirth { get; set; }
        public PhoneNumber PhoneNumber { get; set; }
        public Email Email { get; set; }
        public int Version { get; set; }
        public bool IsDeleted { get; set; }
        public List<object> History { get; set; }
    }
}