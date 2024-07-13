using Mc2.CrudTest.Presentation.DomainServices;
using Mc2.CrudTest.Presentation.Handlers;
using Mc2.CrudTest.Presentation.Infrastructure;
using Mc2.CrudTest.Presentation.Shared.Commands;
using Mc2.CrudTest.Presentation.ViewModels;
using Microsoft.EntityFrameworkCore;
using Testcontainers.PostgreSql;

namespace AcceptanceTest.Drivers
{
    public abstract class CustomerDriver
    {
        private readonly CreateCustomerCommandHandler _createHandler;
        private readonly GetAllCustomerQueryHandler _getAllQueryHandler;
        private readonly UpdateCustomerCommandHandler _updateCustomerCommandHandler;
        private readonly GetCustomerByIdQueryHandler _getCustomerByIdQueryHandler;
        private readonly DeleteCustomerCommandHandler _deleteCustomerCommandHandler;

        protected CustomerDriver()
        {
            DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseNpgsql("TestWriteDb",
            npgsqlOptionsAction: sqlOptions =>
            {
                sqlOptions.EnableRetryOnFailure(
                maxRetryCount: 10,
                maxRetryDelay: TimeSpan.FromSeconds(30),
                errorCodesToAdd: null);
            }).Options;

            DbContextOptions<ReadModelDbContext> readOptions = new DbContextOptionsBuilder<ReadModelDbContext>()
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

            SlowerCustomerService customerService = new(new EventStoreRepository(context, readContext));

            _createHandler = new CreateCustomerCommandHandler(customerService);

            _updateCustomerCommandHandler = new UpdateCustomerCommandHandler(customerService);

            _getCustomerByIdQueryHandler = new GetCustomerByIdQueryHandler(customerService);

            _getAllQueryHandler = new GetAllCustomerQueryHandler(customerService);

            _deleteCustomerCommandHandler = new DeleteCustomerCommandHandler(customerService);
        }

        private readonly PostgreSqlContainer _postgres = new PostgreSqlBuilder()
            .WithImage("postgres:15-alpine")
            .Build();
        
        public async  Task Create(CreateCustomerCommand command)
        {
            await _createHandler.Handle(command, CancellationToken.None);
        }

        public async Task<List<CustomerViewModel>> GetAll()
        {
           return (await _getAllQueryHandler.Handle(new Mc2.CrudTest.Presentation.Shared.Queries.GetAllCustomersQuery(), CancellationToken.None)).ToList();
        }

        public async Task Update(UpdateCustomerCommand command)
        {
            
            await _updateCustomerCommandHandler.Handle(command, CancellationToken.None);
        }

        public async Task Delete(DeleteCustomerCommand command)
        {
            await _deleteCustomerCommandHandler.Handle(command, CancellationToken.None);
        }

        public async Task ResetDb()
        {
            await _postgres.StopAsync();

            await _postgres.StartAsync();
        }
    }
}
