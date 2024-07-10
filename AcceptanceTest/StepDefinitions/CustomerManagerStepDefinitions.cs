using AcceptanceTest.Drivers;
using Mc2.CrudTest.Presentation.Shared;
using Mc2.CrudTest.Presentation.Shared.Commands;
using Mc2.CrudTest.Presentation.Shared.Entities;
using Mc2.CrudTest.Presentation.ViewModels;

namespace AcceptanceTest.StepDefinitions
{
    [Binding]
    public class CustomerManagerStepDefinitions(CustomerDriver customerDriver)
    {
        List<ErrorCodes> errors = new List<ErrorCodes>();

        [Given("platform support following error codes")]
        public void GivenPlatformSupportFollowingErrorCodes(DataTable dataTable)
        {
            errors = dataTable.CreateSet<ErrorCodes>().ToList();
        }

        [Given("platform has {string} record of customers")]
        public async void GivenPlatformHasRecordOfCustomers(string p0)
        {
            await customerDriver.ResetDb();
        }

        [When("When user send command to create new customer with following information")]
        public async Task Whenusersendcommandtocreatenewcustomerwithfollowinginformation(DataTable dataTable)
        {

            List<ErrorCodes> errors = dataTable.CreateSet<ErrorCodes>().ToList();
            CustomerViewModel customerToCreate = dataTable.CreateInstance<CustomerViewModel>();


           Exception ex = await Assert.ThrowsAsync(customerDriver.CreateCustomer(new CreateCustomerCommand(customerToCreate.FirstName, customerToCreate.LastName, customerToCreate.DateOfBirth, customerToCreate.PhoneNumber, customerToCreate.Email, customerToCreate.BankAccount)));

            ex.Message.Should().Be(errors.ToArray()[0].ErrorCode.ToString());
        }

        [Then("user can send query and receive (.*) record of customer with following data")]
        public async Task ThenTheCustomerShouldBeCreatedSuccessfully(DataTable dataTable)
        {
            CustomerViewModel customerToCreate = dataTable.CreateInstance<CustomerViewModel>();
            
            List<CustomerViewModel> customerList = await customerDriver.GetAllCustomers();
          
            customerList.Should().NotBeNull();
          
            CustomerViewModel? customer = customerList.FirstOrDefault();
            
            Assert.Multiple(
                () => customer?.FirstName.Should().Be(customerToCreate.FirstName),
                () => customer?.LastName.Should().Be(customerToCreate.LastName),
                () => customer?.Email.Should().Be(customerToCreate.Email),
                () => customer?.DateOfBirth.Should().Be(customerToCreate.DateOfBirth),
                () => customer?.PhoneNumber.Should().Be(customerToCreate.PhoneNumber)
            );
        }

        [When("user send command to update customer with email of (.*) and with below information")]
        public async Task UserSendCommandToUpdateCustomerWithEmailOfWithBelowInformation(DataTable dataTable)
        {
            CustomerUpdateViewModel customer = dataTable.CreateInstance<CustomerUpdateViewModel>();
            
            List<CustomerViewModel> customers = await customerDriver.GetAllCustomers();
            
            await customerDriver.UpdateCustomer(new UpdateCustomerCommand(customers.FirstOrDefault()!.Id, customer.FirstName, customer.LastName, customer.PhoneNumber, customer.Email, customer.BankAccount,customer.DateOfBirth));
        }


      
    }
}
