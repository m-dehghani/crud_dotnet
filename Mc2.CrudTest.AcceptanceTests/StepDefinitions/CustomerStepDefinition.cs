using Mc2.CrudTest.Presentation.Server.DomainServices;
using Mc2.CrudTest.Presentation.Server.Handlers;
using Mc2.CrudTest.Presentation.Shared.Commands;
using Mc2.CrudTest.Presentation.Shared.ViewModels;
using ReqnrollProject1.Drivers;
using Assert = Xunit.Assert;


namespace ReqnrollProject1.StepDefinitions;

[Binding]
public sealed class CustomerStepDefinition
{
    private Dictionary<string, string> _errors = new();

    private readonly CustomerDriver _customerDriver;

    private CreateCustomerCommand? _command;

    private string _errorMessage =string.Empty;

    private readonly Xunit.Abstractions.ITestOutputHelper _testOutputHelper;
    
    public CustomerStepDefinition(Xunit.Abstractions.ITestOutputHelper testOutputHelper
        , ScenarioContext scenarioContext, CustomerDriver customerDriver)
    {
        _customerDriver = customerDriver;

        _testOutputHelper = testOutputHelper;
    }

    [When("I create a Customer with following details")]
    public async Task WhenICreateACustomerWithTheFollowingDetails(Table table)
    {
        Dictionary<string, string> dictionary = table.Rows.ToDictionary(row => row[0], row => row[1]);

        _command = new CreateCustomerCommand(dictionary["FirstName"], dictionary["LastName"], dictionary["DateOfBirth"], dictionary["PhoneNumber"], dictionary["Email"], dictionary["BankAccountNumber"]);

        SlowerCustomerService customerService = new(null);
      
        CreateCustomerCommandHandler handler = new(customerService);
       
        await handler.Handle(_command, new CancellationToken());
    }
    

    [Then(@"user should receive following error codes")]
    public void ThenUserShouldReceiveFollowingErrorCodes(Table table)
    {
        Dictionary<string, string> definedErrors = table.CreateInstance<Dictionary<string,String>>();
       
        definedErrors.Values.Should().Contain(_errorMessage);
    }

    [Then(@"user can send query and receive (.*) record of customer with following data")]
    public async Task ThenUserCanSendQueryAndReceiveRecordOfCustomerWithFollowingData(int numberOfCustomer, Table table)
    {
        CustomerViewModel customerToCreate = 
            table.CreateInstance<CustomerViewModel>();
            
        List<CustomerViewModel> customerList = 
            await _customerDriver.GetAllCustomers();
          
        customerList.Should().NotBeNull();
        
        Assert.Equal(numberOfCustomer, customerList.Count);
        
        CustomerViewModel? customer = customerList.FirstOrDefault();

        Assert.Multiple(
             () => customer?.FirstName.Should().Be(customerToCreate.FirstName),
             () => customer?.LastName.Should().Be(customerToCreate.LastName),
             () => customer?.Email.Should().Be(customerToCreate.Email),
             () => customer?.DateOfBirth.Should().Be(customerToCreate.DateOfBirth),
             () => customer?.PhoneNumber.Should().Be(customerToCreate.PhoneNumber)
         );
    }

    [When(@"user send command to update customer with email of ""(.*)"" and with below information")]
    public async Task WhenUserSendCommandToUpdateCustomerWithEmailOfAndWithBelowInformation(string p0, Table table)
    {
        CustomerUpdateViewModel customer = 
            table.CreateInstance<CustomerUpdateViewModel>();
            
        List<CustomerViewModel> customers = 
            await _customerDriver.GetAllCustomers();
        try
        {
            await _customerDriver.UpdateCustomer(
                new UpdateCustomerCommand(customers.FirstOrDefault()!.Id, customer.FirstName,
                    customer.LastName, customer.PhoneNumber, customer.Email,
                    customer.BankAccount, customer.DateOfBirth));
        }
        catch(Exception ex)
        {
            _errorMessage = ex.Message;
        }
    }

    [When(@"When user send command to create new customer with following information")]
    public async Task WhenWhenUserSendCommandToCreateNewCustomerWithFollowingInformation(Table table)
    {
        CustomerViewModel customerToCreate =
            table.CreateInstance<CustomerViewModel>();

        await _customerDriver.CreateCustomer(
            new CreateCustomerCommand(customerToCreate.FirstName, 
                customerToCreate.LastName,
            customerToCreate.DateOfBirth, 
                customerToCreate.PhoneNumber, 
                customerToCreate.Email,
            customerToCreate.BankAccount));
        
    }

    [Given(@"platform has ""(.*)"" record of customers")]
    public async Task GivenPlatformHasRecordOfCustomers(string p0)
    {
        await _customerDriver.ResetDb();
    }

    [Given(@"platform support following error codes")]
    public void GivenPlatformSupportFollowingErrorCodes(Table table)
    {
        _errors = new Dictionary<string, string>
        {
            {"101", "Invalid Email"},
            {"102", "Invalid PhoneNumber"},
            {"103", "Invalid BankAccountNumber"},
            {"104", "Invalid DateOfBirth"},
            {"201", "Duplicated Email Address"},
            {"202", "Duplicated Firstname, Lastname"},
            
        };
        _testOutputHelper.WriteLine(string.Join(',' ,_errors.Keys));
    }

   
}