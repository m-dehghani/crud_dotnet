namespace Mc2.CrudTest.Presentation.ViewModels;

public class CustomerUpdateViewModel
{
        public CustomerUpdateViewModel(Guid id,string firstName, string lastName, string phoneNumber,  string email, string bankAccount, string dateOfBirth)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            BankAccount = bankAccount;
            DateOfBirth = dateOfBirth;
            PhoneNumber = phoneNumber;
            Email = email;
        }

        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string BankAccount { get; set; }
        public string DateOfBirth { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
}