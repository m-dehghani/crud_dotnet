namespace Mc2.CrudTest.Presentation.Shared.DomainExceptions;

public class DuplicateEmailException :  Exception
{
    public string? Email { get; }

    public DuplicateEmailException()
    {
       
    }

    public DuplicateEmailException(string message)
        : base(message)
    {
   
    }

    public DuplicateEmailException(string message, Exception inner)
        : base(message, inner)
    {
     
    }

    public DuplicateEmailException(string message, string email)
        : this(message) =>
        Email = email;
}