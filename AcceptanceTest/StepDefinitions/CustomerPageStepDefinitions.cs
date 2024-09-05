using AcceptanceTest.Services;
using AngleSharp.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcceptanceTest.StepDefinitions
{
    [Binding]
    public class CustomerPageStepDefinitions(IPageService pageService)
    {
        private readonly IPageService _pageService = pageService;

        [Given("The customer page is loaded")]
        public async Task GivenTheReqnrollPageIsLoadedAsync()
        {
            await _pageService.CustomerPage.GoToPageAsync();
        }

        [When("Customer has created with the following information")]
        public async Task WhenTheSupportButtonIsClicked(DataTable dataTable)
        {
            await _pageService.CustomerPage.CreateCustomer(dataTable);
        }

        [Then("The following customer is visible")]
        public async Task ThenTheFollowingCustomerIsVisible(DataTable dataTable)
        {
            Dictionary<string, string>? tableRows = dataTable.ToDictionary();
           
              await _pageService.CustomerPage.TextContainsGivenValueAsync(dataTable);
                
           
        }
    }
}
