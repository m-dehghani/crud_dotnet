using System;
using AcceptanceTest.Drivers;
using Mc2.CrudTest.Presentation.DomainServices;
using Mc2.CrudTest.Presentation.Handlers;
using Mc2.CrudTest.Presentation.Infrastructure;
using Mc2.CrudTest.Presentation.Server.Controllers;
using Mc2.CrudTest.Presentation.Shared.Commands;
using Mc2.CrudTest.Presentation.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Moq;
using Reqnroll;
using StackExchange.Redis;
using Testcontainers.PostgreSql;

namespace AcceptanceTest.StepDefinitions
{
    [Binding]
    public class CustomerManagerStepDefinitions
    {
        private readonly CustomerDriver customerDriver;
        public CustomerManagerStepDefinitions(CustomerDriver customerDriver) {
            this.customerDriver = customerDriver;
        }

        [Given("platform has {string} record of customers")]
        public async void GivenPlatformHasRecordOfCustomers(string p0)
        {
            await customerDriver.ResetDb();
        }


        [When("When user send command to create new customer with following information")]
        public async Task WhenICreateACustomerWithFollowingDetails(DataTable dataTable)
        {
            Dictionary<string, string> dictionary = CreateDDictionary(dataTable);
            var command = new CreateCustomerCommand(dictionary["FirstName"], dictionary["LastName"], dictionary["DateOfBirth"], dictionary["PhoneNumber"], dictionary["Email"], dictionary["BankAccountNumber"]);
            await customerDriver.CreateCustomer(command);



            //assert
        }

        [Then("user can send query and receive \"1\" record of customer with following data")]
        public async Task ThenTheCustomerShouldBeCreatedSuccessfully(DataTable dataTable)
        {
            Dictionary<string, string> dictionary = CreateDDictionary(dataTable);

            var customerList = await customerDriver.GetAllCustomers();
            customerList.Should().NotBeNull();
            var customer = customerList.FirstOrDefault();
            customer.FirstName.Should().Be(dictionary["FirstName"]);
            customer.LastName.Should().Be(dictionary["LastName"]);
            customer.Email.Should().Be(dictionary["Email"]);
            customer.DateOfBirth.Should().Be(dictionary["DateOfBirth"]);

        }

        [When("user send command to update customer with email of \"john.doe@email.com\" and with below information")]
        public async Task WhenICreateACustomerWithFollowingDetails(DataTable dataTable)
        { 
        }




            private static Dictionary<string, string> CreateDDictionary(DataTable dataTable)
        {
            var dictionary = new Dictionary<string, string>();
            foreach (var row in dataTable.Rows)
            {
                dictionary.Add(row[0], row[1]);
            }

            return dictionary;
        }

    }
}
