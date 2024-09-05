using AcceptanceTest.Drivers;
using IdentityModel.OidcClient;
using k8s.KubeConfigModels;
using Mc2.CrudTest.Presentation.Server.Pages;
using Mc2.CrudTest.Presentation.Shared;
using Mc2.CrudTest.Presentation.Shared.Commands;
using Mc2.CrudTest.Presentation.Shared.Entities;
using Mc2.CrudTest.Presentation.Shared.Queries;
using Mc2.CrudTest.Presentation.ViewModels;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace AcceptanceTest.StepDefinitions
{
    [Binding]
    public class CustomerManagerStepDefinitions(CustomerDriver customerDriver)
    {
        private List<ErrorCode> _errors = [];
        private readonly List<ErrorCode> _result = [];
        private readonly int _numberOfRecordToBeInDb = 0;
      
        [Given("platform support following error codes")]
        public void GivenPlatformSupportFollowingErrorCodes(DataTable dataTable)
        {
            _errors = dataTable.CreateSet<ErrorCode>().ToList();
        }

        [Given("platform has {string} record of customers")]
        public async void GivenPlatformHasRecordOfCustomers(string p0)
        {
            await customerDriver.ResetDb();
        }

        [When("When user send command to create new customer with following information")]
        public async Task Whenusersendcommandtocreatenewcustomerwithfollowinginformation(DataTable dataTable)
        {
            CustomerViewModel customerToCreate = dataTable.CreateInstance<CustomerViewModel>();

            await customerDriver.Create(
                new CreateCustomerCommand(customerToCreate.FirstName, customerToCreate.LastName
                    , customerToCreate.DateOfBirth, customerToCreate.PhoneNumber
                    , customerToCreate.Email, customerToCreate.BankAccount));
        }

        [Then("user can send query and receive (.*) record of customer with following data")]
        public async Task ThenTheCustomerShouldBeCreatedSuccessfully(DataTable dataTable)
        {
            CustomerViewModel customerToCreate = dataTable.CreateInstance<CustomerViewModel>();
            
            List<CustomerViewModel> customerList = await customerDriver.GetAll();
            if (_numberOfRecordToBeInDb == 0)
            {
                customerList.Should().BeEmpty();
            }
            else
            {
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
        }

        [When("When user send command to create new customer with following information")]
        public async Task WhenUserSendCommandToCreateNewCustomerWithFollowingInformation(DataTable dataTable)
        {
            CustomerViewModel customerToCreate = dataTable.CreateInstance<CustomerViewModel>();
            CreateCustomerCommand createCustomerCommand = new CreateCustomerCommand(customerToCreate.FirstName, customerToCreate.LastName,
                customerToCreate.DateOfBirth, customerToCreate.PhoneNumber,
                customerToCreate.Email, customerToCreate.BankAccount);
            await customerDriver.Create(createCustomerCommand);

        }

        [Then("Then user should receive following error codes")]
        public void ThenUserShouldReceiveFollowingErrorCodes(DataTable dataTable)
        {
            int[] errorsToCatch = dataTable.CreateSet<int>().ToArray();

            _result.Select(r => r.Code).Should().ContainInOrder(errorsToCatch);
        }

        [When("user send command to update customer with email of (.*) and with below information")]
        public async Task UserSendCommandToUpdateCustomerWithEmailOfWithBelowInformation(DataTable dataTable)
        {
            CustomerUpdateViewModel customer = dataTable.CreateInstance<CustomerUpdateViewModel>();
            
            List<CustomerViewModel> customers = await customerDriver.GetAll();
            
            await customerDriver.Update(
                new UpdateCustomerCommand(customers.FirstOrDefault()!.Id, customer.FirstName, customer.LastName, 
                    customer.PhoneNumber, customer.Email, customer.BankAccount,customer.DateOfBirth));
        }

        [When("When user send command to delete customer with email of (.*)")]
        public async Task WhenUserSendCommandToDeleteCustomerWithEmailOf(DataTable dataTable)
        {
            List<CustomerViewModel> customerList = await customerDriver.GetAll();

            Guid customerId = customerList.FirstOrDefault(c => 
                dataTable.CreateInstance<string>() != null && c.Email != null && c.Email == dataTable.CreateInstance<string>()).Id;

            await customerDriver.Delete(new DeleteCustomerCommand(customerId));
        }
    }
}
