# Secrets and where they are used

**IAM roles**

Each application accesses azure resources (possibly just keyvault?) by authenticating as an “Enterprise application” using a clientId and clientSecret.

Rough flow as below.

![](../img/Screenshot%20Jul%2016%202024%20from%20Family%20Hubs.png)

The client secrets within an app registration have a user-defined expiry usually 12-24 months. When these expire it’s necessary to do the following:

* Generate a new secret in the app registration
* Update the ADO pipelines (or maybe github soon™) to substitute the new secret value
* Probably not as afaik these values are stored in the very keyvault they allow access to. It’s chicken and egg.
* Start a new pipeline with the new values

We shouldn’t ever need to change the clientid or access policies but in the case that we can’t get ownership of an app registration it’s possible to work around by:

* Creating a new app registration
* Generating a new secret
* Update pipelines with new values:
  + clientId - Application (client) ID from the app registration
  + clientSecret - Value of the generated secret in the app registration
* Update values in terraform:
  + accesspolicy4\_objectid (Or similar) - Object ID from the enterprise application
  + clientId/clientSecret optionally, I don’t think these values are actually used.
* Start a new pipeline with the new values