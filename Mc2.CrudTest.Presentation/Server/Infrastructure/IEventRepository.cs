using Mc2.CrudTest.Presentation.Shared.Events;

namespace Mc2.CrudTest.Presentation.Server.Infrastructure;

public interface IEventRepository
{
    public Task SaveEventAsync(EventBase @event, Action functionToRun);
    public Task<List<EventBase>> GetEventsAsync(Guid aggregateId);
    public IQueryable<EventBase> GetAllEvents();
}