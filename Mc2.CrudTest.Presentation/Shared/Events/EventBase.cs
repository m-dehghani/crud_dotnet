namespace Mc2.CrudTest.Presentation.Shared.Events;

public class EventBase
{
    public Guid AggregateId { get; set; }

    public DateTimeOffset OccurredOn { get; set; }
    public Guid EventId { get; set; }
    public string? Data { get; set; }
    // public Guid Id { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Email { get; set; }
    public string? BankAccount { get; set; }
    public DateOnly DateOfBirth { get; set; }
}