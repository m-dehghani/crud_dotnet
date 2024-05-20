namespace Mc2.CrudTest.Presentation.ViewModels;

public class CustomerViewModel
{
    public CustomerViewModel(string firstName, string lastName, string phoneNumber,  string email, string bankAccount, string dateOfBirth)
    {
        this.FirstName = firstName;
        this.LastName = lastName;
        this.BankAccount = bankAccount;
        this.DateOfBirth = dateOfBirth;
        this.PhoneNumber = phoneNumber;
        this.Email = email;
    }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string BankAccount { get; set; }
    public string DateOfBirth { get; set; }
    public string PhoneNumber { get; set; }
    public string Email { get; set; }
}