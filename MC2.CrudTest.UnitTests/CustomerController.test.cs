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
using Mc2.CrudTest.Presentation.Shared.Events;
using Mc2.CrudTest.Presentation.Shared.Queries;

namespace MC2.CrudTest.UnitTests
{
    public class CustomerControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private ICustomerService _customerService;
        private Mock<IMediator> _mediator;
        private ApplicationDbContext context;
        private RedisCacheHandler cacheHandler;
        private Mock<IDatabase> mockDatabase;
        private CustomerViewModel newCustomer;
        private CreateCustomerCommandHandler createHandler;
        private UpdateCustomerCommandHandler updateHandler;
        private GetCustomerByIdQueryHandler getByIdHandler;
        private GetAllCustomerQueryHandler getAllQueryHandler;

        public CustomerControllerTests(WebApplicationFactory<Program> factory)
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "customers")
                .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                .Options;

            context = new ApplicationDbContext(options);


            _mediator = new Mock<IMediator>();

            var mockMultiplexer = new Mock<IConnectionMultiplexer>();

            mockMultiplexer.Setup(_ => _.IsConnected).Returns(false);

            mockDatabase = new Mock<IDatabase>();

            mockMultiplexer
                .Setup(_ => _.GetDatabase(It.IsAny<int>(), It.IsAny<object>()))
                .Returns(mockDatabase.Object);

            cacheHandler = new RedisCacheHandler(mockMultiplexer.Object);

            _customerService = new CustomerService(new EventStoreRepository(context, null), mockDatabase.Object);

            newCustomer = new CustomerViewModel(Guid.NewGuid(), [], "Mohammadd", "Dehghaniii", "+989010596159", "dehghany.m@gmail.com",
               "1231564654", "2015-02-04");

            createHandler = new CreateCustomerCommandHandler(_customerService);

            updateHandler = new UpdateCustomerCommandHandler(_customerService);

            getByIdHandler = new GetCustomerByIdQueryHandler(_customerService);

            getAllQueryHandler = new GetAllCustomerQueryHandler(_customerService);

        }

        [Fact]
        public async Task CreateCustomer_DuplicateEmail_throws_exception()
        {
            await createHandler.Handle(new CreateCustomerCommand(newCustomer.FirstName, newCustomer.LastName, newCustomer.PhoneNumber, newCustomer.Email, newCustomer.BankAccount, newCustomer.DateOfBirth), CancellationToken.None);

            cacheHandler.SetCustomerData(new Customer(newCustomer.FirstName, newCustomer.LastName, newCustomer.PhoneNumber, newCustomer.Email, newCustomer.BankAccount, newCustomer.DateOfBirth));

            mockDatabase.Setup((x) => x.StringGet(newCustomer.Email, It.IsAny<CommandFlags>())).Returns(() => "true");

            mockDatabase.Setup((x) => x.StringGet($"{newCustomer.FirstName}-{newCustomer.LastName}-{DateOnly.Parse(newCustomer.DateOfBirth)}", It.IsAny<CommandFlags>())).Returns(() => "true");

            await Assert.ThrowsAsync<ArgumentException>(async () => await createHandler.Handle(new CreateCustomerCommand(newCustomer.FirstName, newCustomer.LastName, newCustomer.PhoneNumber, newCustomer.Email, newCustomer.BankAccount, newCustomer.DateOfBirth), CancellationToken.None));

        }

        [Fact]
        public async Task NewCustomer_CustomerDataIsNotUnique_EmailIsUnique_ShouldFail()
        {

            await createHandler.Handle(new CreateCustomerCommand(newCustomer.FirstName, newCustomer.LastName, newCustomer.PhoneNumber, newCustomer.Email, newCustomer.BankAccount, newCustomer.DateOfBirth), CancellationToken.None);

            cacheHandler.SetCustomerData(new Customer(newCustomer.FirstName, newCustomer.LastName, newCustomer.PhoneNumber, newCustomer.Email, newCustomer.BankAccount, newCustomer.DateOfBirth));

            mockDatabase.Setup((x) => x.StringGet(newCustomer.Email, It.IsAny<CommandFlags>())).Returns(() => "true");

            mockDatabase.Setup((x) => x.StringGet($"{newCustomer.FirstName}-{newCustomer.LastName}-{DateOnly.Parse(newCustomer.DateOfBirth)}", It.IsAny<CommandFlags>())).Returns(() => "true");

            var anotherCustomer = new CustomerViewModel(Guid.NewGuid(), [], "Mohammadd", "Dehghaniii", "+989010596159", "a@a.com",
               "1231564654", "2015-02-04");

            await Assert.ThrowsAsync<ArgumentException>(async () => await createHandler.Handle(new CreateCustomerCommand(anotherCustomer.FirstName, anotherCustomer.LastName, anotherCustomer.PhoneNumber, anotherCustomer.Email, anotherCustomer.BankAccount, anotherCustomer.DateOfBirth), CancellationToken.None));

        }

        [Fact]
        public async Task Both_EmailIAndCustomerDataIsUnique_ShouldPass()
        {
            ResetDbContext();

            await createHandler.Handle(new CreateCustomerCommand(newCustomer.FirstName, newCustomer.LastName, newCustomer.PhoneNumber, newCustomer.Email, newCustomer.BankAccount, newCustomer.DateOfBirth), CancellationToken.None);

            cacheHandler.SetCustomerData(new Customer(newCustomer.FirstName, newCustomer.LastName, newCustomer.PhoneNumber, newCustomer.Email, newCustomer.BankAccount, newCustomer.DateOfBirth));

            mockDatabase.Setup((x) => x.StringGet(newCustomer.Email, It.IsAny<CommandFlags>())).Returns(() => "true");

            mockDatabase.Setup((x) => x.StringGet($"{newCustomer.FirstName}-{newCustomer.LastName}-{DateOnly.Parse(newCustomer.DateOfBirth)}", It.IsAny<CommandFlags>())).Returns(() => "true");

            var anotherCustomer = new CustomerViewModel(Guid.NewGuid(), [], "M", "Dehghaniii", "+989010596159", "a@a.com",
               "1231564654", "2015-02-04");

            await createHandler.Handle(new CreateCustomerCommand(anotherCustomer.FirstName, anotherCustomer.LastName, anotherCustomer.PhoneNumber, anotherCustomer.Email, anotherCustomer.BankAccount, anotherCustomer.DateOfBirth), CancellationToken.None);

            Assert.Equal(2, await context.Events.CountAsync());

        }

        [Fact]
        public async Task Bad_Phone_Number_shouldFail()
        {
            await Assert.ThrowsAsync<ArgumentException>(async () => await createHandler.Handle(new CreateCustomerCommand(newCustomer.FirstName, newCustomer.LastName, "+986546876465", newCustomer.Email, newCustomer.BankAccount, newCustomer.DateOfBirth), CancellationToken.None));
        }

        [Fact]
        public async Task Bad_Email_shouldFail()
        {
            var customer = new CustomerViewModel(Guid.NewGuid(), [], "Mohammadd", "Dehghaniii", "+989010596159", "dfhfdghfgh",
               "1231564654", "2015-02-04");

            await Assert.ThrowsAsync<ArgumentException>(async () => await createHandler.Handle(new CreateCustomerCommand(customer.FirstName, customer.LastName, customer.PhoneNumber, customer.Email, customer.BankAccount, customer.DateOfBirth), CancellationToken.None));
        }

        [Fact]
        public async Task CustomerUpdate_ShouldPass()
        {
            ResetDbContext();

            await createHandler.Handle(new CreateCustomerCommand(newCustomer.FirstName, newCustomer.LastName, newCustomer.PhoneNumber, newCustomer.Email, newCustomer.BankAccount, newCustomer.DateOfBirth), CancellationToken.None);

            cacheHandler.SetCustomerData(new Customer(newCustomer.FirstName, newCustomer.LastName, newCustomer.PhoneNumber, newCustomer.Email, newCustomer.BankAccount, newCustomer.DateOfBirth));

            mockDatabase.Setup((x) => x.StringGet(newCustomer.Email, It.IsAny<CommandFlags>())).Returns(() => "true");

            mockDatabase.Setup((x) => x.StringGet($"{newCustomer.FirstName}-{newCustomer.LastName}-{DateOnly.Parse(newCustomer.DateOfBirth)}", It.IsAny<CommandFlags>())).Returns(() => "true");

            var cutomerId = context.Events.FirstOrDefault()?.AggregateId;

            if (cutomerId.HasValue)
            {

                await updateHandler.Handle(new UpdateCustomerCommand(cutomerId.Value, "a", "a", "+989010596159", "a@a.com", "123456", "2015-02-08"), CancellationToken.None);

                var lastEvent = context.Events.LastOrDefault();

                Assert.Equal(2, context.Events.Count());

                Assert.IsType<CustomerUpdatedEvent>(lastEvent);
            }
        }

        private void ResetDbContext()
        {
            foreach (var entity in context.Events)
                context.Events.Remove(entity);
            context.SaveChanges();
        }

        [Fact]
        public async Task Get_Customer_ShouldPass()
        {
            ResetDbContext();

            await createHandler.Handle(new CreateCustomerCommand(newCustomer.FirstName, newCustomer.LastName, newCustomer.PhoneNumber, newCustomer.Email, newCustomer.BankAccount, newCustomer.DateOfBirth), CancellationToken.None);

            cacheHandler.SetCustomerData(new Customer(newCustomer.FirstName, newCustomer.LastName, newCustomer.PhoneNumber, newCustomer.Email, newCustomer.BankAccount, newCustomer.DateOfBirth));

            mockDatabase.Setup((x) => x.StringGet(newCustomer.Email, It.IsAny<CommandFlags>())).Returns(() => "true");

            mockDatabase.Setup((x) => x.StringGet($"{newCustomer.FirstName}-{newCustomer.LastName}-{DateOnly.Parse(newCustomer.DateOfBirth)}", It.IsAny<CommandFlags>())).Returns(() => "true");

            var cutomerId = context.Events.FirstOrDefault()?.AggregateId;

            if (cutomerId.HasValue)
            {
                //var result = await getByIdHandler.Handle(new GetCustomerByIdQuery(cutomerId.Value), CancellationToken.None);

                //Assert.NotNull(result);

                //Assert.IsType<CustomerViewModel>(result);
               
              
                //Assert.Equal(newCustomer.FirstName, result.FirstName);
             
                //Assert.True(newCustomer.FirstName.Equals(result.FirstName)
                //    && newCustomer.LastName.Equals(result.LastName)
                //    && newCustomer.Email.Equals(result.Email));


                await updateHandler.Handle(new UpdateCustomerCommand(cutomerId.Value, "a", "a", "+989010596159", "a@a.com", "123456", "2015-02-08"), CancellationToken.None);

                var lastEvent = context.Events.LastOrDefault();

                Assert.Equal(2, context.Events.Count());

                Assert.IsType<CustomerUpdatedEvent>(lastEvent);
            }
        }

        [Fact]
        public async Task Update_Customer_NotUniqueEmail_ShouldFail()
        {
            ResetDbContext();

            await createHandler.Handle(new CreateCustomerCommand(newCustomer.FirstName, newCustomer.LastName, newCustomer.PhoneNumber, newCustomer.Email, newCustomer.BankAccount, newCustomer.DateOfBirth), CancellationToken.None);

            cacheHandler.SetCustomerData(new Customer(newCustomer.FirstName, newCustomer.LastName, newCustomer.PhoneNumber, newCustomer.Email, newCustomer.BankAccount, newCustomer.DateOfBirth));

            mockDatabase.Setup((x) => x.StringGet(newCustomer.Email, It.IsAny<CommandFlags>())).Returns(() => "true");

            mockDatabase.Setup((x) => x.StringGet($"{newCustomer.FirstName}-{newCustomer.LastName}-{DateOnly.Parse(newCustomer.DateOfBirth)}", It.IsAny<CommandFlags>())).Returns(() => "true");

            var secondCustomer = new CustomerViewModel(Guid.NewGuid(), [], "M", "Dehghaniii", "+989010596159", "a@a.com",
               "1231564654", "2015-02-04");

            await createHandler.Handle(new CreateCustomerCommand(secondCustomer.FirstName, secondCustomer.LastName, secondCustomer.PhoneNumber, secondCustomer.Email, secondCustomer.BankAccount, secondCustomer.DateOfBirth), CancellationToken.None);

            Assert.Equal(2, await context.Events.CountAsync());

            var secondCutomerId = context.Events.LastOrDefault()?.AggregateId;
            if (secondCutomerId.HasValue)
            {
                await Assert.ThrowsAsync<ArgumentException>(async () => await updateHandler.Handle(new UpdateCustomerCommand(secondCutomerId.Value, secondCustomer.FirstName, secondCustomer.LastName, secondCustomer.PhoneNumber, newCustomer.Email, secondCustomer.BankAccount, secondCustomer.DateOfBirth), CancellationToken.None));
            }
        }

        [Fact]
        public async Task Update_Customer_UniqueEmail_ShouldPass()
        {
            ResetDbContext();

            await createHandler.Handle(new CreateCustomerCommand(newCustomer.FirstName, newCustomer.LastName, newCustomer.PhoneNumber, newCustomer.Email, newCustomer.BankAccount, newCustomer.DateOfBirth), CancellationToken.None);

            cacheHandler.SetCustomerData(new Customer(newCustomer.FirstName, newCustomer.LastName, newCustomer.PhoneNumber, newCustomer.Email, newCustomer.BankAccount, newCustomer.DateOfBirth));

            mockDatabase.Setup((x) => x.StringGet(newCustomer.Email, It.IsAny<CommandFlags>())).Returns(() => "true");

            mockDatabase.Setup((x) => x.StringGet($"{newCustomer.FirstName}-{newCustomer.LastName}-{DateOnly.Parse(newCustomer.DateOfBirth)}", It.IsAny<CommandFlags>())).Returns(() => "true");

            var secondCustomer = new CustomerViewModel(Guid.NewGuid(), [], "M", "Dehghaniii", "+989010596159", "a@a.com",
               "1231564654", "2015-02-04");

            await createHandler.Handle(new CreateCustomerCommand(secondCustomer.FirstName, secondCustomer.LastName, secondCustomer.PhoneNumber, secondCustomer.Email, secondCustomer.BankAccount, secondCustomer.DateOfBirth), CancellationToken.None);

            Assert.Equal(2, await context.Events.CountAsync());

            var secondCutomerId = context.Events.LastOrDefault()?.AggregateId;
            if (secondCutomerId.HasValue)
            {
                var uniqueEmail = "b@b.com";
                await updateHandler.Handle(new UpdateCustomerCommand(secondCutomerId.Value, secondCustomer.FirstName, secondCustomer.LastName, secondCustomer.PhoneNumber, uniqueEmail, secondCustomer.BankAccount, secondCustomer.DateOfBirth), CancellationToken.None);
                Assert.Equal(3, context.Events.Count());
            }
        }
        
      
        [Fact]
        public async Task GetAll_Customer_ShouldPass()
        {
            ResetDbContext();

            await createHandler.Handle(new CreateCustomerCommand(newCustomer.FirstName, newCustomer.LastName, newCustomer.PhoneNumber, newCustomer.Email, newCustomer.BankAccount, newCustomer.DateOfBirth), CancellationToken.None);

            cacheHandler.SetCustomerData(new Customer(newCustomer.FirstName, newCustomer.LastName, newCustomer.PhoneNumber, newCustomer.Email, newCustomer.BankAccount, newCustomer.DateOfBirth));

            mockDatabase.Setup((x) => x.StringGet(newCustomer.Email, It.IsAny<CommandFlags>())).Returns(() => "true");

            mockDatabase.Setup((x) => x.StringGet($"{newCustomer.FirstName}-{newCustomer.LastName}-{DateOnly.Parse(newCustomer.DateOfBirth)}", It.IsAny<CommandFlags>())).Returns(() => "true");

            var secondCustomer = new CustomerViewModel(new Guid(), [], "M", "Dehghaniii", "+989010596159", "a@a.com",
               "1231564654", "2015-02-04");

            await createHandler.Handle(new CreateCustomerCommand(secondCustomer.FirstName, secondCustomer.LastName, secondCustomer.PhoneNumber, secondCustomer.Email, secondCustomer.BankAccount, secondCustomer.DateOfBirth), CancellationToken.None);
            Assert.Equal(2, context.Events.Count());
            //var customersList =  await getAllQueryHandler.Handle(new GetAllCustomersQuery(), CancellationToken.None);
            //var firstSavedCustomer =  customersList.FirstOrDefault();
            //Assert.NotNull(firstSavedCustomer);
            //Assert.Equal(newCustomer.FirstName, firstSavedCustomer.FirstName);
            //Assert.True(newCustomer.FirstName.Equals(firstSavedCustomer.FirstName) 
            //    && newCustomer.LastName.Equals(firstSavedCustomer.LastName)
            //    && newCustomer.Email.Equals(firstSavedCustomer.Email));
            //Assert.Equal(2, customersList.Count());
        }

    }
}