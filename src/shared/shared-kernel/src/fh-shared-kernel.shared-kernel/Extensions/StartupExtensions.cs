using Azure.Extensions.AspNetCore.Configuration.Secrets;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.Extensions.Configuration;

namespace FamilyHubs.SharedKernel.Extensions;

public static class StartupExtensions
{
    public static IConfigurationBuilder ConfigureAzureKeyVault(this IConfigurationBuilder configurationBuilder)
    {
        var tempConfig = configurationBuilder.Build();
        var keyVaultPrefix = tempConfig["AppConfiguration:KeyVaultPrefix"];
        var keyVaultIdentifier = tempConfig["AppConfiguration:KeyVaultIdentifier"];

        if (!string.IsNullOrEmpty(keyVaultPrefix) && !string.IsNullOrEmpty(keyVaultIdentifier))
        {
            return configuration.AddAzureKeyVault(
                new Uri($"https://{keyVaultIdentifier}.vault.azure.net/"),
                new DefaultAzureCredential(),
                new PrefixKeyVaultSecretManager(keyVaultPrefix));
        }

        return configurationBuilder;
    }

    public sealed class PrefixKeyVaultSecretManager(string prefix) : KeyVaultSecretManager
    {
        private readonly string _prefix = $"{prefix}-";
        
        public override bool Load(SecretProperties secret)
            => secret.Name.StartsWith(_prefix);

        public override string GetKey(KeyVaultSecret secret)
            => secret.Name[_prefix.Length..].Replace("--", ConfigurationPath.KeyDelimiter);
    }
}