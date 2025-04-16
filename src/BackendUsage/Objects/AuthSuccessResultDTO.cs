using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompetitiveBackend.BackendUsage.Objects
{
    public class AuthSuccessResultDTO
    {
        public readonly string Token;
        public readonly string RoleName;
        public readonly int AccountID;
        public AuthSuccessResultDTO(string token, string roleName, int accountID)
        {
            Token = token;
            RoleName = roleName;
            AccountID = accountID;
        }
    }
}
