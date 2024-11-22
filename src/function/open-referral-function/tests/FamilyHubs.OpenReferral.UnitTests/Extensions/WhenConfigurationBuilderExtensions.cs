using Azure.Security.KeyVault.Secrets;
using static FamilyHubs.OpenReferral.Function.Extensions.ConfigurationBuilderExtensions;

namespace FamilyHubs.OpenReferral.UnitTests.Extensions;

public class WhenConfigurationBuilderExtensions
{
    [Fact]
    public void Then_Load_ReturnsTrue_WhenSecretNameStartsWithPrefix()
    {
        // Arrange
        const string prefix = "test-prefix";
        var secretProperties = new SecretProperties($"{prefix}-secret-name");
        var manager = new PrefixKeyVaultSecretManager(prefix);

        // Act
        var result = manager.Load(secretProperties);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Then_Load_ReturnsFalse_WhenSecretNameDoesNotStartWithPrefix()
    {
        // Arrange
        const string prefix = "test-prefix";
        var secretProperties = new SecretProperties("other-secret-name");
        var manager = new PrefixKeyVaultSecretManager(prefix);

        // Act
        var result = manager.Load(secretProperties);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void Then_GetKey_ReturnsExpectedKey()
    {
        // Arrange
        const string prefix = "test-prefix";
        var secretName = $"{prefix}-secret--name";
        var secret = new KeyVaultSecret(secretName, "secret-value");
        var manager = new PrefixKeyVaultSecretManager(prefix);

        // Act
        var result = manager.GetKey(secret);

        // Assert
        const string expectedKey = "secret:name";
        Assert.Equal(expectedKey, result);
    }
}