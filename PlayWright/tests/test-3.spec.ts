import { test, expect } from '@playwright/test';

test('test', async ({ page }) => {
  await page.goto('https://localhost:57872/customers');
  await page.getByRole('button', { name: 'Edit' }).click();
  await page.getByLabel('First Name').click();
  await page.getByLabel('First Name').fill('Johnny');
  await page.getByRole('button', { name: 'Update' }).click();
  await page.goto('https://localhost:57872/customers');
  await expect(page.locator('tbody')).toContainText('Johnny');
  await expect(page.locator('tbody')).toContainText('a@a.com');
});