using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace FamilyHubs.SharedKernel.Cookies;

public static class CookieStartupOptions
{
    public static void AddCookieStartupOptions(this IServiceCollection services) => services.AddAntiforgery(options =>
    {
        options.Cookie.HttpOnly = true;
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    });
}