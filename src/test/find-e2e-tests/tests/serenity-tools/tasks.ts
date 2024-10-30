import { Answerable, Task } from '@serenity-js/core';
import { Navigate, Click, Enter } from '@serenity-js/web';
import { startButton, postcodeSearchBox, postcodeSearchButton } from './pageObjects';
import { isTheFindPageDisplayed } from './questions';

export const navigateToFind = (): Task =>
    Task.where(
        `#actor navigates to the Find Website`,
        Navigate.to('/'),
        isTheFindPageDisplayed(),
    );

export const clickOnTheStartButton = (): Task =>
    Task.where(
        `#actor clicks on the start button on the Find Landing Page`,
        Click.on(startButton()),
    );

export const enterPostcode = (postcodeInputValue: Answerable<string>): Task =>
    Task.where(
        `#actor enters a postcode ${postcodeInputValue} and searches for LA services within that area`,
        Enter.theValue(postcodeInputValue).into(postcodeSearchBox()),
    );

export const clickOnPostcodeSearchButton = (): Task =>
    Task.where(
        `#actor clicks on the search button on the enter postcode Page`,
        Click.on(postcodeSearchButton()),
    );

