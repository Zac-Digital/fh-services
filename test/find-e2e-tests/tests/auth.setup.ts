import { test as setup } from '@playwright/test';
import path from 'path';
import {isTheFindPageDisplayed} from "./serenity-tools/find-questions";

const authFile = path.join(__dirname, '../playwright/.auth/user.json');

setup('Perform Basic Auth', async ({ page }) => {

    await page.goto(process.env.BASE_URL);
    isTheFindPageDisplayed()

    await page.context().storageState({ path: authFile });
});