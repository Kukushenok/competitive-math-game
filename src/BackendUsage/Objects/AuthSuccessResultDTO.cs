using System;

namespace CompetitiveBackend.BackendUsage.Objects
{
    [Serializable]
    public class AuthSuccessResultDTO
    {
        public string Token { get; set; }
        public string RoleName { get; set; }
        public int AccountID { get; set; }
        public AuthSuccessResultDTO(string token, string roleName, int accountID)
        {
            Token = token;
            RoleName = roleName;
            AccountID = accountID;
        }
    }
}
