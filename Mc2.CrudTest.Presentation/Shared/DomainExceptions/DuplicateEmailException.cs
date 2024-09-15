namespace Mc2.CrudTest.Presentation.Shared.DomainExceptions;

public class DuplicateEmailException :  Exception
{
    public string ErrorCode => "201";
    private DuplicateEmailException(string message)
        : base(message)
    {
   
    }

    public DuplicateEmailException(string message, string email)
        : this(message)
    {
    }
}