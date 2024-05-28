using Mc2.CrudTest.Presentation.Shared.Events;
using Mc2.CrudTest.Presentation.Shared.ReadModels;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

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
        var trx = await _context.Database.BeginTransactionAsync();
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
            throw ex;
        }
    }
    
    public async Task<List< EventBase>> GetEventsAsync(Guid aggregateId)
    {
        // Retrieve events for the specified aggregate ID
        return await _context.Events
            .Where(e => e.AggregateId == aggregateId)
            .OrderBy(e => e.OccurredOn)
            .ToListAsync();
    }
    public async Task<List<EventBase>> GetAllEventsAsync()
    {
        // Retrieve events for the specified aggregate ID
        return await _context.Events
            .OrderBy(e => e.OccurredOn)
            .ToListAsync();
    }
    
}



