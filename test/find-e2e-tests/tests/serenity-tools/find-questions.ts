import {Page, Text} from '@serenity-js/web';
import {Ensure, equals, includes} from '@serenity-js/assertions';
import {laServiceInformation, serviceDetailsPage, vcfsServiceInformation} from './find-page-objects';

//This is for illustrative purposes and will be removed when appropriate questions can be created. 

export const isTheFindPageDisplayed = () =>
    Ensure.that(
        Page.current().title().describedAs('Find Page Title'),
        equals('Find support for your family - Find support for your family - GOV.UK'),
    )

export const verifyLAServiceInformationInTheListOfServicesPage = () =>
    Ensure.that(
        Text.of(laServiceInformation()),
        includes('Category') &&
        includes('Test         Organisation') &&
        includes('Age Range') &&
        includes('Where') &&
        includes('Free')
    );
export const verifyVCFSServiceInformationInTheListOfServicesPage = () =>
    Ensure.that(
        Text.of(vcfsServiceInformation()),
        includes('Category') &&
        includes('Test      LA') &&
        includes('Age range 16 to 18 years old') &&
        includes('Where') &&
        includes('Free')
    );

export const verifyTheServiceDetailsPageContent = () =>
    Ensure.that(
        Text.of(serviceDetailsPage()),
        includes('Service details') &&
        includes('Location') &&
        includes('More details') &&
        includes('Contact details')
    );     