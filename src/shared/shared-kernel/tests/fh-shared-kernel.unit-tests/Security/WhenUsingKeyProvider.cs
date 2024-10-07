using FamilyHubs.SharedKernel.Security;
using FluentAssertions;
using Microsoft.Extensions.Configuration;

namespace FamilyHubs.SharedKernel.UnitTests.Security;

public class WhenUsingKeyProvider
{
    [Fact]
    public void GetDbEncryptionKey_ShouldReturnKey()
    {
        // Arrange
        var inMemorySettings = new Dictionary<string, string?> {
            {"Crypto:DbEncryptionKey", "DbEncryptionKey"}
        };

        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(inMemorySettings)
            .Build();

        var keyProvider = new KeyProvider(configuration);
        
        // Act
        string publicKey = keyProvider.GetDbEncryptionKey();

        // Assert
        publicKey.Should().Be("DbEncryptionKey");
    }
    
    [Fact]
    public void GetDbEncryptionKey_ShouldThrowException_WhenDbEncryptionKeyIsMissing()
    {
        // Arrange
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>())
            .Build();

        var keyProvider = new KeyProvider(configuration);
        
        // Act
        var exception = Assert.Throws<ArgumentException>(() => keyProvider.GetDbEncryptionKey());

        // Assert
        exception.Message.Should().Be("DbEncryptionKey value missing.");
    }
    
    [Fact]
    public void GetDbEncryptionIVKey_ShouldReturnIVKey()
    {
        // Arrange
        var inMemorySettings = new Dictionary<string, string?> {
            {"Crypto:DbEncryptionIVKey", "DbEncryptionIVKey"}
        };

        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(inMemorySettings)
            .Build();

        var keyProvider = new KeyProvider(configuration);
        
        // Act
        string publicKey = keyProvider.GetDbEncryptionIvKey();

        // Assert
        publicKey.Should().Be("DbEncryptionIVKey");
    }
    
    [Fact]
    public void GetDbEncryptionIVKey_ShouldThrowException_WhenDbEncryptionIVKeyIsMissing()
    {
        // Arrange
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>())
            .Build();

        var keyProvider = new KeyProvider(configuration);

        // Act
        var exception = Assert.Throws<ArgumentException>(() => keyProvider.GetDbEncryptionIvKey());

        // Assert
        exception.Message.Should().Be("DbEncryptionIVKey value missing.");
    }
}
