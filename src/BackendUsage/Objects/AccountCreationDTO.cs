using System;

namespace CompetitiveBackend.BackendUsage.Objects
{
    [Serializable]
    public class AccountCreationDTO : AccountLoginDTO
    {
        public string? Email { get; set; }
        public AccountCreationDTO(string login, string password, string? email)
            : base(login, password)
        {
            Email = email;
        }
    }
}
