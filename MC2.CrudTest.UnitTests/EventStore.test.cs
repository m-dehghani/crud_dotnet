using Mc2.CrudTest.Presentation.Server.Infrastructure;
using Mc2.CrudTest.Presentation.Shared.Events;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace MC2.CrudTest.UnitTests;
/// <summary>
/// this class just test the eventStore class withou checking for uniquness. It will be cheched by another test class
/// </summary>
public class EventStoreTest
{
    [Fact]
    public async Task SaveEventAsync_Should_SaveEventAndCommitTransaction()
    {
        // Arrange
         DbContextOptions<ApplicationDbContext>? options = 
             new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "customers")
            .ConfigureWarnings(x => 
                x.Ignore(InMemoryEventId.TransactionIgnoredWarning))
            .Options;

        await using ApplicationDbContext? context = new ApplicationDbContext(options);
        
        ResetDbContext(context);
        
        EventStoreRepository? repository = new EventStoreRepository(context, null);

        CustomerCreatedEvent? @event = new CustomerCreatedEvent(new Guid(), "Mohammad", "Dehghani", "044 668 18 00", "a@a.com", "123464",
            DateOnly.Parse("1998-02-01"));
      
        Action functionToRun = () => { };

        // Act
        Task<Exception>? exception = Record.ExceptionAsync
            (async () => await repository.SaveEventAsync(@event, functionToRun));
      
        int result = await context.Events.CountAsync();
        // Assert

        Assert.Null(exception.Exception);
        Assert.Equal(1, result);
        // Verify that the event was saved and transaction committed
    }

    [Fact]
    public async Task GetEventsAsync_Should_ReturnEventsForAggregateId()
    {
        // Arrange
        DbContextOptions<ReadModelDbContext>? readDbContextOptions = 
            new DbContextOptionsBuilder<ReadModelDbContext>()
            .UseInMemoryDatabase(databaseName: "customers")
            .ConfigureWarnings(x => 
                x.Ignore(InMemoryEventId.TransactionIgnoredWarning))
            .Options;

        await using ReadModelDbContext? readContext = 
            new ReadModelDbContext(readDbContextOptions);
        
        DbContextOptions<ApplicationDbContext>? options = 
            new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "customers")
            .ConfigureWarnings(x => 
                x.Ignore(InMemoryEventId.TransactionIgnoredWarning))
            .Options;

        await using ApplicationDbContext? context = new ApplicationDbContext(options);
        
        ResetDbContext(context);
        
        EventStoreRepository? repository = new EventStoreRepository(context, readContext);
        
        CustomerCreatedEvent? @event = 
            new CustomerCreatedEvent(Guid.NewGuid(), 
                "Mohammad", "Dehghani", "044 668 18 00", 
                "a@a.com", "123464",
            DateOnly.Parse("1998-02-01")){Data = ""};
       
        await repository.SaveEventAsync(@event, () => { });
      
        // Act
        DbSet<EventBase>? events = context.Events;

        // Assert
       
        Assert.NotEmpty(events);
    }
    private void ResetDbContext(ApplicationDbContext context)
    {
        foreach (EventBase? entity in context.Events)
        {
            context.Events.Remove(entity);
        }
        
        context.SaveChanges();
    }
}