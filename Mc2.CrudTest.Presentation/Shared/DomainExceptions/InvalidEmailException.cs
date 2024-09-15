namespace Mc2.CrudTest.Presentation.Shared.DomainExceptions;

public class InvalidEmailException(string message) : Exception(message)
{
   public string? Email { get; }
   public string ErrorCode => "101";

   public InvalidEmailException(string message, string email)
      : this(message) =>
      Email = email;
}