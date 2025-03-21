# HTTP Headers

HTTP security headers are managed by both the web sites and App Gateway working together.

## Web Site

These headers are managed by the web sites:

Added

* x-frame-options
* x-xss-protection
* x-content-type-options
* referrer-policy
* content-security-policy
* permissions-policy
* x-permitted-cross-domain-policies

The source code for implementing the above can be found in [GitHub](https://github.com/DFE-Digital/fh-service-directory-ui/blob/main/src/FamilyHubs.ServiceDirectory.Web/Security/SecurityHeaders.cs). The code uses the [NetEscapades.AspNetCore.SecurityHeaders](https://github.com/andrewlock/NetEscapades.AspNetCore.SecurityHeaders) NuGet package.

## App Gateway

These headers are managed by the web sites:

Added

* strict-transport security

Removed

* server
* x-powered-by

## Security Scan Results

Here’s a security scan of the headers returned by the Find website in prod. There’s a warning about the CSP containing unsafe-inline, but that’s required for Google Analytics to function correctly.

![](../img/HTTP%20Headers%20Family%20Hubs.png)

## Combined Headers

The set of response headers generated when the website and App Gateway are working in conjunction are as follows:

![](../img/HTTP%20Headers%20Family%20Hubs%20Combined.png)