namespace CompetitiveBackend.BackendUsage.Objects
{
    public class AccountLoginDTO
    {
        public readonly string Login;
        public readonly string Password;
        public AccountLoginDTO(string login, string password)
        {
            Login = login;
            Password = password;
        }
    }
}
