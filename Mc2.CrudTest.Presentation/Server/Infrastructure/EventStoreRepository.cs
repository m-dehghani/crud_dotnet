using Mc2.CrudTest.Presentation.Shared.Events;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Mc2.CrudTest.Presentation.Infrastructure;

public class EventStoreRepository: IEventRepository
{
    private readonly ApplicationDbContext _context;
    //TODO: map the _readcontext to a view in another db syncing with the main db
    private readonly ReadModelDbContext _readContext;
    public EventStoreRepository(ApplicationDbContext context, ReadModelDbContext readModelDbContext)
    {
        _context = context;
        _readContext = readModelDbContext;
    }

    public async Task SaveEventAsync(EventBase @event, Action functionToRun) 
    {
        IExecutionStrategy? executionStrategy = _context.Database.CreateExecutionStrategy();

        await executionStrategy.Execute(
           async () =>
            {
                IDbContextTransaction? trx = await _context.Database.BeginTransactionAsync();
                try
                {
                    _context.Events.Add(@event);
                    await _context.SaveChangesAsync();

                    functionToRun();

                    await trx.CommitAsync();
                }
                catch (Exception ex)
                {
                    await trx.RollbackAsync();
                    throw;
                }
            });
    }
    
    public async Task<List<EventBase>> GetEventsAsync(Guid aggregateId)
    {
        // Retrieve events for the specified aggregate ID
        return await _readContext.CustomerEvents
            .Where(e => e.AggregateId == aggregateId)
            .OrderBy(e => e.OccurredOn)
            .ToListAsync();
    }
    public IQueryable<EventBase> GetAllEvents()
    {
        // Retrieve events for All the aggregates
        return _readContext.CustomerEvents
            .OrderBy(e => e.OccurredOn)
            .AsQueryable();
    }
    
}



