using Mc2.CrudTest.Presentation.Server.Controllers;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Mc2.CrudTest.AcceptanceTests.Steps;

[Binding]
public sealed class CustomerStepDefinition
{
    
    private readonly ScenarioContext _scenarioContext;
    IServiceProvider _serviceProvider;
    public CustomerStepDefinition(ScenarioContext scenarioContext, IServiceProvider serviceProvider)
    {
        _scenarioContext = scenarioContext;
        _serviceProvider = serviceProvider;
        
        
    }

    [Given("I am an operator")]
    public void GivenIAmAnOperator()
    {
        //IServiceProvider s = new ServiceProvider(typeof(CustomerStepDefinition),);
       var customerController = new CustomerController( new Mediator(_serviceProvider));
       _scenarioContext.Pending();
    }


    [When("I create a Customer with following details")]
    public void WhenICreateACustomerWithTheFollowingDetails(Table table)
    {
        //TODO: create a customer with table data and calling the CustomerCreatedHandler for this step

        _scenarioContext.Pending();
    }

    [Then("The customer should be created successfully")]
    public void ThenTheCustomerShouldBeCreatedSuccessfully()
    {
        //TODO: check the customer created in DB by querying the read model
        _scenarioContext.Pending();
    }
}