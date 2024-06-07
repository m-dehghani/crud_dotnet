using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using static System.Net.WebRequestMethods;
using System.Reflection;
using System.Net.Http.Json;
using Mc2.CrudTest.Presentation.ViewModels;
using Mc2.CrudTest.Presentation.Client.Models;
using Mc2.CrudTest.Presentation.Shared.Entities;

namespace Mc2.CrudTest.Presentation.Client.services
{
    public class CustomerService : ICustomerService
    {
        private HttpClient _httpClient;
        private NavigationManager _navigationManager;
        private ILogger<CustomerService> _logger;
        private string ApiAddress = "customer";
        public CustomerService(HttpClient httpClient, NavigationManager navigationManager, ILogger<CustomerService> logger) {
            _httpClient = httpClient;
            _navigationManager = navigationManager;
            _logger = logger;
          
        }
        private List<string> ErrorMessages { get; set; } = new List<string>();
        
        public List<string> GetErrors()
        {
            return ErrorMessages;
        }
        public async Task Update(CustomerUpdateModel model)
        {
            try
            {
                _logger.LogInformation("sending start: sending the form to customer controller");
                var result = await _httpClient.PutAsJsonAsync($"{ApiAddress}/{model.Id}", model);
                if (result.IsSuccessStatusCode)
                    _navigationManager.NavigateTo("customers");
                else
                    ErrorMessages.Add( await result.Content.ReadAsStringAsync());
            }
            catch (Exception ex)
            {
                ErrorMessages.Add("An error has been occured. Please try again later.");
            }
        }
        public async Task Create(CustomerCreateModel model)
        {
            try
            {
                _logger.LogInformation("sending start: sending the form to customer controller");

                var result = await _httpClient.PostAsJsonAsync(ApiAddress, model);
                if (result.IsSuccessStatusCode)
                    _navigationManager.NavigateTo("customers");
                else
                    ErrorMessages.Add(await result.Content.ReadAsStringAsync());
            }

            catch (Exception ex)
            {
                ErrorMessages.Add("An error has been occured. Please try again later.");
            }
        }
    
        public async Task<List<CustomerViewModel>> GetAll()
        {
            var result = await _httpClient.GetAsync(ApiAddress);
            if (result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return await result.Content.ReadFromJsonAsync<List<ViewModels.CustomerViewModel>>();
            }
            return new List<CustomerViewModel>();
        }
        
        public async Task<CustomerUpdateModel> Get(Guid id)
        {
            var result = await _httpClient.GetAsync($"{ApiAddress}/{id}");
            var model = await result.Content.ReadFromJsonAsync<ViewModels.CustomerViewModel>();
            return new CustomerUpdateModel() { Id = model.Id, FirstName = model.FirstName, LastName = model.LastName, PhoneNumber = model.PhoneNumber, BankAccount = model.BankAccount, DateOfBirth = DateOnly.Parse(model.DateOfBirth), Email = model.Email };
        }

        public async Task Delete(Guid id)
        {
            await _httpClient.DeleteAsync($"{ApiAddress}/{id}");
        }

    }
}
