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
using Mc2.CrudTest.Presentation.ViewModels;
using MediatR;
using Newtonsoft.Json;
using Xunit;

using Microsoft.AspNetCore.Mvc.Testing;

namespace MC2.CrudTest.UnitTests
{
    
    public class CustomerControllerTests: IClassFixture<WebApplicationFactory<Program>>
    {
        private Mock<ICustomerService> _mockCustomerService;
        private CustomerController _controller;
        private readonly HttpClient _client;
        private IMediator _mediator;
     
        public CustomerControllerTests(WebApplicationFactory<Program> factory)
        {
            
             _mockCustomerService = new Mock<ICustomerService>();
           //  _controller = new CustomerController(_mockCustomerService.Object);
             _client = factory.CreateClient();
        }
        
        [Fact]
        public async Task CreateCustomer_DuplicateEmail_ReturnsBadRequest()
        {
            // Arrange
            var newCustomer = new { Name = "Alice", Email = "alice@example.com" };
            var requestUri = new Uri("/customer", UriKind.Relative);
            var request = new HttpRequestMessage(new HttpMethod("GET"), "/customer");
          
            // Act
            var response1 = await _client.PostAsJsonAsync(requestUri, newCustomer);
            var response2 = await _client.PostAsJsonAsync(requestUri, newCustomer);
            var response = await _client.SendAsync(request);
            
            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.Created, response1.StatusCode);
            Assert.Equal(HttpStatusCode.BadRequest, response2.StatusCode);

            var errorResponse = await response2.Content.ReadAsStringAsync();
            var errorDetails = JsonConvert.DeserializeAnonymousType(errorResponse, new { Message = "" });
            Assert.Contains("Email address already exists", errorDetails.Message);
        }
        
        
        private readonly List<Customer> _customers =
        [
            // new Customer( Guid.NewGuid(),  "john",  "Doe", "044 668 18 00", "john.doe@example.com",
            //     "1231564654"),
            // new Customer( new Guid(),  "test",  "test", "044 668 18 00",
            //     "jane.smith@example.com", "1231564654")
        ];

        [Fact]
        public void NewCustomer_EmailIsUnique_ShouldPass()
        {
            // Arrange
            var newCustomer = new Customer (  "Mohammad",  "Dehghani", "044 668 18 00", "dehghany.m@gmail.com",
                "1231564654","2015-02-04");

            // Act
            var isUnique = !_customers.Any(c => c.Email.Equals(newCustomer.Email));

            // Assert
            Assert.True(isUnique, "Email should be unique for new customers.");
        }

        [Fact]
        public async Task ExistingCustomer_DuplicateEmail_ShouldFail()
        {
            // Arrange
            var newCustomer = new Customer("Mohammad", "Dehghani", "044 668 18 00", "dehghany@gmail.com", "65468489464","2015-02-04");
         
            await _controller.CreateCustomer(new CustomerViewModel( "Mohammad",  "Dehghani",  "044 668 18 00",  "dehghany@gmail.com",  "65468489464", "2015-02-04"));
            var duplicateEmail = newCustomer.Email;

            // Act
            var isUnique = !_customers.Any(c => c.Id != newCustomer.Id && c.Email.Equals(duplicateEmail));
            
            // Assert
            Assert.False(isUnique, "Email should not be duplicated for existing customers.");
        }
    
        
        
        [Fact]
        public async Task CreateCustomer_ValidInput_CallsAddCustomerAsync()
        {
            var newCustomer = new CustomerViewModel("Mohammad111", "Dehghani", "044 668 18 00", "dehghan1y@ggggggghmail.com", "65468489464","2015-05-06");
         
         
            var request = new HttpRequestMessage(new HttpMethod("POST"), "/customer");
            // Act
            var response2 = await _client.PostAsJsonAsync("/customer", newCustomer);
            
            //var response = await _client.SendAsync(request);
            
            Console.WriteLine(response2);

            // _mockCustomerService.Verify(s => s.CreateCustomerAsync(newCustomer), Times.Once);
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

            var res = await Record.ExceptionAsync(async () => await _controller.DeleteCustomer(id)); 
            
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