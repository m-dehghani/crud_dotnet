import { test, expect } from '@playwright/test';

test('CustomerHistoryE2ETest', async ({ page }) => {
  await page.goto('https://localhost:57872/customers');
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
  await page.goto('https://localhost:57872/Customers');
});