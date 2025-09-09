using System;

namespace CompetitiveBackend.BackendUsage.Objects
{
    [Serializable]
    public class AccountLoginDTO
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public AccountLoginDTO(string login, string password)
        {
            Login = login;
            Password = password;
        }
    }
}
