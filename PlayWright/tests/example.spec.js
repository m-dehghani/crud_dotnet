// @ts-check
const { test, expect } = require('@playwright/test');

// test('has title', async ({ page }) => {
//   await page.goto('https://playwright.dev/');

//   // Expect a title "to contain" a substring.
//   await expect(page).toHaveTitle(/Playwright/);
// });

// test('get started link', async ({ page }) => {
//   await page.goto('https://playwright.dev/');

//   // Click the get started link.
//   await page.getByRole('link', { name: 'Get started' }).click();

//   // Expects page to have a heading with the name of Installation.
//   await expect(page.getByRole('heading', { name: 'Installation' })).toBeVisible();
// });


const CustomerPageUrl = 'https://localhost:7239/customers';

test("CRUD on a new customer", async({page}) => {
  await page.goto(CustomerPageUrl);
  await page.getByRole('link', { name: 'New Customer' }).click();
  await page.getByPlaceholder('First name').click();
  await page.getByPlaceholder('First name').fill('John');
  await page.getByPlaceholder('First name').press('Tab');
  await page.getByPlaceholder('Last name').fill('Doe');
  await page.getByPlaceholder('Last name').press('Tab');
  await page.getByPlaceholder('Email').fill('a@a.com');
  await page.getByPlaceholder('Email').press('Tab');
  await page.getByPlaceholder('Phone Number').fill('+989010596159');
  await page.getByPlaceholder('Phone Number').press('Tab');
  await page.getByPlaceholder('Date Of Birth').press('ArrowRight');
  await page.getByPlaceholder('Date Of Birth').press('ArrowRight');
  await page.getByPlaceholder('Date Of Birth').fill('1995-01-01');
  await page.getByPlaceholder('Date Of Birth').press('Tab');
  await page.getByPlaceholder('Bank Account').fill('123456');
  await page.getByRole('button', { name: 'Create' }).click();
  await expect(page.locator('tbody')).toContainText('John');
  await expect(page.locator('tbody')).toContainText('a@a.com');
  await expect(page.getByRole('article')).toContainText('New Customer');


  await page.goto(CustomerPageUrl);
  await page.getByRole('button', { name: 'Edit' }).click();
  await page.getByLabel('First Name').click();
  await page.getByLabel('First Name').fill('Johnny');
  await page.getByRole('button', { name: 'Update' }).click();
  await page.goto(CustomerPageUrl);
  await expect(page.locator('tbody')).toContainText('Johnny');
  await expect(page.locator('tbody')).toContainText('a@a.com');

  await page.goto(CustomerPageUrl);
  await page.getByRole('button', { name: 'History' }).click();
  await expect(page.getByText('CustomerCreatedEvent')).toBeVisible();
  await expect(page.getByText('CustomerUpdatedEvent')).toBeVisible();
  await expect(page.getByRole('heading', { name: 'Customer History' })).toBeVisible();
  await expect(page.getByText('a@a.com').first()).toBeVisible();
  await expect(page.getByText('a@a.com').nth(1)).toBeVisible();
  await expect(page.getByText('Doe').first()).toBeVisible();
  await expect(page.getByText('Doe').nth(1)).toBeVisible();
  await expect(page.getByText('John', { exact: true })).toBeVisible();
  await expect(page.getByText('Johnny')).toBeVisible();
  await expect(page.getByText('123456').first()).toBeVisible();
  await expect(page.getByText('123456').nth(1)).toBeVisible();
  await page.getByRole('button', { name: 'Back' }).click();

  await page.getByText('About New Customer History').click();
  await page.getByRole('button', { name: 'Delete' }).click();
  await page.goto(CustomerPageUrl);


  await page.goto(CustomerPageUrl);
  await page.getByRole('button', { name: 'History' }).click();
  await expect(page.getByText('CustomerCreatedEvent')).toBeVisible();
  await expect(page.getByText('CustomerUpdatedEvent')).toBeVisible();
  await expect(page.getByRole('heading', { name: 'Customer History' })).toBeVisible();
  await expect(page.getByText('a@a.com').first()).toBeVisible();
  await expect(page.getByText('a@a.com').nth(1)).toBeVisible();
  await expect(page.getByText('Doe').first()).toBeVisible();
  await expect(page.getByText('Doe').nth(1)).toBeVisible();
  await expect(page.getByText('John', { exact: true })).toBeVisible();
  await expect(page.getByText('Johnny')).toBeVisible();
  await expect(page.getByText('123456').first()).toBeVisible();
  await expect(page.getByText('123456').nth(1)).toBeVisible();
  await page.getByRole('button', { name: 'Back' }).click();

  await page.getByRole('button', { name: 'Delete' }).click();
  await page.goto(CustomerPageUrl);
  await expect(page.getByText('Doe').nth(1)).not.toBeVisible();
  await expect(page.getByText('John', { exact: true })).not.toBeVisible();

});

