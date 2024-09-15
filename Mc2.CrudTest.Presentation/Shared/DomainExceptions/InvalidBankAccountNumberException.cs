namespace Mc2.CrudTest.Presentation.Shared.DomainExceptions;

public class InvalidBankAccountNumberException : Exception
{
    public string? BankAccount { get; }

    public InvalidBankAccountNumberException()
    {
       
    }

    public InvalidBankAccountNumberException(string message)
        : base(message)
    {
   
    }

    public InvalidBankAccountNumberException(string message, Exception inner)
        : base(message, inner)
    {
     
    }

    public InvalidBankAccountNumberException(string message, string bankAccount)
        : this(message) =>
        BankAccount = bankAccount;
}