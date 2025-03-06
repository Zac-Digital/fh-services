
# <!-- Identifier: --> ADR028 - Implement technical documentation site <!-- Title: --> TITLE

- **Status**: Draft
- **Date**: 2025-03-05
- **Author**: Aaron Yarborough

## Decision

...

## Context

We are in the process of transferring technical documentation from Confluence, which is accessible only to DfE users, to GitHub. This transition aims to enhance accessibility for others to understand and build upon our software, for which technical documentation plays an important role. Other projects within the VC&F portfolio have already adopted tools like static site generators to improve document viewing and searchability through a user-friendly interface, rather than having users depend on navigating the GitHub folder structure and using its search feature, which can be unintuitive (e.g. easy to accidentally search all of GitHub, rather than the specific directory containing documentation)

## Options considered

1. Continue using GitHub's repository explorer
2. Use DfE's [tech-docs-template](https://github.com/DFE-Digital/tech-docs-template)
3. Use GovUK One Login's [tech-docs](https://github.com/govuk-one-login/tech-docs)
4. Copy [Care Leaver's approach using MKDocs](https://github.com/DFE-Digital/care-leavers/tree/main/resources/tech_docs_template)


## Consequences

### Option 1 - Continue using GitHub's repository explorer

* Requires no additional components/architecture change
* The GitHub UI presents the user with technical language and components they're not necessarily used to (technical README, various tabs at the top of the UI, license information, etc.), and does not make sense for someone who purely wants to read technical documentation
* The GitHub search function defaults to searching the full repo, meaning code search results will be shown and not just document search results, potentially further confusing the users 

### Option 2 - Use DfE's [tech-docs-template](https://github.com/DFE-Digital/tech-docs-template)

* Requires additional components/architecture
* Is a static site generator, meaning we can use GitHub pages to host the output HTML files
* Is a solution recommended by the DfE
* Doesn't look like other DfE technical docs, as the DfE design kit isn't used

### Option 3 - Use GovUK One Login's [tech-docs](https://github.com/govuk-one-login/tech-docs)

* Requires additional components/architecture,


### Option 4 - Copy [Care Leaver's approach using MKDocs](https://github.com/DFE-Digital/care-leavers/tree/main/resources/tech_docs_template)

* Requires additional components/architecture
* Is themed to look like other Government technical documentation sites
* Is a static site generator, meaning we can use GitHub pages to host the output HTML files
* Isn't (yet) an officially recommended DfE approach, though it uses the same underlying technology (MKDOcs) as Option 3 - the only difference is the theme to make it look like other DfE technical docs
* Is being used already on Care Leavers, meaning we're more in-line with other projects on the VC&F portfolio

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
- Josh
- Other portfolio members

**Harry Young (Care Leavers)**

- It's possible to further adopt the DfE design kit in future, making option4 look even more like the classic DfE docs in option 3 (essentially solving the small gripe on things looking slightly different to the DfE docs)
- Solution copied from https://github.com/ministryofjustice/mkdocs-tech-docs-template/tree/main/tech_docs_template originally and MOJ references removed
- Further complexity in hosting a Ruby site; easier to just use MKDocs