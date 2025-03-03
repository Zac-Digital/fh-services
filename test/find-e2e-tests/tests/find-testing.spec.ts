import {describe, it} from '@serenity-js/playwright-test';
import {
    navigateToFind,
    clickOnTheStartButton,
    enterPostcode,
    clickOnPostcodeSearchButton,
    doesTheLAServiceInformationInTheListOfServicesPageContain,
    doesTheVCFSServiceInformationInTheListOfServicesPageContains,
    clickOnTheLaService,
    doesTheServiceDetailsPageContentContain, clickOnTheVcfsService
} from './serenity-tools/find-index';

describe('Find Tests', () => {

    it('should check a DfE_Find_User is able to search for services ', async ({actorCalled}) => {
        await actorCalled('DfE_Find_User)').attemptsTo(
            navigateToFind(),
            clickOnTheStartButton(),
            enterPostcode('W1D 2JT'),
            clickOnPostcodeSearchButton(),
        );
    });


    it('should check a DfE_Find_User is able verify LA Service information', async ({actorCalled}) => {
        await actorCalled('DfE_Find_User').attemptsTo(
            navigateToFind(),
            clickOnTheStartButton(),
            enterPostcode('W1D 2JT'),
            clickOnPostcodeSearchButton(),
            doesTheLAServiceInformationInTheListOfServicesPageContain('Test LA'),
            doesTheLAServiceInformationInTheListOfServicesPageContain('Category'),
            doesTheLAServiceInformationInTheListOfServicesPageContain('Age range'),
            doesTheLAServiceInformationInTheListOfServicesPageContain('Where'),
            doesTheLAServiceInformationInTheListOfServicesPageContain('Cost'),
            clickOnTheLaService(),
            doesTheServiceDetailsPageContentContain('Service details'),
            doesTheServiceDetailsPageContentContain('Location'),
            doesTheServiceDetailsPageContentContain('More details'),
            doesTheServiceDetailsPageContentContain('Contact details')
        );
    });

    it('should check a DfE_Find_User is able verify VCFS Service information', async ({actorCalled}) => {
        await actorCalled('DfE_Find_User').attemptsTo(
            navigateToFind(),
            clickOnTheStartButton(),
            enterPostcode('W1D 2JT'),
            clickOnPostcodeSearchButton(),
            doesTheVCFSServiceInformationInTheListOfServicesPageContains('Test Organisation'),
            doesTheVCFSServiceInformationInTheListOfServicesPageContains('Category'),
            doesTheVCFSServiceInformationInTheListOfServicesPageContains('Age range'),
            doesTheVCFSServiceInformationInTheListOfServicesPageContains('Where'),
            doesTheVCFSServiceInformationInTheListOfServicesPageContains('Cost'),
            clickOnTheVcfsService(),
            doesTheServiceDetailsPageContentContain('Service details'),
            doesTheServiceDetailsPageContentContain('Location'),
            doesTheServiceDetailsPageContentContain('More details'),
            doesTheServiceDetailsPageContentContain('Contact details')
        );
    });
});