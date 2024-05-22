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
                .UseInMemoryDatabase(databaseName: "TestDb")
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
            var mockMultiplexer = new Mock<IConnectionMultiplexer>();

            mockMultiplexer.Setup(_ => _.IsConnected).Returns(false);

            var mockDatabase = new Mock<IDatabase>();

            mockMultiplexer
                .Setup(_ => _.GetDatabase(It.IsAny<int>(), It.IsAny<object>()))
                .Returns(mockDatabase.Object);

            var cacheHandler = new RedisCacheHandler(mockMultiplexer.Object);
            
            var newCustomer = new CustomerViewModel("Mohammadd",  "Dehghaniii", "044 668 18 00", "dehghany.m@gmail.com",
                "1231564654","2015-02-04" );
           
            
            _customerService = new CustomerService(new EventStoreRepository(context, null), mockDatabase.Object);
            _controller = new CustomerController(_customerService, _mediator.Object);
            
            
           
            var requestUri = new Uri("/customer", UriKind.Relative);
            
            // Act
            var response1 = await _client.PostAsJsonAsync(requestUri, newCustomer);
            var response2 = await _client.PostAsJsonAsync(requestUri, newCustomer);
            var msgInData = await response1.Content.ReadAsStringAsync();
            var errorResponse = await response2.Content.ReadAsStringAsync();
            var errorDetails = JsonConvert.DeserializeAnonymousType(errorResponse, new { Message = "" });
            HttpRequestMessage deleteRequestMessage = new HttpRequestMessage(HttpMethod.Delete, requestUri);
            var response3 = await _client.SendAsync(deleteRequestMessage);
            
            // Assert
            
            Assert.Equal(HttpStatusCode.Created, response1.StatusCode);
            var customer = new Customer(Guid.NewGuid(),newCustomer.FirstName, newCustomer.LastName, newCustomer.PhoneNumber, newCustomer.Email, newCustomer.BankAccount, newCustomer.DateOfBirth);
            
            cacheHandler.SetCustomerData(customer);
            // Act
            Assert.Equal(HttpStatusCode.BadRequest, response2.StatusCode);

            Assert.Contains("Email address already exists", errorDetails?.Message);
            
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