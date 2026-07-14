using Microsoft.AspNetCore.DataProtection;



namespace LocalTransfer.Service
{
    public class UrlEncryptionService
    {
        private readonly IDataProtector _protector;

        public UrlEncryptionService(IDataProtectionProvider provider)
        {
            _protector = provider.CreateProtector("LocalTransfer.UrlProtection");
        }

        public string Encrypt(string value)
        {
            return _protector.Protect(value);
        }

        public string Decrypt(string encryptedValue)
        {
            return _protector.Unprotect(encryptedValue);
        }
    }
}
