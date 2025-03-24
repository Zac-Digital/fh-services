# GitHub Secrets & Variables

The purpose of this document is to list out the names of our GitHub Secrets & Variables with a short explanation of what they are used for and what workflows use them.

# Organisation Level

These are set by the DfE and as such are out of our control.

## Secrets

| Name | Use Case | Workflow(s) |
| --- | --- | --- |
| `CODEQL_APP_ID`  | Organisation-level CodeQL analysis | N/A |
| `CODEQL_AUTHENTICATION_PRIVATE_KEY`| Organisation-level CodeQL analysis | N/A |

# Repository Level

These are set by us and apply globally across workflows and environments.

## Secrets

| Name | Use Case | Workflow(s) |
| --- | --- | --- |
| `PLAYWRIGHT_GOVLOGIN_DFE_ADMIN_USER`  | The email address of a test user who has the DfE Admin role, used by the Playwright end-to-end test suite. | `run-e2e-tests.yml` |
| `PLAYWRIGHT_GOVLOGIN_PASSWORD` | The password of a test user who has the DfE Admin role, used by the Playwright end-to-end test suite. | `run-e2e-tests.yml` |
| `PLAYWRIGHT_USER_NAME` | The HTTP Basic Auth username for the test website(s). Used by the Playwright end-to-end test suite. | `run-e2e-tests.yml` |
| `PLAYWRIGHT_PASSWORD` | The HTTP Basic Auth password for the test website(s). Used by the Playwright end-to-end test suite. | `run-e2e-tests.yml` |
| `SONAR_TOKEN` | The token used to upload results to our [SonarCloud instance](https://sonarcloud.io/project/overview?id=DFE-Digital_fh-services) | `sonarcloud.yml` |
| `WEB_APPS_BASIC_AUTH_USER` | The HTTP Basic Auth username for all non-Prod sites, used when configuring Basic Auth On or Off for the web apps during deployment. | `deploy-service.yml` |
| `WEB_APPS_BASIC_AUTH_PASSWORD` | The HTTP Basic Auth password for all non-Prod sites, used when configuring Basic Auth On or Off for the web apps during deployment. | `deploy-service.yml` |

## Variables

| Name | Use Case | Workflow(s) |
| --- | --- | --- |
| `DOTNET_VERSION` | **Legacy** - Used during transition from .NET 7 to .NET 8. Workflows that use this actually run with .NET 8. | `deploy.yml`, `run-acceptance-tests.yml` |
| `DOTNET_VERSION_V8` | .NET 8 as a reusable variable, used across a variety of workflows that need to run dotnet commands. | `build-and-test.yml`, `build-upload-artifact.yml`, `deploy-service.yml`, `migrate-database.yml`, `sonarcloud.yml`, `stryker-full-report.yml` |
| `JAVA_VERSION` | The SonarCloud scanner is a Java application, so this variable just controls the version of Java used for it. | `sonarcloud.yml` |
| `PLAYWRIGHT_FIND_BASE_URL` | The URL of the Find website, used in conjunction with an environment variable to determine the URL to run the Playwright Find test suite against. | `run-e2e-tests.yml` |
| `PLAYWRIGHT_MANAGE_BASE_URL` | The URL of the Find website, used in conjunction with an environment variable to determine the URL to run the Playwright Manage test suite against. | `run-e2e-tests.yml` |

# Environment Level

These are set by us and the values are unique to each environment.

## Secrets

| Name | Use Case | Workflow(s) | Environment(s) |
| --- | --- | --- | --- |
| `AZURE_CLIENT_ID` | The ID of the service principal used to interface GitHub Actions with Azure. | `deploy-function.yml`, `deploy-service.yml`, `e2e-seed-database.yml`, `migrate-database.yml`, `provisioning.yml`, `run-acceptance-tests.yml`, `run-sql-script.yml` | `Development`, `Test`, `Test2`, `Pre-production`, `Production` |
| `AZURE_CLIENT_SECRET` | The secret of the service principal, this in particular is only used by the Terraform as other workflows use federated credentials. | `provisioning.yml` | `Development`, `Test`, `Test2`, `Pre-production`, `Production` |
| `AZURE_SUBSCRIPTION_ID` | The subscription ID of the service principal used to interface GitHub Actions with Azure. | `deploy-function.yml`, `deploy-service.yml`, `e2e-seed-database.yml`, `migrate-database.yml`, `provisioning.yml`, `run-acceptance-tests.yml`, `run-sql-script.yml` | `Development`, `Test`, `Test2`, `Pre-production`, `Production` |
| `AZURE_TENANT_ID` | The tenant ID of the service principal used to interface GitHub Actions with Azure. | `deploy-function.yml`, `deploy-service.yml`, `e2e-seed-database.yml`, `migrate-database.yml`, `provisioning.yml`, `run-acceptance-tests.yml`, `run-sql-script.yml` | `Development`, `Test`, `Test2`, `Pre-production`, `Production` |
| `PLAYWRIGHT_CONNECTION_STRING_REFERRAL_DATABASE` | Used by the E2E seeding scripts to connect to the Referral database when creating E2E data, as part of the Playwright tests. | `e2e-seed-database.yml` | `Development`, `Test`, `Test2`, `Pre-production` |
| `PLAYWRIGHT_CONNECTION_STRING_REPORT_DATABASE` | Used by the E2E seeding scripts to connect to the Report database when creating E2E data, as part of the Playwright tests. | `e2e-seed-database.yml` | `Development`, `Test`, `Test2`, `Pre-production` |
| `PLAYWRIGHT_CONNECTION_STRING_SERVICE_DIRECTORY_DATABASE` | Used by the E2E seeding scripts to connect to the Service Directory database when creating E2E data, as part of the Playwright tests. | `e2e-seed-database.yml` | `Development`, `Test`, `Test2`, `Pre-production` |
| `PLAYWRIGHT_REFERRAL_COLUMN_ENCRYPTION_KEY` | Used by the E2E seeding scripts to encrypt certain column data, as part of the Playwright tests. | `e2e-seed-database.yml` | `Development`, `Test`, `Test2`, `Pre-production` |
| `PLAYWRIGHT_REFERRAL_COLUMN_INITIALISATION_VECTOR` | Used by the E2E seeding scripts to encrypt certain column data, as part of the Playwright tests. | `e2e-seed-database.yml` | `Development`, `Test`, `Test2`, `Pre-production` |
| `TERRAFORM_APP_SQL_SERVER_USER` | Used as part of the configuration of the SQL server in the Terraform IaC | `provisioning.yml` | `Development`, `Test`, `Test2`, `Pre-production`, `Production` |
| `TERRAFORM_APP_SQL_SERVER_PWD` | Used as part of the configuration of the SQL server in the Terraform IaC | `provisioning.yml` | `Development`, `Test`, `Test2`, `Pre-production`, `Production` |
| `TERRAFORM_REPORT_SQL_SERVER_USER` | Used as part of the configuration of the Azure Data Factory SQL server in the Terraform IaC | `provisioning.yml` | `Development`, `Test`, `Test2`, `Pre-production`, `Production` |
| `TERRAFORM_REPORT_SQL_SERVER_PWD` | Used as part of the configuration of the Azure Data Factory SQL server in the Terraform IaC | `provisioning.yml` | `Development`, `Test`, `Test2`, `Pre-production`, `Production` |
| `TERRAFORM_SSL_CERTIFICATE_PWD` | Used as part of the configuration of the Web Apps' SSL certificates in the Terraform IaC | `provisioning.yml` | `Development`, `Test`, `Test2`, `Pre-production`, `Production` |
| `TERRAFORM_SSL_CONNECT_PFX` | Used as part of the configuration of the Web Apps' SSL certificates in the Terraform IaC | `provisioning.yml` | `Development`, `Test`, `Test2`, `Pre-production`, `Production` |
| `TERRAFORM_SSL_FIND_PFX` | Used as part of the configuration of the Web Apps' SSL certificates in the Terraform IaC | `provisioning.yml` | `Development`, `Test`, `Test2`, `Pre-production`, `Production` |
| `TERRAFORM_SSL_MANAGE_PFX` | Used as part of the configuration of the Web Apps' SSL certificates in the Terraform IaC | `provisioning.yml` | `Development`, `Test`, `Test2`, `Pre-production`, `Production` |

## Variables

| Name | Use Case | Workflow(s) | Environment(s) |
| --- | --- | --- | --- |
| `AZURE_RESOURCE_PREFIX` | The prefix pattern of all resources in an environment, used to calculate the names of various Azure resources | `deploy-function.yml`, `deploy-service.yml`, `e2e-seed-database.yml`, `migrate-database.yml`, `provisioning.yml`, `run-acceptance-tests.yml`, `run-sql-script.yml` | `Development`, `Test`, `Test2`, `Pre-production`, `Production` |
| `PLAYWRIGHT_ENVIRONMENT_PREFIX` | The URL prefix of a test website, used in conjunction with the remainder of the URL to tell the Playwright tests where to point at | `run-e2e-tests.yml` | `Development`, `Test`, `Test2`, `Pre-production` |
| `SLACK_SUPPORT_CHANNEL_EMAIL` | Used as part of the monitoring and alerting setup in the Terraform IaC | `provisioning.yml` | `Development`, `Test`, `Test2`, `Pre-production`, `Production` |
| `WEB_APPS_REQUIRE_BASIC_AUTH` | Whether the Web App should be gated behind HTTP Basic Auth | `deploy-service.yml` | `Development`, `Test`, `Test2`, `Pre-production`, `Production` |