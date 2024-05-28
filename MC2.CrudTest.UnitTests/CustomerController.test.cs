using Mc2.CrudTest.Presentation.DomainServices;
using Mc2.CrudTest.Presentation.Server.Controllers;
using Mc2.CrudTest.Presentation.Shared.Entities;
using Moq;
using Mc2.CrudTest.Presentation;
using Mc2.CrudTest.Presentation.Infrastructure;
using Mc2.CrudTest.Presentation.Shared.Commands;
using Mc2.CrudTest.Presentation.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using StackExchange.Redis;
using Mc2.CrudTest.Presentation.Handlers;

namespace MC2.CrudTest.UnitTests
{
    
    public class CustomerControllerTests: IClassFixture<WebApplicationFactory<Program>>
    {
        private ICustomerService _customerService;
        private Mock<IMediator> _mediator;
        private ApplicationDbContext context;
        private RedisCacheHandler cacheHandler;
        private Mock<IDatabase> mockDatabase;
        private CustomerViewModel newCustomer;
        private CreateCustomerEventHandler createHandler;
        public CustomerControllerTests(WebApplicationFactory<Program> factory)
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "customers")
                .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                .Options;
               
            context = new ApplicationDbContext(options);
            _mediator = new  Mock<IMediator>();
            var mockMultiplexer = new Mock<IConnectionMultiplexer>();
            mockMultiplexer.Setup(_ => _.IsConnected).Returns(false);
            mockDatabase = new Mock<IDatabase>();
            
            mockMultiplexer
                .Setup(_ => _.GetDatabase(It.IsAny<int>(), It.IsAny<object>()))
                .Returns(mockDatabase.Object);

            cacheHandler = new RedisCacheHandler(mockMultiplexer.Object);
            _customerService = new CustomerService(new EventStoreRepository(context, null), mockDatabase.Object);

            newCustomer = new CustomerViewModel("Mohammadd", "Dehghaniii", "+989010596159", "dehghany.m@gmail.com",
               "1231564654", "2015-02-04");
            
            createHandler = new CreateCustomerEventHandler(_customerService);
        }

        [Fact]
        public async Task CreateCustomer_DuplicateEmail_throws_exception()
        {
            // Arrange

            await createHandler.Handle(new CreateCustomerCommand(newCustomer.FirstName, newCustomer.LastName, newCustomer.PhoneNumber, newCustomer.Email, newCustomer.BankAccount, newCustomer.DateOfBirth), CancellationToken.None);
                       
            cacheHandler.SetCustomerData(new Customer(newCustomer.FirstName, newCustomer.LastName, newCustomer.PhoneNumber, newCustomer.Email, newCustomer.BankAccount, newCustomer.DateOfBirth));

            mockDatabase.Setup((x) => x.StringGet(newCustomer.Email, It.IsAny<CommandFlags>())).Returns(() => "true");

            mockDatabase.Setup((x) => x.StringGet($"{newCustomer.FirstName}-{newCustomer.LastName}-{DateTime.Parse(newCustomer.DateOfBirth)}", It.IsAny<CommandFlags>())).Returns(() => "true");

            await Assert.ThrowsAsync<ArgumentException>(async() => await createHandler.Handle(new CreateCustomerCommand(newCustomer.FirstName, newCustomer.LastName, newCustomer.PhoneNumber, newCustomer.Email, newCustomer.BankAccount, newCustomer.DateOfBirth), CancellationToken.None));
                       
        }
      

        [Fact]
        public async Task NewCustomer_CustomerDataIsNotUnique_EmailIsUnique_ShouldFail()
        {
           
            await createHandler.Handle(new CreateCustomerCommand(newCustomer.FirstName, newCustomer.LastName, newCustomer.PhoneNumber, newCustomer.Email, newCustomer.BankAccount, newCustomer.DateOfBirth), CancellationToken.None);

            cacheHandler.SetCustomerData(new Customer(newCustomer.FirstName, newCustomer.LastName, newCustomer.PhoneNumber, newCustomer.Email, newCustomer.BankAccount, newCustomer.DateOfBirth));

            mockDatabase.Setup((x) => x.StringGet(newCustomer.Email, It.IsAny<CommandFlags>())).Returns(() => "true");
            mockDatabase.Setup((x) => x.StringGet($"{newCustomer.FirstName}-{newCustomer.LastName}-{DateTime.Parse(newCustomer.DateOfBirth)}", It.IsAny<CommandFlags>())).Returns(() => "true");

            var anotherCustomer = new CustomerViewModel("Mohammadd", "Dehghaniii", "+989010596159", "a@a.com",
               "1231564654", "2015-02-04");

            // Assert
            await Assert.ThrowsAsync<ArgumentException>(async ()=> await createHandler.Handle(new CreateCustomerCommand(anotherCustomer.FirstName, anotherCustomer.LastName, anotherCustomer.PhoneNumber, anotherCustomer.Email, anotherCustomer.BankAccount, anotherCustomer.DateOfBirth), CancellationToken.None));
        
        }

        [Fact]
        public async Task Both_EmailIAndCustomerDataIsUnique_ShouldPass()
        {
           
            await createHandler.Handle(new CreateCustomerCommand(newCustomer.FirstName, newCustomer.LastName, newCustomer.PhoneNumber, newCustomer.Email, newCustomer.BankAccount, newCustomer.DateOfBirth), CancellationToken.None);

            cacheHandler.SetCustomerData(new Customer(newCustomer.FirstName, newCustomer.LastName, newCustomer.PhoneNumber, newCustomer.Email, newCustomer.BankAccount, newCustomer.DateOfBirth));

            mockDatabase.Setup((x) => x.StringGet(newCustomer.Email, It.IsAny<CommandFlags>())).Returns(() => "true");
            mockDatabase.Setup((x) => x.StringGet($"{newCustomer.FirstName}-{newCustomer.LastName}-{DateTime.Parse(newCustomer.DateOfBirth)}", It.IsAny<CommandFlags>())).Returns(() => "true");

            var anotherCustomer = new CustomerViewModel("M", "Dehghaniii", "+989010596159", "a@a.com",
               "1231564654", "2015-02-04");

            // Assert
            await createHandler.Handle(new CreateCustomerCommand(anotherCustomer.FirstName, anotherCustomer.LastName, anotherCustomer.PhoneNumber, anotherCustomer.Email, anotherCustomer.BankAccount, anotherCustomer.DateOfBirth), CancellationToken.None);
            Assert.Equal(2, await context.Events.CountAsync());

        }

        [Fact]
        public async Task Bad_Phone_Number_shouldFail()
        {
            await Assert.ThrowsAsync<ArgumentException>(async() => await createHandler.Handle(new CreateCustomerCommand(newCustomer.FirstName, newCustomer.LastName, "+986546876465", newCustomer.Email, newCustomer.BankAccount, newCustomer.DateOfBirth), CancellationToken.None));
        }

        [Fact]
        public async Task Bad_Email_shouldFail()
        {
            var customer = new CustomerViewModel("Mohammadd", "Dehghaniii", "+989010596159", "dfhfdghfgh",
               "1231564654", "2015-02-04");
         
            await Assert.ThrowsAsync<ArgumentException>(async () => await createHandler.Handle(new CreateCustomerCommand(customer.FirstName, customer.LastName, customer.PhoneNumber, customer.Email, customer.BankAccount, customer.DateOfBirth), CancellationToken.None));
        }
    }
}