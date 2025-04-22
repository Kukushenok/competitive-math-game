using System;

namespace CompetitiveBackend.BackendUsage.Objects
{
    [Serializable]
    public class AccountCreationDTO: AccountLoginDTO
    {
        public string? Email { get; set; }
        public AccountCreationDTO(string Login, string Password, string? email): base(Login, Password)
        {
            Email = email;
        }
    }
}
