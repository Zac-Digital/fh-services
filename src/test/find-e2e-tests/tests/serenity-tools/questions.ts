import { By, PageElement,} from '@serenity-js/web';

export const isstartButtonVisible = () =>
    PageElement
        .located(By.css("a[role='button']"))
        .describedAs('start Now Button');

export const canAPostcodeBeEntered = () =>
     PageElement
        .located(By.id("postcode"))
        .describedAs('the Postcode searchbox');

        
export const isPostcodeSearchButtonVisible = () =>
     PageElement
        .located(By.css("button[class='govuk-button']"))
        .describedAs('the search button');
