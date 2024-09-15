namespace Mc2.CrudTest.Presentation.Shared.DomainExceptions;

public class InvalidDateOfBirthException(string message) : Exception(message)
{
    public string? DateOfBirth { get; }
    public string ErrorCode => "104";

    public InvalidDateOfBirthException(string message, string dateOfBirth)
        : this(message) =>
        DateOfBirth = dateOfBirth;
    
}