import {By, PageElement} from '@serenity-js/web';


export class AddAServicePageObjects {


    // Locator for the "Add a Service" link
    static addAServiceLink = () =>
        PageElement
            .located(By.xpath("//div[@class='govuk-width-container']//div[2]//div[1]//h3[1]//a[1]"))
            .describedAs('the Add an LA service Link');


    static manageServicesLink = () =>
        PageElement
            .located(By.xpath("//div[@class='govuk-width-container']//div[2]//div[2]//h3[1]//a[1]"))
            .describedAs('the Manage services Link');


    // Locator for the "Add a VCSService" link
    static addAVCSServiceLink = () =>
        PageElement
            .located(By.xpath("//a[@href='/manage-services/start-add-service?servicetype=Vcs']"))
            .describedAs('the Add a VCS service Link');


    static manageVcfsServicesLink = () =>
        PageElement
            .located(By.xpath("//a[@href='/manage-services?servicetype=Vcs']"))
            .describedAs('the Manage VCFS services Link');


    // Locator for the "Search and select the VCS authority area this service is in" field
    static searchAndSelectVCSOrgField = () =>
        PageElement
            .located(By.xpath("//input[@id='select']"))
            .describedAs('Search and select the VCS organisation Field');


    // Locator for the "Search and select the local authority area this service is in" field
    static searchAndSelectLaField = () =>
        PageElement
            .located(By.xpath("//input[@id='select']"))
            .describedAs('Search and select the LA organisation Field');


    //What is the service name? Page
    static whatIsTheServiceNameField = () =>
        PageElement
            .located(By.id("textbox"))
            .describedAs('the Enter a name of the service Field');


    static whatSupportDoesTheServiceOfferHealthCategory = () =>
        PageElement
            .located(By.id("category-1"))
            .describedAs('What support does the service offer? Activities Taxonomy');

    static whatSupportDoesTheServiceOfferSecondaryCategory = () =>
        PageElement
            .located(By.id("category-7"))
            .describedAs('What support does the service offer? Field-Activities sub group');


    //Give a description of the service
    static giveADescriptionOfTheServiceField = () =>
        PageElement
            .located(By.id("textarea"))
            .describedAs('Give a description of the service Field');


    //Is support offered
    static SupportRelatedToChildrenOrYoungPeopleFieldNo = () =>
        PageElement
            .located(By.id("ViewModel_Children_No"))
            .describedAs('No support is given');


    // Which language is the service offered in? Page
    static getServiceOfferedLanguagesField = () =>
        PageElement
            .located(By.xpath("//input[@id='language-0']"))
            .describedAs('Which language is the service offered in Field');


    //Does the service cost money to use? Page
    static ServicePaidForOptionNo = () =>
        PageElement
            .located(By.id("UserInput_HasCost_No"))
            .describedAs('No, this service does not require payment option');


    //How can people use this service?
    static inPersonServiceAccessDetails = () =>
        PageElement
            .located(By.id("checkbox-InPerson"))
            .describedAs('Service can be accessed via In Person Option');


    //Do you want to add any locations for this service? Page
    static addServiceLocation = () =>
        PageElement
            .located(By.id("radio-True"))
            .describedAs('Yes, add a location option');


    //Search and select an existing location to add to this service Page
    static searchAndSelectLocation = () =>
        PageElement
            .located(By.xpath("//input[@id='select']"))
            .describedAs('Search and select an existing location');


    //On which days can people use this service at ? Page
    static selectServiceAvailableMonday = () =>
        PageElement
            .located(By.id("checkbox-MO"))
            .describedAs('Selecting Monday');

    static selectServiceAvailableTuesday = () =>
        PageElement
            .located(By.id("checkbox-TU"))
            .describedAs('Selecting Tuesday');


    //Can you provide more details about using this service ? at a location Page

    static getServiceUsageDetailsYes = () =>
        PageElement
            .located(By.id("UserInput_HasDetails_Yes"))
            .describedAs('yes Support is given');


    static enterDetailAboutLocation = () =>
        PageElement
            .located(By.id("text-area"))
            .describedAs('Search and select an existing location');


    //Confirm Location page
    static addAnotherLocationButton = () =>
        PageElement
            .located(By.css("button[value='add']"))
            .describedAs('Add another location button');


    //How can people find out more about this service? Page
    static emailOptionToFindOutAboutService = () =>
        PageElement
            .located(By.id("contact-email"))
            .describedAs('Email option');

    static emailCorrespondenceToFindOutAboutService = () =>
        PageElement
            .located(By.id("email-text-box"))
            .describedAs('Email correspondence option');


    //Give more details about this service
    static provideMoreServiceDetailsTextField = () =>
        PageElement
            .located(By.id("textarea"))
            .describedAs('Give more details about this service Text Field');


    //Check the details and add service
    static confirmAndAddAServiceButton = () =>
        PageElement
            .located(By.css("button[class='govuk-button']"))
            .describedAs('Confirm details and add a service Button');


    //continue buttons
    static continueButtonForCreateService = () =>
        PageElement
            .located(By.id("textarea"))
            .describedAs('Continue Button');

    static continueButtonForAddALocationButton = () =>
        PageElement
            .located(By.css("button[value='continue']"))
            .describedAs('Continue Button');

    //confirm button to create a service
    static confirmAddAServiceButton = () =>
        PageElement
            .located(By.css("button[type='submit']"))
            .describedAs('Continue Button');


    //Check the details and add service
    static enterServiceNameTextAreaInManageService = () =>
        PageElement
            .located(By.xpath("//div[@class='moj-filter__selected']/div[2]/fieldset/input"))
            .describedAs('Enter a service Name into Text Area Field');


    static ApplyfilterInManageService = () =>
        PageElement
            .located(By.css("button[class='govuk-button']"))
            .describedAs('Apply filter button');


    static firstServiceNameTableEntryInManageService = () =>
        PageElement
            .located(By.xpath("//tbody/tr/td[1]"))
            .describedAs('First entry in the List of Services Table');


    static showFiltersButtonAddAService = () =>
        PageElement
            .located(By.css("#main-content > button"))
            .describedAs('Show Filters Button - Only available in Mobile View');
}
