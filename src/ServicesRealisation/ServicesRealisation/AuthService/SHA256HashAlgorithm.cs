using System.Security.Cryptography;
using System.Text;
using CompetitiveBackend.Services.Objects;

namespace CompetitiveBackend.Services.AuthService
{
    internal sealed class SHA256HashAlgorithm : IHashAlgorithm
    {
        private readonly SHA256 sha256 = SHA256.Create();
        public string Hash(string input)
        {
            byte[] hashValue;
            var objUtf8 = new UTF8Encoding();
            hashValue = sha256.ComputeHash(objUtf8.GetBytes(input));
            return Convert.ToBase64String(hashValue);
        }

        public bool Verify(string input, string hash)
        {
            return Hash(input) == hash;
        }
    }
}
