namespace Mc2.CrudTest.Presentation.Shared.Events;

public class CustomerDeletedEvent: EventBase
{
      
    public CustomerDeletedEvent(Guid aggregateId)
    {
        
        AggregateId = aggregateId;
        OccurredOn = DateTime.Now;
    }
    
}