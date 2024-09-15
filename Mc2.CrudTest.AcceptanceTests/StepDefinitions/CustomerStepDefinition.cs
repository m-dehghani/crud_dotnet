using Mc2.CrudTest.Presentation.Shared.Commands;
using MediatR;
using Moq;
using System;
using System.Diagnostics;
using Mc2.CrudTest.Presentation.Server.DomainServices;
using Mc2.CrudTest.Presentation.Server.Handlers;
using Mc2.CrudTest.Presentation.Server.Infrastructure;

namespace ReqnrollProject1.StepDefinitions;

[Binding]
public sealed class CustomerStepDefinition
{
    private readonly Mock<IMediator> _mediator = new Mock<IMediator>();
    
    private CreateCustomerCommand? _command;

    public CustomerStepDefinition(ScenarioContext scenarioContext)
    {
    }

    [Given("I am an operator")]
    public static Task  GivenIAmAnOperator()
    {
        Debug.WriteLine("Staring ...");
        return Task.CompletedTask;
        //IServiceProvider s = new ServiceProvider(typeof(CustomerStepDefinition),);
        // var customerController = new CustomerController( new Mediator(_serviceProvider));


        // await Task.FromResult(Task.CompletedTask);
        // _scenarioContext.Pending();
    }


    [When("I create a Customer with following details")]
    public async Task WhenICreateACustomerWithTheFollowingDetails(Table table)
    {
        //TODO: create a customer with table data and calling the CustomerCreatedHandler for this step
        Dictionary<string, string>? dictionary = table.Rows.ToDictionary(row => row[0], row => row[1]);

        _command = new CreateCustomerCommand(dictionary["FirstName"], dictionary["LastName"], dictionary["DateOfBirth"], dictionary["PhoneNumber"], dictionary["Email"], dictionary["BankAccountNumber"]);

        SlowerCustomerService customerService = new SlowerCustomerService(null);
      
        CreateCustomerCommandHandler handler = new CreateCustomerCommandHandler(customerService);
       
        await handler.Handle(_command, new System.Threading.CancellationToken());
    }

    [Then("The customer should be created successfully")]
    public Task ThenTheCustomerShouldBeCreatedSuccessfully()
    {
        //TODO: check the customer created in DB by querying the read model
        //Arrange
        // _mediator.Verify(x => x.Publish(It.IsAny<CustomersChanged>()));
        return Task.CompletedTask;
    }

    [Then(@"user should receive following error codes")]
    public void ThenUserShouldReceiveFollowingErrorCodes(Table table)
    {
        ScenarioContext.StepIsPending();
    }

    [Then(@"user can send query and receive ""(.*)"" record of customer with following data")]
    public void ThenUserCanSendQueryAndReceiveRecordOfCustomerWithFollowingData(string p0, Table table)
    {
        ScenarioContext.StepIsPending();
    }

    [When(@"user send command to update customer with email of ""(.*)"" and with below information")]
    public void WhenUserSendCommandToUpdateCustomerWithEmailOfAndWithBelowInformation(string p0, Table table)
    {
        ScenarioContext.StepIsPending();
    }

    [When(@"When user send command to create new customer with following information")]
    public void WhenWhenUserSendCommandToCreateNewCustomerWithFollowingInformation(Table table)
    {
        ScenarioContext.StepIsPending();
    }

    [Given(@"platform has ""(.*)"" record of customers")]
    public void GivenPlatformHasRecordOfCustomers(string p0)
    {
        ScenarioContext.StepIsPending();
    }

    [Given(@"platform support following error codes")]
    public void GivenPlatformSupportFollowingErrorCodes(Table table)
    {
        ScenarioContext.StepIsPending();
    }
}