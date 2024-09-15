namespace Mc2.CrudTest.Presentation.Shared.DomainExceptions;

public class InvalidPhonenumberException : Exception
{
    public string? Phonenumber { get; }

    public string ErrorCode => "102";


    private InvalidPhonenumberException(string message)
        : base(message)
    {
   
    }
    

    public InvalidPhonenumberException(string message, string phoneNumber)
        : this(message) =>
        Phonenumber = phoneNumber;
}