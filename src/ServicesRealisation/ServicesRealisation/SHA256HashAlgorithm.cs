using CompetitiveBackend.Core.Auth;
using CompetitiveBackend.Core.Objects;
using CompetitiveBackend.Services.Objects;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ServicesRealisation.ServicesRealisation
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
    class BasicRoleCreator : IRoleCreator
    {
        public Role Create(Account c)
        {
            if (c.Login == "root") return new AdminRole();
            return new PlayerRole();
        }
    }
}
