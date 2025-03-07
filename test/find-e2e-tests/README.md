# E2E Testing

Here are written End to End tests for the three services in Family Hubs: Find, Connect, Manage.
This is a Serenity/JS with Playwright Framework using a screenplay pattern.

# Prerequisites

- Ensure Node.js and npm are installed on your machine.
- Run data seed scripts
- Navigate to folder of service under test (i.e. find-e2e-tests, connect-e2e-tests or manage-e2e-tests)
- Install all required dependencies: npm install
- Ensure to create a .env file containing the baseURL, web authentication username and web authentication password for
  the environment you plan to test.

# How to run the tests locally (against the test environment)

To run tests: `npm test`
To view the test report: `npm run test:report`

# Helpful Links

Screenplay Pattern Overview - https://serenity-js.org/handbook/design/screenplay-pattern/

Serenity JS Playwright Test

- https://serenity-js.org/handbook/test-runners/playwright-test/
- https://serenity-js.org/handbook/getting-started/serenity-js-with-playwright-test/

