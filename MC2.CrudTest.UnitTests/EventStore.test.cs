using Mc2.CrudTest.Presentation.Infrastructure;
using Mc2.CrudTest.Presentation.Shared.Events;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Moq;

namespace MC2.CrudTest.UnitTests;

public class EventStore_test
{
    [Fact]
    public async Task SaveEventAsync_Should_SaveEventAndCommitTransaction()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDb")
            .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))
            .Options;

        await using var context = new ApplicationDbContext(options);
        var repository = new EventStoreRepository(context, null);

        var @event = new CustomerCreatedEvent(new Guid(), "Mohammad", "Dehghani", "044 668 18 00", "a@a.com", "123464",
            DateTime.Parse("1998-02-01"));
        Action functionToRun = () => { };

        // Act
        var exception = Record.ExceptionAsync(async () => await repository.SaveEventAsync(@event, functionToRun));
        var result = await context.Events.CountAsync();
        // Assert

        Assert.Null(exception);
        Assert.Equal(1, result);
        // Verify that the event was saved and transaction committed
        // You can check the in-memory database or mock the context
    }

    [Fact]
    public async Task GetEventsAsync_Should_ReturnEventsForAggregateId()
    {
        // Arrange
        var readDbContextOptions = new DbContextOptionsBuilder<ReadModelDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDb")
            .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))
            .Options;

        await using var readContext = new ReadModelDbContext(readDbContextOptions);
        
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDb")
            .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))
            .Options;

        await using var context = new ApplicationDbContext(options);
      
        var repository = new EventStoreRepository(context, readContext);
        
        var @event = new CustomerCreatedEvent(Guid.NewGuid(), "Mohammad", "Dehghani", "044 668 18 00", "a@a.com", "123464",
            DateTime.Parse("1998-02-01")){Data = ""};
       
        await repository.SaveEventAsync(@event, () => { });
      
        // Act
        var events = await repository.GetAllEventsAsync();

        // Assert
       
        Assert.NotNull(events);
    }
}