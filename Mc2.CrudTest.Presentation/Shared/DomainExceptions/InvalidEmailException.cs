namespace Mc2.CrudTest.Presentation.Shared.DomainExceptions;

public class InvalidEmailException : Exception
{
   public string? Email { get; }

   public InvalidEmailException()
   {
       
   }

   public InvalidEmailException(string message)
      : base(message)
   {
   
   }

   public InvalidEmailException(string message, Exception inner)
      : base(message, inner)
   {
     
   }

   public InvalidEmailException(string message, string email)
      : this(message) =>
      Email = email;
}