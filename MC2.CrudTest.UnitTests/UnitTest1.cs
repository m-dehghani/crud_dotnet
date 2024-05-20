using Mc2.CrudTest.Presentation.Shared.Entities;
using Mc2.CrudTest.Presentation.Shared.Events;

namespace MC2.CrudTest.UnitTests;

public class UnitTest1
{
    [Fact]
    public void Apply_CustomerDeletedEvent_SetsDeletedFlag()
    {
        // Arrange
        // var customer = new Customer( "Mohammad", "Dehghani", "044 668 18 00", "a@gmail.com", "123456");
        // var deletedEvent = new CustomerDeletedEvent(customer.Id);
        //
        // // Act
        // customer.Apply(deletedEvent);
        //
        // // Assert
        // Assert.True(customer.IsDeleted);
    }
    [Fact]
    public void Apply_CustomerCreatedEvent_AppliesCorrectly()
    {
        // Arrange
        var customerCreatedEvent = new CustomerCreatedEvent
        (
             Guid.NewGuid(),
             "Mohammad",
             "Dehghani",
             "044 668 18 00",
             "a@gmail.com",
             "1234564",
             DateTime.Parse("2011-04-09")
        );

        // Act
        var customer = new Customer();
        customer.Apply(customerCreatedEvent);

        // Assert
        Assert.Equal("Mohammad", customer.FirstName.ToString());
        Assert.Equal("Dehghani", customer.LastName.ToString());
        Assert.Equal("a@gmail.com", customer.Email.ToString()); 
    }
}