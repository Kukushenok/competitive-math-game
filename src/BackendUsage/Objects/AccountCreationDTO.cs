namespace CompetitiveBackend.BackendUsage.Objects
{
    public class AccountCreationDTO: AccountLoginDTO
    {
        public readonly string? Email;
        public AccountCreationDTO(string Login, string Password, string? email): base(Login, Password)
        {
            Email = email;
        }
    }
}
