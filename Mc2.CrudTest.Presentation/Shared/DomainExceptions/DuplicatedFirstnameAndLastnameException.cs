namespace Mc2.CrudTest.Presentation.Shared.DomainExceptions;

public class DuplicatedFirstnameAndLastnameExceptionException : Exception
{
    public string? FirstName { get; }
    public string? LastName { get; }

    public DuplicatedFirstnameAndLastnameExceptionException()
    {
       
    }

    public DuplicatedFirstnameAndLastnameExceptionException(string message)
        : base(message)
    {
   
    }

    public DuplicatedFirstnameAndLastnameExceptionException(string message, Exception inner)
        : base(message, inner)
    {
     
    }

    public DuplicatedFirstnameAndLastnameExceptionException(string message, string firstName, string lastName)
        : this(message)
    {

        FirstName = firstName;
        LastName = lastName;
    }
}