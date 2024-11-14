# Family Hubs E2E Seed Data Framework

The Family Hubs E2E Seed Data Framework is a collection of scripts that testers can alter and run in order to set up and tear down test data used by end-to-end tests. The scripts are designed to be run locally (pointing at local databases) and as part of a post-release CI/CD pipeline to run tests against a test environment.

## Prerequisites

1. Read the `Environment Variables` section to set up your local database connections.
2. Run `$ npm i` to install the packages for the first time.

## Scripts

`$ npm run setup:dev`

(For local/development use) Creates test data necessary for E2E tests to run.

`$ npm run teardown:dev`

(For local/development use) Removes test data necessary for E2E tests to run.

## Models

Models are in-code definitions of what a database table looks like. Each table we want to insert test data into/read from must have a model. Models are defined in JS files under the `models/` dir.

More information on what models are and how they work can be found here: https://sequelize.org/docs/v6/core-concepts/model-basics/

## Environment Variables

Environment variables are used to configure the setup and teardown scripts.

To run the tests, make a copy of `.env.local` and call it `.env`, and then edit the connection strings to match your local database setup. Example connection strings are included but your mileage may vary!

The CI/CD pipeline will create its own `.env` file using the connection strings for the environment it's running against.

## Packages

The following packages are used:

- @dotenvx/dotenvx: to support `.env` files for local development and testing
- sequelize: an ORM for interacting with databases through a JS API
- tedious: a SQL Server DB driver used by sequelize