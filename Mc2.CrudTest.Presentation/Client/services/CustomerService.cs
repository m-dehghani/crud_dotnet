﻿using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using static System.Net.WebRequestMethods;
using System.Reflection;
using System.Net.Http.Json;
using Mc2.CrudTest.Presentation.Client.Models;
using Mc2.CrudTest.Presentation.Shared.Entities;
using Mc2.CrudTest.Presentation.Shared.ViewModels;
using Polly.CircuitBreaker;
using System.Collections.Generic;
using Mc2.CrudTest.Presentation.Shared.Helper;
using System.Text.Json;

namespace Mc2.CrudTest.Presentation.Client.services
{
    public class CustomerService : ICustomerService
    {
        private readonly HttpClient _httpClient;
        private readonly NavigationManager _navigationManager;
        private readonly ILogger<CustomerService> _logger;
        private readonly string ApiAddress = "customer/V1";
        private readonly JsonSerializerOptions _options;
        public CustomerService(HttpClient httpClient, NavigationManager navigationManager, ILogger<CustomerService> logger) {
            _httpClient = httpClient;
            _options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            _options.Converters.Add(new DateOnlyJsonConverter());
            _navigationManager = navigationManager;
            _logger = logger;
          
        }
        private static List<string> ErrorMessages { get; set; } = new List<string>();
        
        public List<string> GetErrors()
        {
            return ErrorMessages;
        }
        public async Task Update(CustomerUpdateModel model)
        {
            try
            {
                _logger.LogInformation("sending start: sending the form to customer controller");
                HttpResponseMessage? result = await _httpClient.PutAsJsonAsync($"{ApiAddress}/{model.Id}", model, _options);
                if (result.IsSuccessStatusCode)
                    _navigationManager.NavigateTo("customers");
                else
                    HandleNotSuccessfulError();
            }
            catch (BrokenCircuitException)
            {
                HandleBrokenCircuitException();
            }
            catch (Exception)
            {
                ErrorMessages.Add("An error has been occured. Please try again later.");
            }
        }
        public async Task Create(CustomerCreateModel model)
        {
            try
            {
                _logger.LogInformation("sending start: sending the form to customer controller");
                
                HttpResponseMessage? result = await _httpClient.PostAsJsonAsync(ApiAddress, model, _options);
                if (result.IsSuccessStatusCode)
                    _navigationManager.NavigateTo("customers");
                else
                    HandleNotSuccessfulError();

            }
            catch (BrokenCircuitException)
            {
                HandleBrokenCircuitException();
            }
            catch (Exception ex)
            {
                ErrorMessages.Add("An error has been occured. Please try again later.");
            }
        }
    
        public async Task<List<CustomerViewModel>> GetAll()
        {
            List<CustomerViewModel>? model  = [];
            try
            {
                HttpResponseMessage? result = await _httpClient.GetAsync(ApiAddress);
                if (result.IsSuccessStatusCode)
                {
                    model = await result.Content.ReadFromJsonAsync<List<CustomerViewModel>>(_options);
                }
                else
                {
                    HandleNotSuccessfulError();
                }
            }
            catch (BrokenCircuitException)
            {
                HandleBrokenCircuitException();
            }
            return model;
        }
        
        public async Task<CustomerUpdateModel> Get(Guid id)
        {
            CustomerViewModel? model = new();
            try
            {
                HttpResponseMessage? result = await _httpClient.GetAsync($"{ApiAddress}/{id}");
                if (result.IsSuccessStatusCode)
                {
                    model = await result.Content.ReadFromJsonAsync<CustomerViewModel>(_options);
                }
                else
                {
                    HandleNotSuccessfulError();
                }
            }
            catch (BrokenCircuitException)
            {
                HandleBrokenCircuitException();
            }
            return new CustomerUpdateModel() { Id = model.Id, FirstName = model.FirstName, LastName = model.LastName, PhoneNumber = model.PhoneNumber, BankAccount = model.BankAccount, DateOfBirth = DateOnly.Parse(model.DateOfBirth), Email = model.Email };
        }

        public async Task Delete(Guid id)
        {
            try
            {
                HttpResponseMessage? result = await _httpClient.DeleteAsync($"{ApiAddress}/{id}");
                if (!result.IsSuccessStatusCode)
                {
                    HandleNotSuccessfulError();
                }
            }
            catch (BrokenCircuitException)
            {
                HandleBrokenCircuitException();
            }
        }

        public async Task<CustomerHistoryViewModel> GetCustomerHistory(Guid id)
        { 
            CustomerHistoryViewModel? model = new();
            try
            {
                HttpResponseMessage? result = await _httpClient.GetAsync($"{ApiAddress}/{id}/History");
                if (result.IsSuccessStatusCode)
                { 
                    model = await result.Content.ReadFromJsonAsync<CustomerHistoryViewModel>(_options);
                }
                else
                {
                    HandleNotSuccessfulError();
                }
            }
            catch (BrokenCircuitException)
            {
                HandleBrokenCircuitException();
            }
            return model;
        }
       
        private static void HandleBrokenCircuitException()
        {
           ErrorMessages.Add("Customer Service is inoperative, please try later on");
        }

        private static void HandleNotSuccessfulError()
        {
            ErrorMessages.Add("An error has been occured. Please try again later.");
        }
    }
}
