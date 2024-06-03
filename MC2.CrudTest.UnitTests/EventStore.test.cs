using Mc2.CrudTest.Presentation.Infrastructure;
using Mc2.CrudTest.Presentation.Shared.Events;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Moq;

namespace MC2.CrudTest.UnitTests;
/// <summary>
/// this class just test the eventStore class withou checking for uniquness. It will be cheched by another test class
/// </summary>
public class EventStore_test
{
    [Fact]
    public async Task SaveEventAsync_Should_SaveEventAndCommitTransaction()
    {
        // Arrange
         var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "customers")
            .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))
            .Options;

        await using var context = new ApplicationDbContext(options);
        ResetDbContext(context);
        var repository = new EventStoreRepository(context, null);

        var @event = new CustomerCreatedEvent(new Guid(), "Mohammad", "Dehghani", "044 668 18 00", "a@a.com", "123464",
            DateOnly.Parse("1998-02-01"));
        Action functionToRun = () => { };

        // Act
        var exception = Record.ExceptionAsync(async () => await repository.SaveEventAsync(@event, functionToRun));
        var result = await context.Events.CountAsync();
        // Assert

        Assert.Null(exception.Exception);
        Assert.Equal(1, result);
        // Verify that the event was saved and transaction committed
    }

    [Fact]
    public async Task GetEventsAsync_Should_ReturnEventsForAggregateId()
    {
        // Arrange
        var readDbContextOptions = new DbContextOptionsBuilder<ReadModelDbContext>()
            .UseInMemoryDatabase(databaseName: "customers")
            .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))
            .Options;

        await using var readContext = new ReadModelDbContext(readDbContextOptions);
        
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "customers")
            .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))
            .Options;

        await using var context = new ApplicationDbContext(options);
        ResetDbContext(context);
        var repository = new EventStoreRepository(context, readContext);
        
        var @event = new CustomerCreatedEvent(Guid.NewGuid(), "Mohammad", "Dehghani", "044 668 18 00", "a@a.com", "123464",
            DateOnly.Parse("1998-02-01")){Data = ""};
       
        await repository.SaveEventAsync(@event, () => { });
      
        // Act
        var events = await repository.GetAllEventsAsync();

        // Assert
       
        Assert.NotEmpty(events);
    }
    private void ResetDbContext(ApplicationDbContext context)
    {
        foreach (var entity in context.Events)
            context.Events.Remove(entity);
        context.SaveChanges();
    }
}