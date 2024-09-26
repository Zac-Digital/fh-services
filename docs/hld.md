# Family Hubs High Level Design Document

## Table of Contents

TODO

## 1. Glossary

FH - Family hubs
FX - Family experience
IS - Information Sharing
MVS - Minimal Viable Service
VCS – Voluntary Community Services
LA – Local Authority

## 2. Problem Statement

This document provides details of the high-level design for the Vulnerable Children and Families portfolio Family Hubs service directory and request for support products. It consists of three main components:

1. **Find:** Allows public users to browse a national service directory
2. **Connect:** Allows professional and voluntary users to refer families to services
3. **Manage:** Allows administration of service data and user accounts

## 3. Service Information

### Third Parties

We use the following third party services:

- **Postcodes.io:** Free service for postcode lookups
- **GOV.UK One Login:** Authentication service
- **GOV.UK Notify:** Email notification service

### Services

Family Hubs consists of the following services:

#### UIs

- **Find Service:** lets users find services in their area through a postcode search
- **Connect Service:** allows LAs to connect families with services through connection requests
- **Referral Management Service:** allows LA and VS users to send/receive connection requests. Is built to be visually indistinguishable from the main Connect side, appearing as just another section of Connect.
- **Manage Service:** allows LA and VCS users to manage their data, view metrics, and other administrative tasks

#### APIs

- Service Directory API
- Referral API
- IdAM Service
- Notification API
- Report API

## 4. Technical Details

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

## 5. Support and Maintenance

- Azure resources deployed in Common Infrastructure Platform (CIP)
- Monitoring and alerting provided by the team and Azure platform
- Access control to Azure with user/password + 2FA, and PIM for Test and Production subscriptions access

## 6. Logging, Monitoring, and Alerting

- Google Analytics for client-side events
- Azure App Insights for service analytics
- Alerting via Log Analytics in Azure Monitor
- Microsoft Clarify for screen recordings, use behaviour tracking, and life performance metrics

## 7. Data Storage, Retention, and Cleanse

- PII data retained for 7 years, then anonymized
- Service directory data retained indefinitely
- Other data (e.g., IdAM) retained for 1 year
- Data encrypted in flight and at rest
- Additional encryption for PII data

## 8. DNS

Public DNS records for each site (Find, Connect, Manage) as subdomains of education.gov.uk.

## 9. Patch & Update Management

PaaS services in Azure handle patch and update management. Renovate enabled for the GitHub mono repo to alert us to/automatically raise PRs for dependency updates

## 10. Resilience/Fault Tolerance

Provided by application plan and container orchestration in PaaS services. Application plan auto scales to n number of containers to support load.

## 11. Disaster Recovery

- Daily and weekly backups to storage account
- Recovery Point Objective: 24 hours
- Recovery Time Objective: 24 hours (maximum)

## 12. DevOps

- Single GitHub repository for all Family Hubs services
- Terraform repository for IAC source
- Managed pipeline in GitHub Actions for both code and IAC deployments
- Open-source code management with feature branching and Pull Requests

## 13. Assurances

- Various testing types (unit, regression, code security, accessibility, end-to-end, performance)
- IT Health Checks and penetration tests performed every 2 years, or when moving to new live statuses as per GDS and DfE guidelines
