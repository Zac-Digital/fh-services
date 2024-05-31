using FamilyHubs.SharedKernel.Razor.FamilyHubsUi.Options;
using FamilyHubs.SharedKernel.Razor.FamilyHubsUi.Options.Configure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FamilyHubs.SharedKernel.Razor.FamilyHubsUi.Extensions;

public static class IServiceCollectionExtensions
{
    public static IServiceCollection AddFamilyHubsUi(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<FamilyHubsUiOptions>(configuration.GetSection(FamilyHubsUiOptions.FamilyHubsUi))
            .AddSingleton<IValidateOptions<FamilyHubsUiOptions>, FamilyHubsUiOptionsValidation>()
            .AddSingleton<IConfigureOptions<FamilyHubsUiOptions>, FamilyHubsUiOptionsConfigure>();
            //todo: should replace the above, but seems to pick up an old version of the above
            //.ConfigureOptions<FamilyHubsUiOptions>();

        services.AddOptions<FamilyHubsUiOptions>()
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.AddTransient<FamilyHubsLayoutModel>();

        return services;
    }
}