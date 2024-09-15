import { test, expect } from '@playwright/test';

test('CreateCustomerE2ETest', async ({ page }) => {
  await page.goto('https://localhost:5090/customers');
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
});


test('editJohnDoe', async ({ page }) => {
  await page.goto('http://localhost:5090/customers');
  await page.getByRole('button', { name: 'Edit' }).click();
  await page.getByLabel('First Name').click();
  await page.getByLabel('First Name').fill('Johnny');
  await page.getByRole('button', { name: 'Update' }).click();
  await page.goto('http://localhost:5090/customers');
  await expect(page.locator('tbody')).toContainText('Johnny');
  await expect(page.locator('tbody')).toContainText('a@a.com');
});