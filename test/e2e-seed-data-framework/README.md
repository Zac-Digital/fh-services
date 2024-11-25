# Family Hubs E2E Seed Data Framework

The Family Hubs E2E Seed Data Framework is a collection of scripts that testers can alter and run in order to set up and tear down test data used by end-to-end tests. The scripts are designed to be run locally (pointing at local databases) and as part of a post-release CI/CD pipeline to run tests against a test environment.

## Pre-Requisites

1. Read the `Environment Variables` section to set up your local database connections.
2. Run `$ npm i` to install the packages for the first time.

## Scripts

`$ npm run setup:dev`

(For local/development use) Creates test data necessary for E2E tests to run.

`$ npm run teardown:dev`

(For local/development use) Removes test data necessary for E2E tests to run.

Whenever we want to seed a set of tables, we need to create the seed scripts in the `scripts/` folder and make sure it follows the convention defined in the examples - that is it needs an exported function `seed()`. It then needs to be imported and ran alongside the others in the `setup` script and its models added to the `teardown` script.

## Models

Models are in-code definitions of what a database table looks like. Each table we want to insert test data into/read from must have a model. Models are defined in JS files under the `models/` dir.

More information on what models are and how they work can be found here: https://sequelize.org/docs/v6/core-concepts/model-basics/

## Core

The `core/` folder contains the "Db Contexts", this is where CRUD operations are performed on the models residing in the Db.

In our case, we only create new entries of each model into the database. The database contains tables and columns that are unused by the web-apps (that is, unpopulated and/or not consumed) and so the wrapper methods for adding models don't include those to reduce cognitive load. It is easy to add them should they be needed.

In each wrapper also contains helper functions such as appending Test IDs to each ID field, encrypting strings before they are inserted into certain tables and prepending the test prefix to certain fields.

Whenever we create a new set of models for a new table, we need to create the appropriate db context in the `core/` folder.

## Examples

In the `scripts/example/` folder exists examples of how to seed each database, where each relevant table is seeded at least once. You can run these against your local database
by setting `EXAMPLE_SEED='True'` inside your `.env` file. Set it to `'False'` to run your own implementation outside of the `example/` folder.

## Environment Variables

Environment variables are used to configure the setup and teardown scripts.

To run the tests, make a copy of `.env.local` and call it `.env`, and then edit the connection strings to match your local database setup. Example connection strings are included but your mileage may vary!

You will also need to obtain Development encryption key and encryption IV key from KeyVault for the Referral Db.

Toggle `EXAMPLE_SEED` on or off (`'True'` or `'False'`) to choose whether to run the example scripts or the implementation scripts against the database.

The CI/CD pipeline will create its own `.env` file using the connection strings for the environment it's running against.

## Packages

The following packages are used:

- @dotenvx/dotenvx: to support `.env` files for local development and testing
- sequelize: an ORM for interacting with databases through a JS API
- tedious: a SQL Server DB driver used by sequelize
