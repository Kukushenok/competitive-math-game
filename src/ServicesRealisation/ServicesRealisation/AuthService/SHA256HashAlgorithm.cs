using CompetitiveBackend.Services.Objects;
using System.Security.Cryptography;
using System.Text;

namespace CompetitiveBackend.Services.AuthService
{
    class SHA256HashAlgorithm : IHashAlgorithm
    {
        SHA256 sha256 = SHA256.Create();
        public string Hash(string input)
        {
            byte[] hashValue;
            UTF8Encoding objUtf8 = new UTF8Encoding();
            hashValue = sha256.ComputeHash(objUtf8.GetBytes(input));
            return Convert.ToBase64String(hashValue);
        }

        public bool Verify(string input, string hash)
        {
            return Hash(input) == hash;
        }
    }
}
