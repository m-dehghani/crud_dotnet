namespace Mc2.CrudTest.Presentation.Shared.DomainExceptions;

public class InvalidBankAccountNumberException(string message) : Exception(message)
{
    public string? BankAccount { get; }

    public string ErrorCode => "103";

    public InvalidBankAccountNumberException(string message, string bankAccount)
        : this(message) =>
        BankAccount = bankAccount;
}