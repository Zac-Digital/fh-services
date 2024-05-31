using FamilyHubs.SharedKernel.Razor.FamilyHubsUi.Extensions;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FamilyHubs.SharedKernel.Razor.Urls;

public static class HtmlHelperFamilyHubExtensions
{
    public static Uri FamilyHubUrl<TUrlKeyEnum>(this IHtmlHelper htmlHelper, TUrlKeyEnum baseUrl, string? url)
        where TUrlKeyEnum : struct, Enum
    {
        return htmlHelper.ViewData.GetFamilyHubsUiOptions().Url(baseUrl, url);
    }

    public static Uri FamilyHubConfigUrl<TUrlKeyEnum>(this IHtmlHelper htmlHelper, TUrlKeyEnum baseUrl, string configKey)
        where TUrlKeyEnum : struct, Enum
    {
        var configuration = htmlHelper.ViewContext.HttpContext.RequestServices.GetRequiredService<IConfiguration>();

        return htmlHelper.FamilyHubUrl(baseUrl, configuration[configKey]);
    }
}