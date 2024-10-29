import { describe, it } from '@serenity-js/playwright-test';

import { navigateToFind,clickOnTheStartButton,enterPostcodeAndSearch} from './serenity-tools';

describe('Find Tests', () => {

    it('should check a Dfe Find User is able to search for services ', async ({ actorCalled }) => {
            await actorCalled('Dfe_Find_User').attemptsTo(
                navigateToFind(),
                clickOnTheStartButton(),
                enterPostcodeAndSearch('E1 2EN'), 
            );
        });
    });