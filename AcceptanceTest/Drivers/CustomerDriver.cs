using Mc2.CrudTest.Presentation.Server.DomainServices;
using Mc2.CrudTest.Presentation.Server.Handlers;
using Mc2.CrudTest.Presentation.Server.Infrastructure;
using Mc2.CrudTest.Presentation.Shared.Commands;
using Mc2.CrudTest.Presentation.Shared.ViewModels;
using Microsoft.EntityFrameworkCore;
using Testcontainers.PostgreSql;

namespace AcceptanceTest.Drivers
{
    public abstract class CustomerDriver
    {
        private readonly CreateCustomerCommandHandler _createHandler;
        private readonly GetAllCustomerQueryHandler _getAllQueryHandler;
        private readonly UpdateCustomerCommandHandler _updateCustomerCommandHandler;

        protected CustomerDriver()
        {
            DbContextOptions<ApplicationDbContext> options =
                new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseNpgsql("TestWriteDb",
            npgsqlOptionsAction: sqlOptions =>
            {
                sqlOptions.EnableRetryOnFailure(
                maxRetryCount: 10,
                maxRetryDelay: TimeSpan.FromSeconds(30),
                errorCodesToAdd: null);
            }).Options;

            DbContextOptions<ReadModelDbContext> readOptions = 
                new DbContextOptionsBuilder<ReadModelDbContext>()
               .UseNpgsql("TestReadDb",
                   npgsqlOptionsAction: sqlOptions =>
                       {
                           sqlOptions.EnableRetryOnFailure(
                           maxRetryCount: 10,
                           maxRetryDelay: TimeSpan.FromSeconds(30),
                           errorCodesToAdd: null);
                       }).Options;
            
            ApplicationDbContext context = new(options);

            ReadModelDbContext readContext = new(readOptions);

            SlowerCustomerService customerService = 
                new(new EventStoreRepository(context, readContext));

            _createHandler = new CreateCustomerCommandHandler(customerService);

            _updateCustomerCommandHandler = new UpdateCustomerCommandHandler(customerService);

            new GetCustomerByIdQueryHandler(customerService);

            _getAllQueryHandler = new GetAllCustomerQueryHandler(customerService);
        }

        private readonly PostgreSqlContainer _postgres = new PostgreSqlBuilder()
            .WithImage("postgres:15-alpine")
            .Build();
        
        public async  Task CreateCustomer(CreateCustomerCommand? command)
        {
            await _createHandler.Handle(command, CancellationToken.None);
        }

        public async Task<List<CustomerViewModel>> GetAllCustomers()
        {
            return (await _getAllQueryHandler.Handle(new Mc2.CrudTest.Presentation.Shared.Queries.GetAllCustomersQuery(), CancellationToken.None)).ToList();
        }

        public async Task UpdateCustomer(UpdateCustomerCommand command)
        {
            await _updateCustomerCommandHandler.Handle(command, CancellationToken.None);
        }

        public async Task ResetDb()
        {
            await _postgres.StopAsync();

            await _postgres.StartAsync();
        }
    }
}
