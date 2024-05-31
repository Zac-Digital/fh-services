using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.DataEncryption.Providers;
using System.Security.Cryptography;

namespace FamilyHubs.Idams.Maintenance.UI.Pages;

public class CreateEncryptionKeysModel : PageModel
{
    public string DbEncryptionKey { get; private set; } = string.Empty;
    public string DbEncryptionIVKey {  get; private set; } = string.Empty;

    public void OnGet()
    {
        using (var rsa = RSA.Create())
        {
            var result = AesProvider.GenerateKey(AesKeySize.AES256Bits);
            List<string> items = new List<string>();
            foreach (var item in result.IV)
            {
                items.Add(item.ToString());
            }
            DbEncryptionIVKey = string.Join(',', items.ToArray());
            items.Clear();

            foreach (var item in result.Key)
            {
                items.Add(item.ToString());
            }
            DbEncryptionKey = string.Join(',', items.ToArray());
        }
    }
}
