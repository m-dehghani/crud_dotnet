using Mc2.CrudTest.Presentation.Shared.ValueObjects;
using System.Text.Json.Serialization;
using Mc2.CrudTest.Presentation.Shared.Validators.Abstract;
using Mc2.CrudTest.Presentation.Shared.Validators.Concrete;

namespace Mc2.CrudTest.Presentation.Shared.Entities
{
    public class Customer
    {
        public Guid Id { get; set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public BankAccount BankAccount { get; private set; }
        public DateOfBirth DateOfBirth { get; private set; }
        public PhoneNumber PhoneNumber { get; private set; }
        public Email Email { get; private set; }
        private int Version { get; set; } = 0;
        public bool IsDeleted { get; private set; }
        
        private readonly ICustomerValidator _validator;
        
        [JsonIgnore]
        public List<string> History { get; private set; } = [];

        
        // Constructor for rehydration(Deserialisation)
        public Customer(Guid id, string firstName, string lastName, 
            PhoneNumber phoneNumber, Email email, BankAccount bankAccount, 
            DateOfBirth dateOfBirth)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            PhoneNumber = phoneNumber;
            Email = email;
            BankAccount = bankAccount;
            DateOfBirth = dateOfBirth;
            History = [];
            IsDeleted = false;
        }

        [JsonConstructor]
        public Customer(Guid id, string firstName, string lastName, 
            PhoneNumber phoneNumber, Email email, BankAccount bankAccount, 
            DateOfBirth dateOfBirth, string[] history,  int version = 0, 
            bool isDeleted = false)
        {
            Id = id;
            
            FirstName = firstName;
            
            LastName = lastName;
            
            PhoneNumber = phoneNumber;
            
            Email = email;
            
            BankAccount = bankAccount;
            
            DateOfBirth = dateOfBirth;
            
            History = [];
            
            IsDeleted = IsDeleted;
        }

        public Dictionary<string, string> Errors =new Dictionary<string, string>();

        
        public Customer(string firstName, string lastName, 
            string phoneNumber, string email, string bankAccount, 
            string dateOfBirth, ICustomerValidator validator)
        {
            _validator = validator ?? throw new ArgumentNullException(nameof(validator));

            var validationResult = _validator.ValidateCustomer(firstName, lastName, phoneNumber, email, bankAccount, dateOfBirth);

            if (!validationResult.IsValid)
            {
                throw new ArgumentException(validationResult.ErrorMessage);
            }
            
            FirstName = firstName;
            LastName = lastName;
            PhoneNumber = new PhoneNumber(phoneNumber);
            Email = new Email(email);
            BankAccount = new BankAccount(bankAccount);
            DateOfBirth = new DateOfBirth(dateOfBirth);
            History = new List<string>();
            IsDeleted = false;
        }

        public Customer()
        {
            
        }

        // Method to apply events generically
        public void Apply(object @event)
        {
            ((dynamic)this).Apply((dynamic)@event);
            
            Version += 1;
        }
        
        public ValidationResult Validate()
        {
            return _validator.ValidateCustomer(FirstName, LastName, PhoneNumber.Value, Email.Value, BankAccount.Value, DateOfBirth.Value?.ToString());
        }
    }
}