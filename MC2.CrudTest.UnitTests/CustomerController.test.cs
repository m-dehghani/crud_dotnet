using System.Diagnostics;
using Mc2.CrudTest.Presentation.DomainServices;
using Mc2.CrudTest.Presentation.Server.Controllers;
using Mc2.CrudTest.Presentation.Shared.Entities;
using Mc2.CrudTest.Presentation.Shared.ValueObjects;
using Moq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using FluentAssertions.Common;
using Mc2.CrudTest.Presentation;
using Mc2.CrudTest.Presentation.Infrastructure;
using Mc2.CrudTest.Presentation.Shared.Commands;
using Mc2.CrudTest.Presentation.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Xunit;

using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using StackExchange.Redis;
using Mc2.CrudTest.Presentation.Handlers;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace MC2.CrudTest.UnitTests
{
    
    public class CustomerControllerTests: IClassFixture<WebApplicationFactory<Program>>
    {
        private ICustomerService _customerService;
        private CustomerController _controller;
        private readonly HttpClient _client;
        private Mock<IMediator> _mediator;
        private ApplicationDbContext context;
        public CustomerControllerTests(WebApplicationFactory<Program> factory)
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "customers")
                .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                .Options;
               
               context = new ApplicationDbContext(options);
             _mediator = new  Mock<IMediator>();
           
             _client = factory.CreateClient();
        }
        
        [Fact]
        public async Task CreateCustomer_DuplicateEmail_ReturnsBadRequest()
        {
            // Arrange

            var newCustomer = new CustomerViewModel("Mohammadd", "Dehghaniii", "044 668 18 00", "dehghany.m@gmail.com",
              "1231564654", "2015-02-04");
            var mockMultiplexer = new Mock<IConnectionMultiplexer>();

            mockMultiplexer.Setup(_ => _.IsConnected).Returns(false);

            var mockDatabase = new Mock<IDatabase>();
            mockMultiplexer
                .Setup(_ => _.GetDatabase(It.IsAny<int>(), It.IsAny<object>()))
                .Returns(mockDatabase.Object);

            var cacheHandler = new RedisCacheHandler(mockMultiplexer.Object);
             
           
            _customerService = new CustomerService(new EventStoreRepository(context, null), mockDatabase.Object);
            var handler = new CreateCustomerEventHandler(_customerService);
            await handler.Handle(new CreateCustomerCommand(newCustomer.FirstName, newCustomer.LastName, newCustomer.PhoneNumber, newCustomer.Email, newCustomer.BankAccount, newCustomer.DateOfBirth), CancellationToken.None);
                       
            cacheHandler.SetCustomerData(new Customer(newCustomer.FirstName, newCustomer.LastName, newCustomer.PhoneNumber, newCustomer.Email, newCustomer.BankAccount, newCustomer.DateOfBirth));
                       
            mockDatabase.Setup((x) => x.StringGet(It.IsAny<RedisKey>(), It.IsAny<CommandFlags>())).Returns(() => "true");
            await Assert.ThrowsAsync<ArgumentException>(async() => await handler.Handle(new CreateCustomerCommand(newCustomer.FirstName, newCustomer.LastName, newCustomer.PhoneNumber, newCustomer.Email, newCustomer.BankAccount, newCustomer.DateOfBirth), CancellationToken.None));
                       
        }
      

        [Fact]
        public async Task NewCustomer_EmailIsUnique_ShouldPass()
        {
            // Arrange
            var newCustomer = new CustomerViewModel(  "Mohammad",  "Dehghani", "044 668 18 00", "dehghany.m@gmail.com",
                "1231564654","2015-02-04");

            // Act
            var resp=  await _controller.CreateCustomer(newCustomer);
           //  var isUnique = !_customers.Any(c => c.Email.Equals(newCustomer.Email));

            // Assert
            Assert.Equal(new OkResult(), resp);
        }

       
        
        [Fact]
        public async Task CreateCustomer_ValidInput_CallsAddCustomerAsync()
        {
            // Arrange
            var newCustomer = new CustomerViewModel("Mohammad1111", "Dehghani", "044 668 18 00", "dehghan1y@gggkgggghmail.com", "65468489464","2015-05-06");
         
            // Act
            var response2 = await _client.PostAsJsonAsync("/customer", newCustomer);
           
            // Assert
            Assert.Equal(HttpStatusCode.OK,response2.StatusCode);
            var res = await response2.Content.ReadAsStringAsync();
            Debug.WriteLine(res);
        }

        [Fact]
        public async Task UpdateCustomer_ValidInput_CallsUpdateCustomerAsync()
        {
            // var existingCustomer = new CustomerUpdateViewModel(); // TODO: complete this
            //
            // var res = await Record.ExceptionAsync(async () => await _controller.UdateCustomer(existingCustomer));
            //
            // Assert.Null(res);

        }

        [Fact]
        public async Task DeleteCustomer_ValidInput_CallsDeleteCustomerAsync()
        {
            var id = new Guid();

            var res = await Record.ExceptionAsync(async () => await _controller.DeleteCustomer(string.Empty)); 
            
            Assert.Null(res);
        }

        [Fact]
        public async Task GetCustomer_ValidInput_CallsGetCustomerAsync()
        {
            var id = new Guid("1234565487987");
          
            // var customer = await _controller.Get(id);
            //
            // Assert.NotNull(customer);
        }
    }
}