using Mc2.CrudTest.Presentation.Shared.Entities;
using Moq;
using Mc2.CrudTest.Presentation.Server;
using Mc2.CrudTest.Presentation.Server.DomainServices;
using Mc2.CrudTest.Presentation.Server.Handlers;
using Mc2.CrudTest.Presentation.Server.Infrastructure;
using Mc2.CrudTest.Presentation.Shared.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using StackExchange.Redis;
using Mc2.CrudTest.Presentation.Shared.Events;
using Mc2.CrudTest.Presentation.Shared.ViewModels;

namespace MC2.CrudTest.UnitTests
{
    public class CustomerControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly ApplicationDbContext _context;
        private readonly RedisCacheHandler _cacheHandler;
        private readonly Mock<IDatabase?> _mockDatabase;
        private readonly CustomerViewModel _newCustomer;
        private readonly CreateCustomerCommandHandler _createHandler;
        private readonly UpdateCustomerCommandHandler _updateHandler;


        public CustomerControllerTests(WebApplicationFactory<Program> factory)
        {
            DbContextOptions<ApplicationDbContext>? options = 
                new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "customers")
                .ConfigureWarnings(x => 
                    x.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                .Options;

            _context = new ApplicationDbContext(options);

            Mock<IConnectionMultiplexer>? mockMultiplexer = new();

            mockMultiplexer.Setup(_ => _.IsConnected).Returns(false);

            _mockDatabase = new Mock<IDatabase?>();

            mockMultiplexer
                .Setup(_ => _.GetDatabase
                    (It.IsAny<int>(), It.IsAny<object>()))
                .Returns(_mockDatabase.Object);

            _cacheHandler = new RedisCacheHandler(mockMultiplexer.Object);

            ICustomerService customerService = 
                new CustomerService(new EventStoreRepository(_context, null),
                    _mockDatabase.Object);

            _newCustomer = new CustomerViewModel(Guid.NewGuid(),
                new List<string>().ToArray(), "Mohammadd",
                "Dehghaniii", "+989010596159",
                "dehghany.m@gmail.com",
               "1231564654", "2015-02-04");

            _createHandler = new CreateCustomerCommandHandler(customerService);

            _updateHandler = new UpdateCustomerCommandHandler(customerService);

        }

        [Fact]
        public async Task CreateCustomer_DuplicateEmail_throws_exception()
        {
            await _createHandler.Handle(new 
                CreateCustomerCommand(_newCustomer.FirstName, _newCustomer.LastName,
                    _newCustomer.PhoneNumber, _newCustomer.Email, _newCustomer.BankAccount,
                    _newCustomer.DateOfBirth), CancellationToken.None);

            _cacheHandler.SetCustomerData(new Customer(_newCustomer.FirstName,
                _newCustomer.LastName, _newCustomer.PhoneNumber, _newCustomer.Email, 
                _newCustomer.BankAccount, _newCustomer.DateOfBirth));

            _mockDatabase.Setup((x) => x.StringGet(_newCustomer.Email, 
                It.IsAny<CommandFlags>())).Returns(() => "true");

            _mockDatabase.Setup((x) => 
                x.StringGet($"{_newCustomer.FirstName}-" +
                            $"{_newCustomer.LastName}-{DateOnly.Parse(_newCustomer.DateOfBirth)}", 
                    It.IsAny<CommandFlags>())).Returns(() => "true");

            await Assert.ThrowsAsync<ArgumentException>(async () => 
                await _createHandler.Handle(new CreateCustomerCommand(_newCustomer.FirstName, 
                    _newCustomer.LastName, _newCustomer.PhoneNumber, _newCustomer.Email, 
                    _newCustomer.BankAccount, _newCustomer.DateOfBirth), CancellationToken.None));

        }

        [Fact]
        public async Task NewCustomer_CustomerDataIsNotUnique_EmailIsUnique_ShouldFail()
        {

            await _createHandler.Handle(
                new CreateCustomerCommand(_newCustomer.FirstName, _newCustomer.LastName,
                    _newCustomer.PhoneNumber, _newCustomer.Email, _newCustomer.BankAccount,
                    _newCustomer.DateOfBirth), CancellationToken.None);

            _cacheHandler.SetCustomerData(new Customer(_newCustomer.FirstName,
                _newCustomer.LastName, _newCustomer.PhoneNumber, _newCustomer.Email, 
                _newCustomer.BankAccount, _newCustomer.DateOfBirth));

            _mockDatabase.Setup((x) => x.StringGet(_newCustomer.Email,
                It.IsAny<CommandFlags>())).Returns(() => "true");

            _mockDatabase.Setup((x) => x.StringGet(
                $"{_newCustomer.FirstName}-{_newCustomer.LastName}" +
                $"-{DateOnly.Parse(_newCustomer.DateOfBirth)}", It.IsAny<CommandFlags>()))
                .Returns(() => "true");

            CustomerViewModel? anotherCustomer = 
                new(Guid.NewGuid(), [], "Mohammadd", 
                    "Dehghaniii", "+989010596159", "a@a.com",
               "1231564654", "2015-02-04");

            await Assert.ThrowsAsync<ArgumentException>
                (async () => await _createHandler.Handle(
                    new CreateCustomerCommand(anotherCustomer.FirstName, 
                        anotherCustomer.LastName, anotherCustomer.PhoneNumber, 
                        anotherCustomer.Email, anotherCustomer.BankAccount, 
                        anotherCustomer.DateOfBirth), CancellationToken.None));

        }

        [Fact]
        public async Task Both_EmailIAndCustomerDataIsUnique_ShouldPass()
        {
            ResetDbContext();

            await _createHandler.Handle(
                new CreateCustomerCommand(_newCustomer.FirstName, 
                    _newCustomer.LastName, _newCustomer.PhoneNumber, 
                    _newCustomer.Email, _newCustomer.BankAccount, 
                    _newCustomer.DateOfBirth), CancellationToken.None);

            _cacheHandler.SetCustomerData(
                new Customer(_newCustomer.FirstName, _newCustomer.LastName, 
                    _newCustomer.PhoneNumber, _newCustomer.Email, _newCustomer.BankAccount,
                    _newCustomer.DateOfBirth));

            _mockDatabase.Setup((x) => x.StringGet(
                _newCustomer.Email, It.IsAny<CommandFlags>()))
                .Returns(() => "true");

            _mockDatabase.Setup((
                x) => x.StringGet($"{_newCustomer.FirstName}-" +
                                  $"{_newCustomer.LastName}-{DateOnly.Parse(_newCustomer.DateOfBirth)}"
                , It.IsAny<CommandFlags>())).Returns(() => "true");

            CustomerViewModel? anotherCustomer = 
                new(Guid.NewGuid(), new List<string>().ToArray(), 
                    "M", "Dehghaniii", "+989010596159", "a@a.com",
               "1231564654", "2015-02-04");

            await _createHandler.Handle(
                new CreateCustomerCommand(anotherCustomer.FirstName, anotherCustomer.LastName, 
                    anotherCustomer.PhoneNumber, anotherCustomer.Email, anotherCustomer.BankAccount, 
                    anotherCustomer.DateOfBirth), CancellationToken.None);

            Assert.Equal(2, await _context.Events.CountAsync());

        }

        [Fact]
        public async Task Bad_Phone_Number_shouldFail()
        {
            await Assert.ThrowsAsync<ArgumentException>(
                async () => await _createHandler.Handle(
                    new CreateCustomerCommand(_newCustomer.FirstName, 
                        _newCustomer.LastName, "+986546876465", 
                        _newCustomer.Email, _newCustomer.BankAccount, _newCustomer.DateOfBirth)
                    , CancellationToken.None));
        }

        [Fact]
        public async Task Bad_Email_shouldFail()
        {
            CustomerViewModel? customer = 
                new(Guid.NewGuid(), new List<string>().ToArray(),
                    "Mohammadd", "Dehghaniii", 
                    "+989010596159", "dfhfdghfgh",
               "1231564654", "2015-02-04");

            await Assert.ThrowsAsync<ArgumentException>
                (async () => await _createHandler.Handle(
                    new CreateCustomerCommand(customer.FirstName, customer.LastName, 
                        customer.PhoneNumber, customer.Email, customer.BankAccount, 
                        customer.DateOfBirth), CancellationToken.None));
        }

        [Fact]
        public async Task CustomerUpdate_ShouldPass()
        {
            ResetDbContext();

            await _createHandler.Handle(
                new CreateCustomerCommand(_newCustomer.FirstName, _newCustomer.LastName,
                    _newCustomer.PhoneNumber, _newCustomer.Email, _newCustomer.BankAccount, 
                    _newCustomer.DateOfBirth), CancellationToken.None);

            _cacheHandler.SetCustomerData(new Customer(
                _newCustomer.FirstName, _newCustomer.LastName,
                _newCustomer.PhoneNumber, _newCustomer.Email, 
                _newCustomer.BankAccount, _newCustomer.DateOfBirth));

            _mockDatabase.Setup((x) => x.StringGet(
                _newCustomer.Email, It.IsAny<CommandFlags>()))
                .Returns(() => "true");

            _mockDatabase.Setup((x) => x.StringGet(
                $"{_newCustomer.FirstName}-{_newCustomer.LastName}-" +
                $"{DateOnly.Parse(
                    _newCustomer.DateOfBirth)}", 
                It.IsAny<CommandFlags>()))
                .Returns(() => "true");

            Guid? cutomerId = _context.Events.FirstOrDefault()?.AggregateId;

            if (cutomerId.HasValue)
            {

                await _updateHandler.Handle(
                    new UpdateCustomerCommand(cutomerId.Value, 
                        "a", "a", "+989010596159", 
                        "a@a.com", "123456", "2015-02-08"), 
                    CancellationToken.None);

                EventBase? lastEvent = _context.Events.LastOrDefault();

                Assert.Equal(2, _context.Events.Count());

                Assert.IsType<CustomerUpdatedEvent>(lastEvent);
            }
        }

        private void ResetDbContext()
        {
            foreach (EventBase? entity in _context.Events)
            {
                _context.Events.Remove(entity);
            }
            
            _context.SaveChanges();
        }

        [Fact]
        public async Task Get_Customer_ShouldPass()
        {
            ResetDbContext();

            await _createHandler.Handle(
                new CreateCustomerCommand(
                    _newCustomer.FirstName, _newCustomer.LastName, 
                    _newCustomer.PhoneNumber, _newCustomer.Email,
                    _newCustomer.BankAccount, _newCustomer.DateOfBirth), 
                CancellationToken.None);

            _cacheHandler.SetCustomerData(
                new Customer(_newCustomer.FirstName, 
                    _newCustomer.LastName, _newCustomer.PhoneNumber,
                    _newCustomer.Email, _newCustomer.BankAccount, 
                    _newCustomer.DateOfBirth));

            _mockDatabase.Setup((x) => x.StringGet(
                _newCustomer.Email, It.IsAny<CommandFlags>()))
                .Returns(() => "true");

            _mockDatabase.Setup((x) => x.StringGet(
                $"{_newCustomer.FirstName}-{_newCustomer.LastName}-" +
                $"{DateOnly.Parse(_newCustomer.DateOfBirth)}", 
                It.IsAny<CommandFlags>())).Returns(() => "true");

            Guid? cutomerId = _context.Events.FirstOrDefault()?.AggregateId;

            if (cutomerId.HasValue)
            {
                await _updateHandler.Handle(
                    new UpdateCustomerCommand(
                        cutomerId.Value, "a", "a", 
                        "+989010596159", "a@a.com", 
                        "123456", "2015-02-08"), 
                    CancellationToken.None);

                EventBase? lastEvent = _context.Events.LastOrDefault();

                Assert.Equal(2, _context.Events.Count());

                Assert.IsType<CustomerUpdatedEvent>(lastEvent);
            }
        }

        [Fact]
        public async Task Update_Customer_NotUniqueEmail_ShouldFail()
        {
            ResetDbContext();

            await _createHandler.Handle(
                new CreateCustomerCommand(_newCustomer.FirstName, 
                    _newCustomer.LastName, _newCustomer.PhoneNumber, 
                    _newCustomer.Email, _newCustomer.BankAccount, 
                    _newCustomer.DateOfBirth), CancellationToken.None);

            _cacheHandler.SetCustomerData(
                new Customer(_newCustomer.FirstName, 
                    _newCustomer.LastName, _newCustomer.PhoneNumber, 
                    _newCustomer.Email, _newCustomer.BankAccount,
                    _newCustomer.DateOfBirth));

            _mockDatabase.Setup(
                (x) => x.StringGet(
                    _newCustomer.Email, It.IsAny<CommandFlags>()))
                .Returns(() => "true");

            _mockDatabase.Setup(
                (x) => x.StringGet(
                    $"{_newCustomer.FirstName}-{_newCustomer.LastName}-" +
                    $"{DateOnly.Parse(_newCustomer.DateOfBirth)}", It.IsAny<CommandFlags>()))
                .Returns(() => "true");

            CustomerViewModel? secondCustomer = 
                new(Guid.NewGuid(), [], 
                    "M", "Dehghaniii", 
                    "+989010596159", "a@a.com",
               "1231564654", "2015-02-04");

            await _createHandler.Handle(
                new CreateCustomerCommand(
                    secondCustomer.FirstName, secondCustomer.LastName, 
                    secondCustomer.PhoneNumber, secondCustomer.Email, 
                    secondCustomer.BankAccount, secondCustomer.DateOfBirth), 
                CancellationToken.None);

            Assert.Equal(2, await _context.Events.CountAsync());

            Guid? secondCutomerId = _context.Events.LastOrDefault()?.AggregateId;
            
            if (secondCutomerId.HasValue)
            {
                await Assert.ThrowsAsync<ArgumentException>(
                    async () => await _updateHandler.Handle(
                        new UpdateCustomerCommand(secondCutomerId.Value, 
                            secondCustomer.FirstName, secondCustomer.LastName, 
                            secondCustomer.PhoneNumber, _newCustomer.Email, 
                            secondCustomer.BankAccount, secondCustomer.DateOfBirth), 
                        CancellationToken.None));
            }
        }

        [Fact]
        public async Task Update_Customer_UniqueEmail_ShouldPass()
        {
            ResetDbContext();

            await _createHandler.Handle(
                new CreateCustomerCommand(_newCustomer.FirstName, 
                    _newCustomer.LastName, _newCustomer.PhoneNumber, 
                    _newCustomer.Email, _newCustomer.BankAccount, 
                    _newCustomer.DateOfBirth), 
                CancellationToken.None);

            _cacheHandler.SetCustomerData(
                new Customer(_newCustomer.FirstName, 
                    _newCustomer.LastName, _newCustomer.PhoneNumber, 
                    _newCustomer.Email, _newCustomer.BankAccount,
                    _newCustomer.DateOfBirth));

            _mockDatabase.Setup(
                (x) => x.StringGet(
                    _newCustomer.Email, It.IsAny<CommandFlags>()))
                .Returns(() => "true");

            _mockDatabase.Setup(
                (x) => x.StringGet(
                    $"{_newCustomer.FirstName}-{_newCustomer.LastName}" +
                    $"-{DateOnly.Parse(_newCustomer.DateOfBirth)}", 
                    It.IsAny<CommandFlags>()))
                .Returns(() => "true");

            CustomerViewModel? secondCustomer = 
                new(Guid.NewGuid(), [], 
                    "M", "Dehghaniii", "+989010596159"
                    , "a@a.com",
               "1231564654", "2015-02-04");

            await _createHandler.Handle(
                new CreateCustomerCommand(secondCustomer.FirstName, secondCustomer.LastName
                    , secondCustomer.PhoneNumber, secondCustomer.Email, secondCustomer.BankAccount
                    , secondCustomer.DateOfBirth), CancellationToken.None);

            Assert.Equal(2, await _context.Events.CountAsync());

            Guid? secondCutomerId = _context.Events.LastOrDefault()?.AggregateId;
            if (secondCutomerId.HasValue)
            {
                string? uniqueEmail = "b@b.com";
                await _updateHandler.Handle(
                    new UpdateCustomerCommand(secondCutomerId.Value, 
                        secondCustomer.FirstName, secondCustomer.LastName, 
                        secondCustomer.PhoneNumber, uniqueEmail, secondCustomer.BankAccount, 
                        secondCustomer.DateOfBirth), CancellationToken.None);
                
                Assert.Equal(3, _context.Events.Count());
            }
        }
        
      
        [Fact]
        public async Task GetAll_Customer_ShouldPass()
        {
            ResetDbContext();

            await _createHandler.Handle(
                new CreateCustomerCommand(_newCustomer.FirstName, 
                    _newCustomer.LastName, _newCustomer.PhoneNumber,
                    _newCustomer.Email, _newCustomer.BankAccount, 
                    _newCustomer.DateOfBirth), CancellationToken.None);

            _cacheHandler.SetCustomerData(
                new Customer(_newCustomer.FirstName, 
                    _newCustomer.LastName, _newCustomer.PhoneNumber, 
                    _newCustomer.Email, _newCustomer.BankAccount,
                    _newCustomer.DateOfBirth));

            _mockDatabase.Setup(
                (x) => x.StringGet(
                    _newCustomer.Email, It.IsAny<CommandFlags>()))
                .Returns(() => "true");

            _mockDatabase.Setup((
                x) => x.StringGet(
                $"{_newCustomer.FirstName}-{_newCustomer.LastName}-" +
                $"{DateOnly.Parse(_newCustomer.DateOfBirth)}", It.IsAny<CommandFlags>()))
                .Returns(() => "true");

            CustomerViewModel? secondCustomer = 
                new(new Guid(), [], 
                    "M", "Dehghaniii", "+989010596159", "a@a.com",
               "1231564654", "2015-02-04");

            await _createHandler.Handle(
                new CreateCustomerCommand(secondCustomer.FirstName,
                    secondCustomer.LastName, secondCustomer.PhoneNumber,
                    secondCustomer.Email, secondCustomer.BankAccount,
                    secondCustomer.DateOfBirth), 
                CancellationToken.None);
            
            Assert.Equal(2, _context.Events.Count());
           
        }
        [Fact]
        public Task Check_phoneNumber_should_pass()
        {
            string phone = "+989121234567";
            
            _ = new Mc2.CrudTest.Presentation.Shared.ValueObjects.PhoneNumber(phone);
            
            return Task.CompletedTask;
        }

        [Fact]
        public Task Check_phoneNumber_should_throws_exception()
        {
            string phone = "+982188776655";
            
            Assert.Throws<ArgumentException>(
                ()  => new Mc2.CrudTest.Presentation.Shared.ValueObjects.PhoneNumber(phone));
           
            return Task.CompletedTask;
        }
    }
}