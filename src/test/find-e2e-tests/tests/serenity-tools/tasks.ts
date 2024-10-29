import { Answerable, Task} from '@serenity-js/core';
import { Navigate,Click, Enter, Page } from '@serenity-js/web';
import { isstartButtonVisible,canAPostcodeBeEntered,isPostcodeSearchButtonVisible} from './questions';


export const navigateToFind = () =>
    Task.where(`#actor navigates to the Find Website`,
        Navigate.to('/'),
        )

export const clickOnTheStartButton = (): Task =>
        Task.where(`#actor clicks on the start button on the Find Landing Page`,
            Click.on(isstartButtonVisible()),
        )    

export const enterPostcodeAndSearch = (postcodeInputValue: Answerable<string>): Task =>
        Task.where(`#actor enters a postcode ${ postcodeInputValue } and searches for LA services within that area`,
            Enter.theValue(postcodeInputValue).into(canAPostcodeBeEntered()),
            Click.on(isPostcodeSearchButtonVisible()),
        )
    
    