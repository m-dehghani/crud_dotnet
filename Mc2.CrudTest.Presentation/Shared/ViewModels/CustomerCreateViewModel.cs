namespace Mc2.CrudTest.Presentation.Shared.ViewModels
{
    internal class CustomerCreateViewModel : BaseViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string BankAccount { get; set; }
        public DateOnly DateOfBirth { get; set; } = DateOnly.Parse("1900-01-01");
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
    }
}
