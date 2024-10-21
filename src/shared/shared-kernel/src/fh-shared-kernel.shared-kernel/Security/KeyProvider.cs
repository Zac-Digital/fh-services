using Microsoft.Extensions.Configuration;

namespace FamilyHubs.SharedKernel.Security;

public interface IKeyProvider
{
    string GetDbEncryptionKey();
    string GetDbEncryptionIvKey();
}

public class KeyProvider : IKeyProvider
{
    private readonly IConfiguration _configuration;
    
    public KeyProvider(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string GetDbEncryptionKey() 
        => _configuration.GetValue<string>("Crypto:DbEncryptionKey") 
           ?? throw new ArgumentException("DbEncryptionKey value missing.");

    public string GetDbEncryptionIvKey()
        => _configuration.GetValue<string>("Crypto:DbEncryptionIVKey")
           ?? throw new ArgumentException("DbEncryptionIVKey value missing.");
}