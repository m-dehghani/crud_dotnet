namespace Mc2.CrudTest.Presentation.Shared.Events;

public class CustomerDeletedEvent: EventBase
{
    public Guid AggregateId { get;set; }

    public CustomerDeletedEvent(Guid aggregateId)
    {
        AggregateId = aggregateId;
    }
    
}