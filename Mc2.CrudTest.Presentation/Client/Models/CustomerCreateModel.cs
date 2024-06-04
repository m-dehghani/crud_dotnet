namespace Mc2.CrudTest.Presentation.Client.Models
{
    public class CustomerCreateModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string BankAccount { get; set; }
        public DateOnly DateOfBirth { get; set; } = DateOnly.Parse("1900-01-01");
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string[] History { get; set; } = [];
    }
}
