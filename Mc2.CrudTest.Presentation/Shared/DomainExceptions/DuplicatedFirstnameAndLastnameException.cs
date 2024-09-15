namespace Mc2.CrudTest.Presentation.Shared.DomainExceptions;

public class DuplicatedFirstnameAndLastnameException(string message) : Exception(message)
{
}