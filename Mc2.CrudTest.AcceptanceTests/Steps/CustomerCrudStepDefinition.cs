﻿using FluentAssertions;
using Mc2.CrudTest.AcceptanceTests.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mc2.CrudTest.AcceptanceTests.Steps
{
    [Binding]
    public class CustomerCrudStepDefinition
    {
        private readonly CustomerApi _customerApi;

        private int _statusCode;
        public CustomerCrudStepDefinition(CustomerApi customerApi)
        {
            _customerApi = customerApi;
        }

        [Given("phone number (.*)")]

        public void GivenPhoneNumber(ulong phoneNumber)
        {
            _customerApi.PhoneNumber = phoneNumber;
        }

        [When("the customer is being created")]

        public async Task WhenCustomerIsBeingCreated()
        {
            var result = await _customerApi.CreateAsync();

            _statusCode = (int)result.StatusCode;
        }
        [Then("status code will be (.*)")]

        public void ThenTheStatusCodeWillBe(int statusCode)
        {
            _statusCode.Should().Be(statusCode);
        }
    }
}
