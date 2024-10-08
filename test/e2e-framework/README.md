# Family Hubs E2E Test Framework

The Family Hubs E2E Test Framework is a collection of scripts that testers can alter and run in order to set up and tear down test data used by end-to-end tests. The scripts are designed to be run locally (pointing at local databases) and as part of a post-release CI/CD pipeline to run tests against a test environment.

## Scripts

`$ npm run setup`

Creates test data necessary for E2E tests to run.

`$ npm run teardown`

Removes test data necessary for E2E tests to run.

## Packages

The following packages are used:

- dotenv: to support `.env` files for local development and testing
- sequelize: an ORM for interacting with databases through a JS API
- tedious: a SQL Server DB driver used by sequelize