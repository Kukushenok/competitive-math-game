using CompetitiveBackend.LogicComponents;

namespace CompetitiveBackend.Core.Objects
{
    public sealed class Account: IntIdentifiable
    {
        public readonly string Login;
        public readonly string? Email;
        public readonly string PasswordHash;
        public Account(string login, string passwordHash, string? email = null, int? id = null) : base(id)
        {
            Login = login;
            Email = email;
            PasswordHash = passwordHash;
        }
    }
    public record AccountRegisterInfo
    {
        public required string Login { get; init; }
        public required string Password { get; init; }
    }
}
