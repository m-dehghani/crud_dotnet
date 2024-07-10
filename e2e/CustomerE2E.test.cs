using Microsoft.Playwright;
// Added for e2e testing
namespace e2e
{
    [Parallelizable(ParallelScope.Self)]
    [TestFixture]
    public class Tests : PageTest
    {
        private const string CustomerPageUrl = "https://localhost:7239/customers";

        [Test]
        public async Task DoingCrudOperationInCustomerPage()
        {
            await Page.GotoAsync(CustomerPageUrl);
            await Page.GetByRole(AriaRole.Link, new PageGetByRoleOptions() {NameString = "New Customer"}).ClickAsync();
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
            await Page.GetByRole(AriaRole.Button, new PageGetByRoleOptions() {NameString = "Create"}).ClickAsync();
            await Expect(Page.Locator("tbody")).ToContainTextAsync("John");
            await Expect(Page.Locator("tbody")).ToContainTextAsync("a@a.com");
            await Expect(Page.GetByRole(AriaRole.Article)).ToContainTextAsync("New Customer");


            await Page.GotoAsync(CustomerPageUrl);
            await Page.GetByRole(AriaRole.Button, new PageGetByRoleOptions() {NameString = "Edit"}).ClickAsync();
            await Page.GetByLabel("First Name").ClickAsync();
            await Page.GetByLabel("First Name").FillAsync("Johnny");
            await Page.GetByRole(AriaRole.Button, new PageGetByRoleOptions() {NameString = "Update"}).ClickAsync();
            await Page.GotoAsync(CustomerPageUrl);
            await Expect(Page.Locator("tbody")).ToContainTextAsync("Johnny");
            await Expect(Page.Locator("tbody")).ToContainTextAsync("a@a.com");

            await Page.GotoAsync(CustomerPageUrl);
            await Page.GetByRole(AriaRole.Button, new PageGetByRoleOptions() {NameString = "History"}).ClickAsync();
            await Expect(Page.GetByText("CustomerCreatedEvent")).ToBeVisibleAsync();
            await Expect(Page.GetByText("CustomerUpdatedEvent")).ToBeVisibleAsync();
            await Expect(Page.GetByRole(AriaRole.Heading, new PageGetByRoleOptions() {NameString = "Customer History"}))
                .ToBeVisibleAsync();
            await Expect(Page.GetByText("a@a.com").First).ToBeVisibleAsync();
            await Expect(Page.GetByText("a@a.com").Nth(1)).ToBeVisibleAsync();
            await Expect(Page.GetByText("Doe").First).ToBeVisibleAsync();
            await Expect(Page.GetByText("Doe").Nth(1)).ToBeVisibleAsync();
            await Expect(Page.GetByText("John", new PageGetByTextOptions() {Exact = true})).ToBeVisibleAsync();
            await Expect(Page.GetByText("Johnny")).ToBeVisibleAsync();
            await Expect(Page.GetByText("123456").First).ToBeVisibleAsync();
            await Expect(Page.GetByText("123456").Nth(1)).ToBeVisibleAsync();
            await Expect(Page.GetByRole(AriaRole.Button, new PageGetByRoleOptions() {NameString = "Back"}))
                .ToBeVisibleAsync();

            await Page.GetByText("About New Customer History").ClickAsync();
            await Expect(Page.GetByRole(AriaRole.Button, new PageGetByRoleOptions() {NameString = "Delete"}))
                .ToBeVisibleAsync();
            await Page.GotoAsync(CustomerPageUrl);


            await Page.GotoAsync(CustomerPageUrl);
            await Page.GetByRole(AriaRole.Button, new PageGetByRoleOptions() {NameString = "History"}).ClickAsync();
            await Expect(Page.GetByText("CustomerCreatedEvent")).ToBeVisibleAsync();
            await Expect(Page.GetByText("CustomerUpdatedEvent")).ToBeVisibleAsync();
            await Expect(Page.GetByRole(AriaRole.Heading, new PageGetByRoleOptions() {NameString = "Customer History"}))
                .ToBeVisibleAsync();
            await Expect(Page.GetByText("a@a.com").First).ToBeVisibleAsync();
            await Expect(Page.GetByText("a@a.com").Nth(1)).ToBeVisibleAsync();
            await Expect(Page.GetByText("Doe").First).ToBeVisibleAsync();
            await Expect(Page.GetByText("Doe").Nth(1)).ToBeVisibleAsync();
            await Expect(Page.GetByText("John", new PageGetByTextOptions() {Exact = true})).ToBeVisibleAsync();
            await Expect(Page.GetByText("Johnny")).ToBeVisibleAsync();
            await Expect(Page.GetByText("123456").First).ToBeVisibleAsync();
            await Expect(Page.GetByText("123456").Nth(1)).ToBeVisibleAsync();
            await Page.GetByRole(AriaRole.Button, new PageGetByRoleOptions() {NameString = "Back"}).ClickAsync();

            await Page.GetByRole(AriaRole.Button, new PageGetByRoleOptions() {NameString = "Delete"}).ClickAsync();
            await Page.GotoAsync(CustomerPageUrl);
            await Expect(Page.GetByText("Doe").Nth(1)).Not.ToBeVisibleAsync();
            await Expect(Page.GetByText("John", new PageGetByTextOptions() {Exact = true})).Not.ToBeVisibleAsync();

        }
    }
}
