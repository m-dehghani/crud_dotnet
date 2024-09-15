namespace Mc2.CrudTest.Presentation.Shared.DomainExceptions;

public class InvalidPhonenumberException : Exception
{
    public string? Phonenumber { get; }

    public InvalidPhonenumberException()
    {
       
    }

    public InvalidPhonenumberException(string message)
        : base(message)
    {
   
    }

    public InvalidPhonenumberException(string message, Exception inner)
        : base(message, inner)
    {
     
    }

    public InvalidPhonenumberException(string message, string phonenumber)
        : this(message) =>
        Phonenumber = phonenumber;
}