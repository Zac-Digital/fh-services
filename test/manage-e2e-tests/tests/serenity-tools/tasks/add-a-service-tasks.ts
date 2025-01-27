import {Answerable, Task, Wait, Check} from '@serenity-js/core';
import {Click, Enter, Press, Key, isVisible, isClickable, Scroll} from '@serenity-js/web';
import {AddAServicePageObjects} from "../page-objects/add-a-service-page-objects";
import {homeButton} from "../page-objects/manage-page-objects";



export const addAnLAService = (serviceName: Answerable<string>, emailAddress: Answerable<string>,): Task =>
    Task.where(
        `#actor creates a LA service named ${serviceName}`,
        Click.on(AddAServicePageObjects.addAServiceLink()),
        Wait.until(AddAServicePageObjects.searchAndSelectLaField(), isVisible()),
        Scroll.to(AddAServicePageObjects.searchAndSelectLaField()),
        Enter.theValue('Test LA').into(AddAServicePageObjects.searchAndSelectLaField()),
        Press.the(Key.Tab).in(AddAServicePageObjects.searchAndSelectLaField()),
        Click.on(AddAServicePageObjects.continueButtonForCreateService()),
        Enter.theValue(serviceName).into(AddAServicePageObjects.whatIsTheServiceNameField()),
        Click.on(AddAServicePageObjects.continueButtonForCreateService()),
        Click.on(AddAServicePageObjects.whatSupportDoesTheServiceOfferHealthCategory()),
        Click.on(AddAServicePageObjects.whatSupportDoesTheServiceOfferSecondaryCategory()),
        Click.on(AddAServicePageObjects.continueButtonForCreateService()),
        Enter.theValue('This is an automated Test').into(AddAServicePageObjects.giveADescriptionOfTheServiceField()),
        Click.on(AddAServicePageObjects.continueButtonForCreateService()),
        Click.on(AddAServicePageObjects.SupportRelatedToChildrenOrYoungPeopleFieldNo()),
        Click.on(AddAServicePageObjects.continueButtonForCreateService()),
        Enter.theValue('Danish').into(AddAServicePageObjects.getServiceOfferedLanguagesField()),
        Press.the(Key.Tab).in(AddAServicePageObjects.getServiceOfferedLanguagesField()),
        Click.on(AddAServicePageObjects.confirmAndAddAServiceButton()),
        Click.on(AddAServicePageObjects.ServicePaidForOptionNo()),
        Click.on(AddAServicePageObjects.continueButtonForCreateService()),
        Click.on(AddAServicePageObjects.inPersonServiceAccessDetails()),
        Click.on(AddAServicePageObjects.continueButtonForCreateService()),
        Click.on(AddAServicePageObjects.addServiceLocation()),
        Click.on(AddAServicePageObjects.continueButtonForCreateService()),
        Enter.theValue('Test Location').into(AddAServicePageObjects.searchAndSelectLocation()),
        Press.the(Key.Tab).in(AddAServicePageObjects.searchAndSelectLocation()),
        Click.on(AddAServicePageObjects.continueButtonForCreateService()),
        Click.on(AddAServicePageObjects.selectServiceAvailableMonday()),
        Click.on(AddAServicePageObjects.selectServiceAvailableTuesday()),
        Click.on(AddAServicePageObjects.continueButtonForCreateService()),
        Click.on(AddAServicePageObjects.getServiceUsageDetailsYes()),
        Enter.theValue('This is a Test Location').into(AddAServicePageObjects.enterDetailAboutLocation()),
        Click.on(AddAServicePageObjects.continueButtonForCreateService()),
        Wait.until((AddAServicePageObjects.addAnotherLocationButton()), isClickable()),
        Click.on(AddAServicePageObjects.continueButtonForAddALocationButton()),
        Click.on(AddAServicePageObjects.emailOptionToFindOutAboutService()),
        Enter.theValue(emailAddress).into(AddAServicePageObjects.emailCorrespondenceToFindOutAboutService()),
        Click.on(AddAServicePageObjects.continueButtonForCreateService()),
        Enter.theValue('This is a Test Description').into(AddAServicePageObjects.provideMoreServiceDetailsTextField()),
        Click.on(AddAServicePageObjects.continueButtonForCreateService()),
        Wait.until((AddAServicePageObjects.confirmAndAddAServiceButton()), isClickable()),
        Click.on(AddAServicePageObjects.confirmAndAddAServiceButton()),
    );


export const searchForService = (serviceName: Answerable<string>): Task =>
    Task.where(
        `#actor navigates to the find services page from the home page and seacrhes for the service`,
        Click.on(homeButton()),
        Click.on(AddAServicePageObjects.manageServicesLink()),
        Check.whether(AddAServicePageObjects.showFiltersButtonAddAService(), isVisible())
            .andIfSo(
                Click.on(AddAServicePageObjects.showFiltersButtonAddAService())
            ),
        Enter.theValue(serviceName).into(AddAServicePageObjects.enterServiceNameTextAreaInManageService()),
        Click.on(AddAServicePageObjects.ApplyfilterInManageService()),
    );


export const addAnVCSService = (serviceName: Answerable<string>, emailAddress: Answerable<string>,): Task =>
    Task.where(
        `#actor creates a VCFS service named ${serviceName}`,
        Click.on(AddAServicePageObjects.addAVCSServiceLink()),
        Wait.until(AddAServicePageObjects.searchAndSelectLaField(), isVisible()),
        Enter.theValue('Test LA').into(AddAServicePageObjects.searchAndSelectLaField()),
        Press.the(Key.Tab).in(AddAServicePageObjects.searchAndSelectLaField()),
        Click.on(AddAServicePageObjects.continueButtonForCreateService()),
        Enter.theValue(' Test Organisation').into(AddAServicePageObjects.searchAndSelectVCSOrgField()),
        Press.the(Key.Tab).in(AddAServicePageObjects.searchAndSelectLaField()),
        Click.on(AddAServicePageObjects.continueButtonForCreateService()),
        Enter.theValue(serviceName).into(AddAServicePageObjects.whatIsTheServiceNameField()),
        Click.on(AddAServicePageObjects.continueButtonForCreateService()),
        Click.on(AddAServicePageObjects.whatSupportDoesTheServiceOfferHealthCategory()),
        Click.on(AddAServicePageObjects.whatSupportDoesTheServiceOfferSecondaryCategory()),
        Click.on(AddAServicePageObjects.continueButtonForCreateService()),
        Enter.theValue('This is an automated Test').into(AddAServicePageObjects.giveADescriptionOfTheServiceField()),
        Click.on(AddAServicePageObjects.continueButtonForCreateService()),
        Click.on(AddAServicePageObjects.SupportRelatedToChildrenOrYoungPeopleFieldNo()),
        Click.on(AddAServicePageObjects.continueButtonForCreateService()),
        Enter.theValue('Danish').into(AddAServicePageObjects.getServiceOfferedLanguagesField()),
        Press.the(Key.Tab).in(AddAServicePageObjects.getServiceOfferedLanguagesField()),
        Click.on(AddAServicePageObjects.confirmAndAddAServiceButton()),
        Click.on(AddAServicePageObjects.ServicePaidForOptionNo()),
        Click.on(AddAServicePageObjects.continueButtonForCreateService()),
        Click.on(AddAServicePageObjects.inPersonServiceAccessDetails()),
        Click.on(AddAServicePageObjects.continueButtonForCreateService()),
        Click.on(AddAServicePageObjects.addServiceLocation()),
        Click.on(AddAServicePageObjects.continueButtonForCreateService()),
        Enter.theValue('Test Location').into(AddAServicePageObjects.searchAndSelectLocation()),
        Press.the(Key.Tab).in(AddAServicePageObjects.searchAndSelectLocation()),
        Click.on(AddAServicePageObjects.continueButtonForCreateService()),
        Click.on(AddAServicePageObjects.selectServiceAvailableMonday()),
        Click.on(AddAServicePageObjects.selectServiceAvailableTuesday()),
        Click.on(AddAServicePageObjects.continueButtonForCreateService()),
        Click.on(AddAServicePageObjects.getServiceUsageDetailsYes()),
        Enter.theValue('Elop Community Center').into(AddAServicePageObjects.enterDetailAboutLocation()),
        Click.on(AddAServicePageObjects.continueButtonForCreateService()),
        Wait.until((AddAServicePageObjects.addAnotherLocationButton()), isClickable()),
        Click.on(AddAServicePageObjects.continueButtonForAddALocationButton()),
        Click.on(AddAServicePageObjects.emailOptionToFindOutAboutService()),
        Enter.theValue(emailAddress).into(AddAServicePageObjects.emailCorrespondenceToFindOutAboutService()),
        Click.on(AddAServicePageObjects.continueButtonForCreateService()),
        Enter.theValue('This is a Test Description').into(AddAServicePageObjects.provideMoreServiceDetailsTextField()),
        Click.on(AddAServicePageObjects.continueButtonForCreateService()),
        Wait.until((AddAServicePageObjects.confirmAndAddAServiceButton()), isClickable()),
        Click.on(AddAServicePageObjects.confirmAndAddAServiceButton()),
    );


export const searchForVCFSService = (serviceName: Answerable<string>): Task =>
    Task.where(
        `#actor navigates to the find services page from the home page and seacrhes for the service`,
        Click.on(homeButton()),
        Scroll.to(AddAServicePageObjects.manageVcfsServicesLink()),
        Click.on(AddAServicePageObjects.manageVcfsServicesLink()),
        Check.whether(AddAServicePageObjects.showFiltersButtonAddAService(), isVisible())
            .andIfSo(
                Click.on(AddAServicePageObjects.showFiltersButtonAddAService())
            ),
        Enter.theValue(serviceName).into(AddAServicePageObjects.enterServiceNameTextAreaInManageService()),
        Click.on(AddAServicePageObjects.ApplyfilterInManageService()),
    );
