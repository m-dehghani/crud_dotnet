using System.Data.SqlTypes;
using System.Text.Json.Serialization;
using Mc2.CrudTest.Presentation.Shared.Entities;
using MediatR;

namespace Mc2.CrudTest.Presentation.Shared.Commands;

public class CreateCustomerCommand(
    string firstName,
    string lastName,
    string phoneNumber,
    string email,
    string bankAccount,
    string dateOfBirth)
    : IRequest, INotification
{
    public Guid CustomerId { get; set; } = Guid.NewGuid();
    public string FirstName { get; set; } = firstName;
    public string LastName { get; set; } = lastName;
    public string PhoneNumber { get; set; } = phoneNumber;
    public string Email { get; set; } = email;
    public string BankAccount { get; set; } = bankAccount;
    public string DateOfBirth { get; set; } = dateOfBirth;
}