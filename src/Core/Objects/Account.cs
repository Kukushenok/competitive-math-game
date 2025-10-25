namespace CompetitiveBackend.Core.Objects
{
    public sealed class Account : IntIdentifiable
    {
        public readonly string Login;
        public readonly string? Email;
        public Account(string login, string? email = null, int? id = null)
            : base(id)
        {
            Login = login;
            Email = email;
        }
    }

    public record AccountRegisterInfo
    {
        public required string Login { get; init; }
        public required string Password { get; init; }
    }
}
