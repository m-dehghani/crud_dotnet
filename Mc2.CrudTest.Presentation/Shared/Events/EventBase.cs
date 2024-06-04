using System.ComponentModel.DataAnnotations.Schema;

namespace Mc2.CrudTest.Presentation.Shared.Events;

public class EventBase
{
    public Guid AggregateId { get; set; }

    public DateTimeOffset OccurredOn { get; set; }

    public Guid EventId { get; set; }

    public string? Data { get; set; } 

    [NotMapped]
    public string? FirstName { get; set; }

    [NotMapped]
    public string? LastName { get; set; }

    [NotMapped]
    public string? PhoneNumber { get; set; }

    [NotMapped]
    public string? Email { get; set; }

    [NotMapped]
    public string? BankAccount { get; set; }

    [NotMapped]
    public DateOnly DateOfBirth { get; set; }
}