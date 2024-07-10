using Mc2.CrudTest.Presentation.DomainServices;
using Mc2.CrudTest.Presentation.Handlers;
using Mc2.CrudTest.Presentation.Infrastructure;
using Mc2.CrudTest.Presentation.Server.Controllers;
using Mc2.CrudTest.Presentation.Shared.Commands;
using Mc2.CrudTest.Presentation.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.EntityFrameworkCore;
using Moq;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Testcontainers.PostgreSql;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace AcceptanceTest.Drivers
{
    public class CustomerDriver
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
       
        public CustomerDriver()
        {
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
        }

        private readonly PostgreSqlContainer _postgres = new PostgreSqlBuilder()
       .WithImage("postgres:15-alpine")
       .Build();
        public async  Task CreateCustomer(CreateCustomerCommand command)
        {
            await _postgres.StartAsync();
            await createHandler.Handle(command, CancellationToken.None);
        }

        public async Task<List<CustomerViewModel>> GetAllCustomers()
        {
           return (await getAllQueryHandler.Handle(new Mc2.CrudTest.Presentation.Shared.Queries.GetAllCustomersQuery(), CancellationToken.None)).ToList();
        }

        public async Task UpdateCustomer(UpdateCustomerCommand command)
        {
            await Handler.Handle(command, CancellationToken.None);
        }
        public async Task ResetDb()
        {
            await _postgres.StopAsync();

            await _postgres.StartAsync();

        }
    }
}
