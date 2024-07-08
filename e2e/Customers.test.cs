using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Playwright.NUnit;
using Microsoft.Playwright;

namespace e2e
{
    [Parallelizable(ParallelScope.Self)]
    [TestFixture]
    public class CustomerTests : PageTest
    {
        [Test]
        public async Task CreateANewCustomer()
        {
            await Page.GetByRole(AriaRole.Link, new() {NameString = "New Customer" }).ClickAsync();
            await Page.WaitForURLAsync("https://localhost:7045/create-customer");

            await Page.GetByPlaceholder("First name").ClickAsync();

            await Page.GetByPlaceholder("First name").FillAsync("a");

            await Page.GetByPlaceholder("First name").PressAsync("Tab");

            await Page.GetByPlaceholder("Last name").FillAsync("a");

            await Page.GetByPlaceholder("Last name").PressAsync("Tab");

            await Page.GetByPlaceholder("Email").FillAsync("a@a.com");

            await Page.GetByPlaceholder("Email").PressAsync("Tab");

            await Page.GetByPlaceholder("Phone Number").FillAsync("+989010596159");

            await Page.GetByPlaceholder("Phone Number").PressAsync("Tab");

            await Page.GetByPlaceholder("Date Of Birth").PressAsync("ArrowRight");

            await Page.GetByPlaceholder("Date Of Birth").PressAsync("ArrowRight");

            await Page.GetByPlaceholder("Date Of Birth").FillAsync("1980-01-01");

            await Page.GetByPlaceholder("Date Of Birth").PressAsync("Tab");

            await Page.GetByPlaceholder("Bank Account").FillAsync("65487161");

            await Page.GetByRole(AriaRole.Button, new() { NameString = "Create" }).ClickAsync();
           
            await Page.WaitForURLAsync("https://localhost:7045/customers");
            
            await Expect(Page).ToHaveURLAsync("https://localhost:7045/customers");
            await Expect(Page).ToHaveTitleAsync(new Regex("Playwright"));
        }


    }

}
