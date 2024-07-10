using Mc2.CrudTest.Presentation.DomainServices;
using Mc2.CrudTest.Presentation.Handlers;
using Mc2.CrudTest.Presentation.Infrastructure;
using Mc2.CrudTest.Presentation.Server.Controllers;
using Mc2.CrudTest.Presentation.Shared.Commands;
using Mc2.CrudTest.Presentation.Shared.Queries;
using Mc2.CrudTest.Presentation.ViewModels;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Moq;
using StackExchange.Redis;
using System.Diagnostics;
using Testcontainers.PostgreSql;

namespace AcceptanceTest.StepDefinitions
{
    public class CustomerStepDefinitions : IAsyncLifetime
    {
        private readonly PostgreSqlContainer _postgreSqlContainer = new PostgreSqlBuilder().Build();
        private SlowerCustomerService _customerService;
        private Mock<IMediator> _mediator;
        private ApplicationDbContext context;
        private ReadModelDbContext readContext;
        private CustomerViewModel newCustomer;
        private CreateCustomerCommandHandler createHandler;
        private UpdateCustomerCommandHandler updateHandler;
        private GetCustomerByIdQueryHandler getByIdHandler;
        private GetAllCustomerQueryHandler getAllQueryHandler;
        private CustomerController customerController;
        public Task InitializeAsync()
        {
             _postgreSqlContainer.StartAsync();
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseNpgsql("TestWriteDb",
                npgsqlOptionsAction: sqlOptions =>
                {
                    sqlOptions.EnableRetryOnFailure(
                    maxRetryCount: 10,
                    maxRetryDelay: TimeSpan.FromSeconds(30),
                    errorCodesToAdd: null);
                }).Options;

            var readOptions = new DbContextOptionsBuilder<ReadModelDbContext>()
               .UseNpgsql("TestReadDb",
               npgsqlOptionsAction: sqlOptions =>
               {
                   sqlOptions.EnableRetryOnFailure(
                   maxRetryCount: 10,
                   maxRetryDelay: TimeSpan.FromSeconds(30),
                   errorCodesToAdd: null);
               }).Options;
         


            context = new ApplicationDbContext(options);
            readContext = new ReadModelDbContext(readOptions);

            _mediator = new Mock<IMediator>();

            var mockMultiplexer = new Mock<IConnectionMultiplexer>();

            mockMultiplexer.Setup(_ => _.IsConnected).Returns(false);
            
            _customerService = new SlowerCustomerService(new EventStoreRepository(context, readContext));

            newCustomer = new CustomerViewModel(Guid.NewGuid(), new List<string>().ToArray(), "Mohammadd", "Dehghaniii", "+989010596159", "dehghany.m@gmail.com",
               "1231564654", "2015-02-04");

            createHandler = new CreateCustomerCommandHandler(_customerService);

            updateHandler = new UpdateCustomerCommandHandler(_customerService);

            getByIdHandler = new GetCustomerByIdQueryHandler(_customerService);

            getAllQueryHandler = new GetAllCustomerQueryHandler(_customerService);

            
            customerController = new CustomerController(_mediator.Object);

           return Task.CompletedTask;
        }

        public Task DisposeAsync()
        {
            return _postgreSqlContainer.DisposeAsync().AsTask();
        }

        [Given("I am an operator")]
        public async Task GivenIAmAnOperator()
        {
            Debug.WriteLine("Staring ...");
            //IServiceProvider s = new ServiceProvider(typeof(CustomerStepDefinition),);
            // var customerController = new CustomerController( new Mediator(_serviceProvider));


            // await Task.FromResult(Task.CompletedTask);
            // _scenarioContext.Pending();
        }

        [When("I create a Customer with following details")]
        public async Task WhenICreateACustomerWithTheFollowingDetails(Table table)
        {
            //TODO: create a customer with table data and calling the CustomerCreatedHandler for this step
            var dictionary = new Dictionary<string, string>();
            foreach (var row in table.Rows)
            {
                dictionary.Add(row[0], row[1]);
            }
           var command = new CreateCustomerCommand(dictionary["FirstName"], dictionary["LastName"], dictionary["DateOfBirth"], dictionary["PhoneNumber"], dictionary["Email"], dictionary["BankAccountNumber"]);
           await createHandler.Handle(command, CancellationToken.None);
        }

        [Then("The customer should be created successfully")]
        public async Task ThenTheCustomerShouldBeCreatedSuccessfully()
        {
            //TODO: check the customer created in DB by querying the read model
            //Arrange

           var customerList = await getAllQueryHandler.Handle(new GetAllCustomersQuery(), CancellationToken.None);

            var result = await customerController.GetAll();
            result.Should().BeEquivalentTo(customerList);
            
        }

    }
}
