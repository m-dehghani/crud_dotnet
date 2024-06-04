using Mc2.CrudTest.Presentation.Shared.Events;
using Mc2.CrudTest.Presentation.Shared.ValueObjects;
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
        public List<string> History { get; private set; } = new();

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
            History = new();
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
                
                //var options = new JsonSerializerOptions() { IncludeFields = true,IgnoreReadOnlyFields = true, IgnoreReadOnlyProperties = true };
                var customer = JsonSerializer.Deserialize<Test>(@event.Data);// json = null;

                //var test = JsonConvert.DeserializeObject<Test>(@event.Data);
                //var customer = JsonConvert.DeserializeObject<Customer>(@event.Data);
                Id = @event.AggregateId;
                FirstName = customer.FirstName;
                LastName = customer.LastName;
                Email = customer.Email;
                PhoneNumber = customer.PhoneNumber;
                BankAccount = customer.BankAccount;
                DateOfBirth = customer.DateOfBirth;
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
            //var customer =  JsonConvert.DeserializeObject<Customer>(@event.Data);
            var customer = JsonSerializer.Deserialize<Test>(@event.Data); 
            FirstName = customer.FirstName;
            LastName = customer.LastName;
            Email = customer.Email;
            PhoneNumber = customer.PhoneNumber;
            BankAccount = customer.BankAccount;
            DateOfBirth = customer.DateOfBirth;
            Id = @event.AggregateId;
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
        public Test()
        {
            
        }

    }
}