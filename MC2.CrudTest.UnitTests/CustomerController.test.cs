using Mc2.CrudTest.Presentation.DomainServices;
using Mc2.CrudTest.Presentation.Server.Controllers;
using Mc2.CrudTest.Presentation.Shared.Entities;
using Mc2.CrudTest.Presentation.Shared.ValueObjects;
using Moq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Mc2.CrudTest.Presentation;
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
     
        public CustomerControllerTests(WebApplicationFactory<Program> factory)
        {
             _mockCustomerService = new Mock<ICustomerService>();
             _controller = new CustomerController(_mockCustomerService.Object);
             _client = factory.CreateClient();
        }
        
        [Fact]
        public async Task CreateCustomer_DuplicateEmail_ReturnsBadRequest()
        {
            // Arrange
            var newCustomer = new { Name = "Alice", Email = "alice@example.com" };
            var requestUri = new Uri("/api/customers", UriKind.Relative);
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
            new Customer(id: Guid.NewGuid(), firstName: "john", lastName: "Doe", "044 668 18 00", "john.doe@example.com",
                "1231564654"),
            new Customer(id: Guid.NewGuid(), firstName: "test", lastName: "test", "044 668 18 00",
                "jane.smith@example.com", "1231564654")
        ];

        [Fact]
        public void NewCustomer_EmailIsUnique_ShouldPass()
        {
            // Arrange
            var newCustomer = new Customer ( Guid.NewGuid(), firstName: "Mohammad", lastName: "Dehghani", "044 668 18 00", "dehghany.m@gmail.com",
                "1231564654");

            // Act
            var isUnique = !_customers.Any(c => c.Email.Equals(newCustomer.Email));

            // Assert
            Assert.True(isUnique, "Email should be unique for new customers.");
        }

        [Fact]
        public async Task ExistingCustomer_DuplicateEmail_ShouldFail()
        {
            // Arrange
            var newCustomer = new Customer("Mohammad", "Dehghani", "044 668 18 00", "dehghany@gmail.com", "65468489464");
            await _controller.CreateCustomer(newCustomer);
            var duplicateEmail = newCustomer.Email;

            // Act
            var isUnique = !_customers.Any(c => c.Id != newCustomer.Id && c.Email.Equals(duplicateEmail));
            
            // Assert
            Assert.False(isUnique, "Email should not be duplicated for existing customers.");
        }
    
        
        
        [Fact]
        public async Task CreateCustomer_ValidInput_CallsAddCustomerAsync()
        {
            var newCustomer = new Customer("Mohammad", "Dehghani", "044 668 18 00", "dehghany@gmail.com", "65468489464");
         
            await _controller.CreateCustomer(newCustomer);

            _mockCustomerService.Verify(s => s.CreateCustomerAsync(newCustomer), Times.Once);
        }

        [Fact]
        public async Task UpdateCustomer_ValidInput_CallsUpdateCustomerAsync()
        {
            var existingCustomer = new Customer(); // TODO: complete this
         
            var res = await Record.ExceptionAsync(async () => await _controller.UdateCustomer(existingCustomer));
            
            Assert.Null(res);

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