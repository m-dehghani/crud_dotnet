namespace Mc2.CrudTest.AcceptanceTests.old.Steps;

[Binding]
public sealed class CustomerStepDefinition
{
    
    private readonly ScenarioContext _scenarioContext;
    public CustomerStepDefinition(ScenarioContext scenarioContext)
    {
    }

    [Given("I am an operator")]
    public async Task  GivenIAmAnOperator()
    {
        //IServiceProvider s = new ServiceProvider(typeof(CustomerStepDefinition),);
        // var customerController = new CustomerController( new Mediator(_serviceProvider));


        // await Task.FromResult(Task.CompletedTask);
        // _scenarioContext.Pending();
    }


    [When("I create a Customer with following details")]
    public async Task WhenICreateACustomerWithTheFollowingDetails(Table table)
    {
        //TODO: create a customer with table data and calling the CustomerCreatedHandler for this step
      
    }

    [Then("The customer should be created successfully")]
    public async void ThenTheCustomerShouldBeCreatedSuccessfully()
    {
        //TODO: check the customer created in DB by querying the read model
      
    }
}