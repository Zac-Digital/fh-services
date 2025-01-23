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


    // ******Locator for the "Add a VCSService" link
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


    ///***** */
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


    //What support does the service offer?  Page
    static whatSupportDoesTheServiceOfferPrimaryCategory = (categoryType) => {
        if (categoryType === "Activities") {
            return PageElement
                .located(By.id("category-1"))
                .describedAs('What support does the service offer? Activities Taxonomy');
        } else if (categoryType === "Health") {
            return PageElement
                .located(By.id("category-3"))
                .describedAs('What support does the service offer? Health Taxonomy');
        } else {
            throw new Error(`The primary category "Activities" could not be found.`);
        }
    };


    static whatSupportDoesTheServiceOfferSecondaryCategory = () =>
        PageElement
            .located(By.id("category-7"))
            .describedAs('What support does the service offer? Field-Activities sub group');


    //Give a description of the service
    static giveADescriptionOfTheServiceField = () =>
        PageElement
            .located(By.id("textarea"))
            .describedAs('Give a description of the service Field');

    //Is the support offered by this service related to children or young people?
    static isSupportRelatedToChildrenOrYoungPeopleField = (supportType) => {
        if (supportType === "Yes") {
            return PageElement
                .located(By.id("ViewModel_Children_Yes"))
                .describedAs('yes Support is given');
        } else if (supportType === "No") {
            return PageElement
                .located(By.id("ViewModel_Children_No"))
                .describedAs('No support is given');
        } else {
            throw new Error(`The selection does not exist`);
        }
    };


    // Which language is the service offered in? Page
    static getServiceOfferedLanguagesField = () =>
        PageElement
            .located(By.xpath("//input[@id='language-0']"))
            .describedAs('Which language is the service offered in Field');


    //Does the service cost money to use? Page
    static isServicePaid = (serviceCost) => {
        if (serviceCost === "Yes") {
            return PageElement
                .located(By.id("UserInput_HasCost_Yes"))
                .describedAs('Yes, this service requires a cost option');
        } else if (serviceCost === "No") {
            return PageElement
                .located(By.id("UserInput_HasCost_No"))
                .describedAs('No, this service does not require payment option');
        } else {
            throw new Error(`Selection does not exist.`);
        }
    };


    //How can people use this service?
    static selectServiceAccessDetails = (accessType) => {
        if (accessType === "In person") {
            return PageElement
                .located(By.id("checkbox-InPerson"))
                .describedAs('Service can be accessed via In Person Option');
        } else if (accessType === "Online") {
            return PageElement
                .located(By.id("checkbox-Online"))
                .describedAs('Service can be accessed via Online Option');
        } else if (accessType === "Telephone") {
            return PageElement
                .located(By.id("checkbox-Telephone"))
                .describedAs('Service can be accessed via Telephone Option');
        } else {
            throw new Error(`The selection does not exist`);
        }
    };


    //Do you want to add any locations for this service? Page
    static addServiceLocations = (supportType) => {
        if (supportType === "Yes") {
            return PageElement
                .located(By.id("radio-True"))
                .describedAs('Yes, add a location option');
        } else if (supportType === "No") {
            return PageElement
                .located(By.id("radio-False"))
                .describedAs('No, do not add a location option');
        } else {
            throw new Error(`The selection does not exist`);
        }
    };


    //Search and select an existing location to add to this service Page
    static searchAndSelectLocation = () =>
        PageElement
            .located(By.xpath("//input[@id='select']"))
            .describedAs('Search and select an existing location');


    //On which days can people use this service at ? Page
    static selectServiceAvailableDays = (serviceDay) => {
        if (serviceDay === "Monday") {
            return PageElement
                .located(By.id("checkbox-MO"))
                .describedAs('Selecting Monday');
        } else if (serviceDay === "Tuesday") {
            return PageElement
                .located(By.id("checkbox-TU"))
                .describedAs('Selecting Tuesday');
        } else {
            throw new Error(`Option does not exist`);
        }
    };

    //Can you provide more details about using this service ? at a location Page

    static getServiceUsageDetails = (supportType) => {
        if (supportType === "Yes") {
            return PageElement
                .located(By.id("UserInput_HasDetails_Yes"))
                .describedAs('yes Support is given');
        } else if (supportType === "No") {
            return PageElement
                .located(By.id("#UserInput_HasDetails_No"))
                .describedAs('No support is given option');
        } else {
            throw new Error(`Option does not exist`);
        }
    };


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
    static selectOptionToFindOutAboutService = (supportType) => {
        if (supportType === "Email") {
            return PageElement
                .located(By.id("contact-email"))
                .describedAs('Email option');
        } else if (supportType === "Telephone") {
            return PageElement
                .located(By.id("#contact-telephone"))
                .describedAs('Telephone option');
        } else if (supportType === "Website") {
            return PageElement
                .located(By.id("#contact-website"))
                .describedAs('website option');
        } else if (supportType === "Text message") {
            return PageElement
                .located(By.id("#contact-text-message"))
                .describedAs('Text option');
        } else {
            throw new Error(`The primary category "Activities" could not be found.`);
        }
    };

    static enterTextCorrespondenceToFindOutAboutService = (contactType) => {
        if (contactType === "Email") {
            return PageElement
                .located(By.id("email-text-box"))
                .describedAs('Email correspondence option');
        } else if (contactType === "Telephone") {
            return PageElement
                .located(By.id("telephone-text-box"))
                .describedAs('Telephone Correspondence option');
        } else if (contactType === "Website") {
            return PageElement
                .located(By.id("website-text-box"))
                .describedAs('Website corresspondence option');
        } else if (contactType === "Text message") {
            return PageElement
                .located(By.id("text-message-text-box"))
                .describedAs('Text Corresspondence option');
        } else {
            throw new Error(`Option does not exist`);
        }
    };


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
            .located(By.css("button[type='Submit']"))
            .describedAs('Continue Button');


    static continueButtonForCreateServiceupdate = () =>
        PageElement
            .located(By.css("button[type='submit']"))
            .describedAs('Continue Button');


    static continueButtonForAddALocationButton = () =>
        PageElement
            .located(By.css("button[value='continue']"))
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
