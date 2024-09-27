# Family Hubs High Level Design Document

## Table of Contents

TODO

## Glossary

- FH - Family hubs
- FX - Family experience
- IS - Information Sharing
- MVS - Minimal Viable Service
- VCS – Voluntary Community Services
- LA – Local Authority

## Problem Statement

This document provides details of the high-level design for the Vulnerable Children and Families portfolio Family Hubs service directory and request for support products. It consists of three main components:

1. **Find:** Allows public users to browse a national service directory
2. **Connect:** Allows professional and voluntary users to refer families to services
3. **Manage:** Allows administration of service data and user accounts

## Architecture

The following diagram provides a high-level overview of the Family Hubs architecture.

**Note:** as this is only a high-level overview, it is not intended to be a 1:1 visualisation of the entire Family Hubs architecture.

### High-level Architecture

```mermaid

graph LR
    %%{init:{'flowchart':{'nodeSpacing': 50, 'rankSpacing': 100 }}}%%
    %% User Styles
    style PublicUsers fill:#ffcccb,stroke:#333,stroke-width:2px
    PublicUsers[Public Users] -->|Use| Find[Find]

    style ProfessionalUsers fill:#ffcccb,stroke:#333,stroke-width:2px
    ProfessionalUsers[Professional Users] -->|Authenticate using| GovOneLogin[Gov One Login] -->|Use| Connect[Connect]
    ProfessionalUsers[Professional Users] -->|Authenticate using| GovOneLogin[Gov One Login] -->|Use| ConnectDashboard[Connect Dashboard]

    style AdministrativeUsers fill:#ffcccb,stroke:#333,stroke-width:2px
    AdministrativeUsers[Administrative Users] -->|Authenticate using| GovOneLogin -->|Use| Manage[Manage]

    style FamilyHubsDev fill:#ffcccb,stroke:#333,stroke-width:2px
    FamilyHubsDev[Family Hubs Developers] -->|Use| IdAMMaintenanceUI[IdAM Maintenance UI]

    %% Service Styles
    style Find fill:#90ee90,stroke:#333,stroke-width:2px
    style Connect fill:#90ee90,stroke:#333,stroke-width:2px
    style ConnectDashboard fill:#90ee90,stroke:#333,stroke-width:2px
    style Manage fill:#90ee90,stroke:#333,stroke-width:2px
    style IdAMMaintenanceUI fill:#90ee90,stroke:#333,stroke-width:2px
    style NotificationAPI fill:#90ee90,stroke:#333,stroke-width:2px

    subgraph Azure
        style Azure fill:#e0f7fa,stroke:#333,stroke-width:2px,stroke-dasharray: 5, 5,margin:102px
        
        subgraph App_Services[App Services]
            style ServiceDirectoryAPI fill:#add8e6,stroke:#333,stroke-width:2px
            style IdAMAPI fill:#add8e6,stroke:#333,stroke-width:2px
            style ReferralAPI fill:#add8e6,stroke:#333,stroke-width:2px
            style ReportAPI fill:#add8e6,stroke:#333,stroke-width:2px
            style NotificationAPI fill:#add8e6,stroke:#333,stroke-width:2px
            
            Find -->|Connects to| ServiceDirectoryAPI[Service Directory API]

            Connect -->|Connects to| ServiceDirectoryAPI
            Connect -->|Connects to| IdAMAPI[IdAM API]
            Connect -->|Connects to| ReferralAPI[Referral API]
            Connect -->|Connects to| NotificationAPI[Notification API]

            ConnectDashboard -->|Connects to| ServiceDirectoryAPI
            ConnectDashboard -->|Connects to| IdAMAPI[IdAM API]
            ConnectDashboard -->|Connects to| ReferralAPI[Referral API]
            ConnectDashboard -->|Connects to| NotificationAPI[Notification API]

            Manage -->|Connects to| ServiceDirectoryAPI
            Manage -->|Connects to| IdAMAPI
            Manage -->|Connects to| ReportAPI[Report API]
            Manage -->|Connects to| NotificationAPI

            IdAMMaintenanceUI -->|Connects to| ServiceDirectoryAPI
            IdAMMaintenanceUI -->|Connects to| IdAMAPI
        end

        subgraph Databases
            style ServiceDirectoryDB fill:#d3d3d3,stroke:#333,stroke-width:2px
            style IdAMDB fill:#d3d3d3,stroke:#333,stroke-width:2px
            style ReferralDB fill:#d3d3d3,stroke:#333,stroke-width:2px
            style ReportDB fill:#d3d3d3,stroke:#333,stroke-width:2px
            style NotificationDB fill:#d3d3d3,stroke:#333,stroke-width:2px
            
            ServiceDirectoryAPI -->|Stores data in| ServiceDirectoryDB[s181p01-service-directory-db]
            IdAMAPI -->|Stores data in| IdAMDB[s181p01-idam-db]
            ReferralAPI -->|Stores data in| ReferralDB[s181p01-referral-db]
            ReportAPI -->|Stores data in| ReportDB[s181p01-report-db]
            NotificationAPI -->|Stores data in| NotificationDB[s181p01-notification-db]
        end
    end

    %% External Service
    style GovOneLogin fill:#ffeb3b,stroke:#333,stroke-width:2px,stroke-dasharray: 5, 5
    GovOneLogin[Gov One Login] -->|Authenticates| Connect
    GovOneLogin -->|Authenticates| Manage
```
## Service Information

### Third Parties

We use the following third party services:

- **Postcodes.io:** Free service for postcode lookups
- **GOV.UK One Login:** Authentication service
- **GOV.UK Notify:** Email notification service

### Services

Family Hubs consists of the following services:

#### UIs

- **Find:** lets users find services in their area through a postcode search
- **Connect:** allows LAs to connect families with services through connection requests
- **Connect Dashboard:** allows LA and VS users to send/receive connection requests. Is built to be visually indistinguishable from the main Connect side, appearing as just another section of Connect.
- **Manage:** allows LA and VCS users to manage their data, view metrics, and other administrative tasks
- **IdAM Maintenance UI:** TODO

#### APIs

- Service Directory API
- Referral API
- IdAM Service
- Notification API
- Report API


#### Functions
- TODO

## Technical Details

### Hosting and Environments

We have the following environments deployed within the CIP infrastructure on Azure:
- **Development**: deployed in the Development subscription
- **Test 1**: deployed in the Test subscription
- **Test 2**: deployed in the Test subscription
- **Production**: deployed in the Production subscription

### Networking

- Virtual network with private endpoints
- Network security groups for traffic control

### Compute

Services run as app services within an app service plan on Azure.

### Storage

- Storage accounts for logging data and Terraform plans
- Additional storage for static custom error pages

### Security

- Authentication via GOV.UK One Login
- Authorization via IdAM service
- TLS termination  and WAF rules via Application Gateway
- Secrets management using Azure Key Vault
- Cyber attack protection (WAF, OWASP, Bot protection)
- NCSC WebCheck for security profiling
- Access to SQL Servers secured by firewall IP whitelist

### Databases

- SQL Server managed instances hosted on Azure
- Easy DTU scaling provided to easily scale to accommodate load as necessary

## Support and Maintenance

- Azure resources deployed in Common Infrastructure Platform (CIP)
- Monitoring and alerting provided by the team and Azure platform
- Access control to Azure with user/password + 2FA, and PIM for Test and Production subscriptions access

## Logging, Monitoring, and Alerting

- Google Analytics for client-side events
- Azure App Insights for service analytics
- Alerting via Log Analytics in Azure Monitor
- Microsoft Clarify for screen recordings, use behaviour tracking, and life performance metrics

## Data Storage & Retention

- PII data retained for 7 years, then anonymized
- Service directory data retained indefinitely
- Other data (e.g., IdAM) retained for 1 year
- Data encrypted in flight and at rest
- Additional encryption for PII data

## DNS

Public DNS records for each site (Find, Connect, Manage) as subdomains of education.gov.uk.

## Patch & Update Management

PaaS services in Azure handle patch and update management. Renovate enabled for the GitHub mono repo to alert us to/automatically raise PRs for dependency updates

## Resilience/Fault Tolerance

Provided by application plan and container orchestration in PaaS services. Application plan auto scales to n number of containers to support load.

## Disaster Recovery

- Daily and weekly backups to storage account
- Recovery Point Objective: 24 hours
- Recovery Time Objective: 24 hours (maximum)

## DevOps

- Single GitHub repository for all Family Hubs services
- Terraform repository for IAC source
- Managed pipeline in GitHub Actions for both code and IAC deployments
- Open-source code management with feature branching and Pull Requests

## Assurances

- Various testing types (unit, regression, code security, accessibility, end-to-end, performance)
- IT Health Checks and penetration tests performed every 2 years, or when moving to new live statuses as per GDS and DfE guidelines
