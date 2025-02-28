import {describe, it} from '@serenity-js/playwright-test';
import {
    navigateToFind,
    clickOnTheStartButton,
    enterPostcode,
    clickOnPostcodeSearchButton,
    verifyLAServiceInformationInTheListOfServicesPage,
    verifyVCFSServiceInformationInTheListOfServicesPage,
    clickOnTheLaService,
    verifyTheServiceDetailsPageContent, clickOnTheVcfsService
} from './serenity-tools/find-index';

describe('Find Tests', () => {

    it('should check a DfE Find User is able to search for services ', async ({actorCalled}) => {
        await actorCalled('DfE_Find_User').attemptsTo(
            navigateToFind(),
            clickOnTheStartButton(),
            enterPostcode('E1 2EN'),
            clickOnPostcodeSearchButton(),
        );
    });


    it('Verify LA Service information', async ({actorCalled}) => {
        await actorCalled('DfE_Find_User').attemptsTo(
            navigateToFind(),
            clickOnTheStartButton(),
            enterPostcode(' W1D 2JT'),
            clickOnPostcodeSearchButton(),
            verifyLAServiceInformationInTheListOfServicesPage(),
            clickOnTheLaService(),
            verifyTheServiceDetailsPageContent(),
        );
    });

    it('Verify VCFS Service information', async ({actorCalled}) => {
        await actorCalled('DfE_Find_User').attemptsTo(
            navigateToFind(),
            clickOnTheStartButton(),
            enterPostcode(' W1D 2JT'),
            clickOnPostcodeSearchButton(),
            verifyVCFSServiceInformationInTheListOfServicesPage(),
            clickOnTheVcfsService(),
            verifyTheServiceDetailsPageContent(),
        );
    });
});