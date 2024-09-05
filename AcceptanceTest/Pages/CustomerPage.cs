using AcceptanceTest.Services;
using Mc2.CrudTest.Presentation.ViewModels;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Routing;
using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AcceptanceTest.Settings;
using Microsoft.Playwright.NUnit;
using NUnit.Framework;

namespace AcceptanceTest.Pages
{
    [Parallelizable(ParallelScope.Self)]
    [TestFixture]
    public class CustomerPage(IPageDependencyService pageDependencyService, AppSettings appSettings): PageTest
    {
        private readonly IPageDependencyService _pageDependencyService = pageDependencyService;
        private readonly AppSettings _appSettings = appSettings;
        public async Task GoToPageAsync()
        {
            await _pageDependencyService.Page.Result.GotoAsync(_pageDependencyService.AppSettings.Value.UiUrl);
        }

        internal async Task CreateCustomer(DataTable dataTable)
        {
            await GoToPageAsync();
            CustomerViewModel customer = new DataTable().CreateInstance<CustomerViewModel>();
            await Page.GetByRole(AriaRole.Link, new PageGetByRoleOptions() { NameString = "New Customer" }).ClickAsync();
            await Page.GetByPlaceholder("First name").ClickAsync();
            await Page.GetByPlaceholder("First name").FillAsync(customer.FirstName);
            await Page.GetByPlaceholder("First name").PressAsync("Tab");
            await Page.GetByPlaceholder("Last name").FillAsync(customer.LastName);
            await Page.GetByPlaceholder("Last name").PressAsync("Tab");
            await Page.GetByPlaceholder("Email").FillAsync(customer.Email);
            await Page.GetByPlaceholder("Email").PressAsync("Tab");
            await Page.GetByPlaceholder("Phone Number").FillAsync(customer.PhoneNumber);
            await Page.GetByPlaceholder("Phone Number").PressAsync("Tab");
            await Page.GetByPlaceholder("Date Of Birth").PressAsync("ArrowRight");
            await Page.GetByPlaceholder("Date Of Birth").PressAsync("ArrowRight");
            await Page.GetByPlaceholder("Date Of Birth").FillAsync(customer.DateOfBirth);
            await Page.GetByPlaceholder("Date Of Birth").PressAsync("Tab");
            await Page.GetByPlaceholder("Bank Account").FillAsync(customer.BankAccount);
            await Page.GetByRole(AriaRole.Button, new PageGetByRoleOptions() { NameString = "Create" }).ClickAsync();
        }

        public ILocator StrongTexts => _pageDependencyService.Page.Result.Locator("strong");

        public async Task TextContainsGivenValueAsync(DataTable dataTable)
        {
            CustomerViewModel customer = new DataTable().CreateInstance<CustomerViewModel>();
            await Expect(Page.Locator("tbody")).ToContainTextAsync(customer.FirstName);
            await Expect(Page.Locator("tbody")).ToContainTextAsync(customer.Email);
            await Expect(Page.GetByRole(AriaRole.Article)).ToContainTextAsync("New Customer");
        }

        public async Task UpdateCustomer(DataTable dataTable)
        {
            CustomerViewModel customer = new DataTable().CreateInstance<CustomerViewModel>();
            await GoToPageAsync();
            await Page.GetByRole(AriaRole.Button, new PageGetByRoleOptions() { NameString = "Edit" }).ClickAsync();
            await Page.GetByLabel("First Name").ClickAsync();
            await Page.GetByLabel("First Name").FillAsync(customer.FirstName);
            await Page.GetByRole(AriaRole.Button, new PageGetByRoleOptions() { NameString = "Update" }).ClickAsync();
            await Page.GotoAsync(_appSettings.UiUrl);
            await Expect(Page.Locator("tbody")).ToContainTextAsync(customer.FirstName);
            await Expect(Page.Locator("tbody")).ToContainTextAsync(customer.Email);
        }

        public async Task DeleteCustomer(DataTable dataTable)
        {
            await GoToPageAsync();
            await Page.GetByRole(AriaRole.Button, new PageGetByRoleOptions() { NameString = "Delete" }).ClickAsync();
            await Page.GotoAsync(_appSettings.UiUrl);
            await Expect(Page.GetByText("Doe").Nth(1)).Not.ToBeVisibleAsync();
            await Expect(Page.GetByText("John", new PageGetByTextOptions() { Exact = true })).Not.ToBeVisibleAsync();
        }

        public async Task DoingCrudOperationInCustomerPage(IPage Page)
        {
            await GoToPageAsync();
            await Page.GetByRole(AriaRole.Link, new PageGetByRoleOptions() { NameString = "New Customer" }).ClickAsync();
            await Page.GetByPlaceholder("First name").ClickAsync();
            await Page.GetByPlaceholder("First name").FillAsync("John");
            await Page.GetByPlaceholder("First name").PressAsync("Tab");
            await Page.GetByPlaceholder("Last name").FillAsync("Doe");
            await Page.GetByPlaceholder("Last name").PressAsync("Tab");
            await Page.GetByPlaceholder("Email").FillAsync("a@a.com");
            await Page.GetByPlaceholder("Email").PressAsync("Tab");
            await Page.GetByPlaceholder("Phone Number").FillAsync("+989010596159");
            await Page.GetByPlaceholder("Phone Number").PressAsync("Tab");
            await Page.GetByPlaceholder("Date Of Birth").PressAsync("ArrowRight");
            await Page.GetByPlaceholder("Date Of Birth").PressAsync("ArrowRight");
            await Page.GetByPlaceholder("Date Of Birth").FillAsync("1995-01-01");
            await Page.GetByPlaceholder("Date Of Birth").PressAsync("Tab");
            await Page.GetByPlaceholder("Bank Account").FillAsync("123456");
            await Page.GetByRole(AriaRole.Button, new PageGetByRoleOptions() { NameString = "Create" }).ClickAsync();
            await Expect(Page.Locator("tbody")).ToContainTextAsync("John");
            await Expect(Page.Locator("tbody")).ToContainTextAsync("a@a.com");
            await Expect(Page.GetByRole(AriaRole.Article)).ToContainTextAsync("New Customer");


            await Page.GotoAsync(_appSettings.UiUrl);
            await Page.GetByRole(AriaRole.Button, new PageGetByRoleOptions() { NameString = "Edit" }).ClickAsync();
            await Page.GetByLabel("First Name").ClickAsync();
            await Page.GetByLabel("First Name").FillAsync("Johnny");
            await Page.GetByRole(AriaRole.Button, new PageGetByRoleOptions() { NameString = "Update" }).ClickAsync();
            await Page.GotoAsync(_appSettings.UiUrl);
            await Expect(Page.Locator("tbody")).ToContainTextAsync("Johnny");
            await Expect(Page.Locator("tbody")).ToContainTextAsync("a@a.com");

            await Page.GotoAsync(_appSettings.UiUrl);
            await Page.GetByRole(AriaRole.Button, new PageGetByRoleOptions() { NameString = "History" }).ClickAsync();
            await Expect(Page.GetByText("CustomerCreatedEvent")).ToBeVisibleAsync();
            await Expect(Page.GetByText("CustomerUpdatedEvent")).ToBeVisibleAsync();
            await Expect(Page.GetByRole(AriaRole.Heading, new PageGetByRoleOptions() { NameString = "Customer History" }))
                .ToBeVisibleAsync();
            await Expect(Page.GetByText("a@a.com").First).ToBeVisibleAsync();
            await Expect(Page.GetByText("a@a.com").Nth(1)).ToBeVisibleAsync();
            await Expect(Page.GetByText("Doe").First).ToBeVisibleAsync();
            await Expect(Page.GetByText("Doe").Nth(1)).ToBeVisibleAsync();
            await Expect(Page.GetByText("John", new PageGetByTextOptions() { Exact = true })).ToBeVisibleAsync();
            await Expect(Page.GetByText("Johnny")).ToBeVisibleAsync();
            await Expect(Page.GetByText("123456").First).ToBeVisibleAsync();
            await Expect(Page.GetByText("123456").Nth(1)).ToBeVisibleAsync();
            await Expect(Page.GetByRole(AriaRole.Button, new PageGetByRoleOptions() { NameString = "Back" }))
                .ToBeVisibleAsync();

            await Page.GetByText("About New Customer History").ClickAsync();
            await Expect(Page.GetByRole(AriaRole.Button, new PageGetByRoleOptions() { NameString = "Delete" }))
                .ToBeVisibleAsync();
            await Page.GotoAsync(_appSettings.UiUrl);


            await Page.GotoAsync(_appSettings.UiUrl);
            await Page.GetByRole(AriaRole.Button, new PageGetByRoleOptions() { NameString = "History" }).ClickAsync();
            await Expect(Page.GetByText("CustomerCreatedEvent")).ToBeVisibleAsync();
            await Expect(Page.GetByText("CustomerUpdatedEvent")).ToBeVisibleAsync();
            await Expect(Page.GetByRole(AriaRole.Heading, new PageGetByRoleOptions() { NameString = "Customer History" }))
                .ToBeVisibleAsync();
            await Expect(Page.GetByText("a@a.com").First).ToBeVisibleAsync();
            await Expect(Page.GetByText("a@a.com").Nth(1)).ToBeVisibleAsync();
            await Expect(Page.GetByText("Doe").First).ToBeVisibleAsync();
            await Expect(Page.GetByText("Doe").Nth(1)).ToBeVisibleAsync();
            await Expect(Page.GetByText("John", new PageGetByTextOptions() { Exact = true })).ToBeVisibleAsync();
            await Expect(Page.GetByText("Johnny")).ToBeVisibleAsync();
            await Expect(Page.GetByText("123456").First).ToBeVisibleAsync();
            await Expect(Page.GetByText("123456").Nth(1)).ToBeVisibleAsync();
            await Page.GetByRole(AriaRole.Button, new PageGetByRoleOptions() { NameString = "Back" }).ClickAsync();

            await Page.GetByRole(AriaRole.Button, new PageGetByRoleOptions() { NameString = "Delete" }).ClickAsync();
            await Page.GotoAsync(_appSettings.UiUrl);
            await Expect(Page.GetByText("Doe").Nth(1)).Not.ToBeVisibleAsync();
            await Expect(Page.GetByText("John", new PageGetByTextOptions() { Exact = true })).Not.ToBeVisibleAsync();

        }

       
    }
}
