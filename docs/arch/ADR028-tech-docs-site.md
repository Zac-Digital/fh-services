<!-- 
    Choose an identifier for the ADR by adding 1 to the previous ADR's id. 

    Also choose a title, which should be a very short description of the 
    decision itself. Make it specific.    
-->

# <!-- Identifier: --> ADR028 - Implement technical documentation site <!-- Title: --> TITLE

<!-- Metadata section. All fields are mandatory. -->
- **Status**: Draft
- **Date**: 2025-03-05 <!-- The day the draft was started, in the YYYY-MM-DD format, for example '1970-01-01' -->
- **Author**: Aaron Yarborough<!-- Your full name as the owner of the decision, for example 'Joe Bloggs'. -->

## Decision

TODO

## Context

<!-- 
    Describe the forces and circumstances that brought about this decision. 
-->
We are in the process of transferring technical documentation from Confluence, which is accessible only to DfE users, to GitHub. This transition aims to enhance accessibility for others to understand and build upon our software, for which technical documentation plays an important role. Other projects within the VC&F portfolio have already adopted tools like static site generators to improve document viewing and searchability through a user-friendly interface, rather than having users depend on navigating the GitHub folder structure and using its search feature, which can be unintuitive (e.g. easy to accidentally search all of GitHub, rather than the specific directory containing documentation)

## Options considered

<!-- 
    Briefly describe each option considered as a numbered list. Start with the selected option.
    It's usually wise to include a 'do nothing' option.

    e.g.

    1. (SELECTED) PostgreSQL
    2. Oracle
    3. SQL Server  
-->

1. Continue using GitHub's repository explorer
2. Use DfE's [tech-docs-template](https://github.com/DFE-Digital/tech-docs-template)
3. Use GovUK One Login's [tech-docs](https://github.com/govuk-one-login/tech-docs)

## Consequences

<!-- 
    For each of the options above, describe positive and negative consequences
    of selecting that option. Create a new section for each option under a heading.

    Remember a law of architecture: There are no solutions, only trade-offs. Make
    sure to include any negative consequences of the selected option.

    e.g.

    ### Option 1 - XXX

    - Consequence 1
    - Consequence 2

    ### Option 2 - XXX

    etc.
-->

## Advice

To discuss with:
- Tester
- Developer
- Other portfolio members
