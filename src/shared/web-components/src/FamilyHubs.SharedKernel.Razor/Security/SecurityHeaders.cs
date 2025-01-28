﻿using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;

namespace FamilyHubs.SharedKernel.Razor.Security;

public static class SecurityHeaders
{
    /// <summary>
    /// nuget documentation
    /// https://github.com/andrewlock/NetEscapades.AspNetCore.SecurityHeaders
    /// csp introduction
    /// https://scotthelme.co.uk/content-security-policy-an-introduction/
    /// jquery csp
    /// https://content-security-policy.com/examples/jquery/
    /// </summary>
    public static WebApplication UseAppSecurityHeaders(this WebApplication app)
    {
#pragma warning disable S1075
        app.UseSecurityHeaders(policies =>
            policies.AddDefaultSecurityHeaders()
                .AddContentSecurityPolicy(builder =>
                {
                    builder.AddUpgradeInsecureRequests();

                    var defaultSrc = builder.AddDefaultSrc()
                        .Self();

                    var connectSrc = builder.AddConnectSrc()
                        .Self()
                        .From(new[]
                        {
                            "https://*.google-analytics.com",
                            "https://*.analytics.google.com",
                            "https://*.googletagmanager.com",
                            "https://*.clarity.ms",
                            "https://c.bing.com",
                            //todo: is this needed in prod?
                            /* application insights*/ "https://dc.services.visualstudio.com/v2/track", "rt.services.visualstudio.com/v2/track"
                        });

                    builder.AddFontSrc()
                        .Self();

                    builder.AddObjectSrc()
                        .None();

                    builder.AddFormAction()
                        .Self();

                    builder.AddImgSrc()
                        .Self()
                        .From(new[]
                        {
                            "https://*.google-analytics.com",
                            "https://*.analytics.google.com",
                            "https://*.googletagmanager.com",
                            "https://*.clarity.ms",
                            "https://c.bing.com"
                        });

                    builder.AddScriptSrc()
                        .UnsafeInline()
                        .StrictDynamic()
                        .WithNonce();

                    builder.AddStyleSrc()
                        .Self()
                        .StrictDynamic();

                    builder.AddMediaSrc()
                        .None();

                    builder.AddFrameAncestors()
                        .None();

                    builder.AddBaseUri()
                        .Self();

                    if (app.Environment.IsDevelopment())
                    {
                        // open up for browserlink
                        defaultSrc.From(new[] { "http://localhost:*", "ws://localhost:*" });

                        connectSrc.From(new[] { "http://localhost:*", "https://localhost:*", "ws://localhost:*", "wss://localhost:*" });
                    }
                })
                .AddPermissionsPolicy(builder =>
                {
                    builder.AddAccelerometer()
                        .None();
                    builder.AddAmbientLightSensor()
                        .None();
                    builder.AddAutoplay()
                        .None();
                    builder.AddCamera()
                        .None();
                    builder.AddEncryptedMedia()
                        .None();
                    builder.AddFullscreen()
                        .None();
                    builder.AddGeolocation()
                        .None();
                    builder.AddGyroscope()
                        .None();
                    builder.AddMagnetometer()
                        .None();
                    builder.AddMicrophone()
                        .None();
                    builder.AddMidi()
                        .None();
                    builder.AddPayment()
                        .None();
                    builder.AddPictureInPicture()
                        .None();
                    builder.AddSpeaker()
                        .None();
                    // don't need it yet, but we probably will when we enable js filtering, and we don't want to set a trap
                    builder.AddSyncXHR()
                        .Self();
                    builder.AddUsb()
                        .None();
                    builder.AddVR()
                        .None();
                })
                .AddCustomHeader("X-Permitted-Cross-Domain-Policies", "none")
                .AddXssProtectionDisabled());
#pragma warning restore S1075

        return app;
    }
}