using CompetitiveBackend.Core.Objects;
using Microsoft.Extensions.Configuration;
using ServicesRealisation.Objects;
using ServicesRealisation.ServicesRealisation.Validator.Constraints;

namespace ServicesRealisation.ServicesRealisation.Validator
{
    public class PlayerAccountValidator : ConfigurationValidator<PlayerProfile>, IValidator<Account>, IValidator<AccountCreationData>
    {
        private readonly MinMaxIntConstraint descriptionLength;
        private readonly RegexConstraint nameRegex;
        private readonly RegexConstraint emailRegex;
        private readonly MinMaxIntConstraint nameLength;
        private readonly PasswordConstraint password;
        public PlayerAccountValidator(IConfiguration configuration)
            : base(configuration, "Player")
        {
            descriptionLength = Read(new MinMaxIntConstraint(0, 64), nameof(descriptionLength));
            nameRegex = Read(new RegexConstraint(), nameof(nameRegex));
            nameLength = Read(new MinMaxIntConstraint(4, 32), "NameLength");
            emailRegex = Read(new RegexConstraint(), nameof(emailRegex));
            password = Read(new PasswordConstraint(5, false), "Password");
        }

        public override bool IsValid(PlayerProfile value, out string? msg)
        {
            return descriptionLength.IsValid(value.Description?.Length ?? 0, out msg)
                && nameLength.IsValid(value.Name?.Length ?? 0, out msg);
        }

        public bool IsValid(Account value, out string? msg)
        {
            msg = null;
            if (value.Login?.Contains(' ') ?? false)
            {
                msg = "Login should not have spaces";
            }

            if (value.Email != null)
            {
                emailRegex.IsValid(value.Email, out msg);
            }

            return msg == null && nameRegex.IsValid(value.Login ?? string.Empty, out msg)
                && nameLength.IsValid(value.Login?.Length ?? 0, out msg);
        }

        public bool IsValid(AccountCreationData value, out string? msg)
        {
            return IsValid(value.Account, out msg) && password.IsValid(value.Password, out msg);
        }
    }
}
