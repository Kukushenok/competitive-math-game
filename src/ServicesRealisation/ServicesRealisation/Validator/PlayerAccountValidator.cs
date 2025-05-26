using CompetitiveBackend.Core.Objects;
using Microsoft.Extensions.Configuration;
using ServicesRealisation.Objects;
using ServicesRealisation.ServicesRealisation.Validator.Constraints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicesRealisation.ServicesRealisation.Validator
{
    public class PlayerAccountValidator : ConfigurationValidator<PlayerProfile>, IValidator<Account>, IValidator<AccountCreationData>
    {
        private MinMaxIntConstraint DescriptionLength;
        private RegexConstraint NameRegex;
        private RegexConstraint EmailRegex;
        private MinMaxIntConstraint nameLength;
        private PasswordConstraint password;
        public PlayerAccountValidator(IConfiguration configuration) : base(configuration, "Player")
        {
            DescriptionLength = Read(new MinMaxIntConstraint(0, 64), nameof(DescriptionLength));
            NameRegex = Read(new RegexConstraint(), nameof(NameRegex));
            nameLength = Read(new MinMaxIntConstraint(4, 32), "NameLength");
            EmailRegex = Read(new RegexConstraint(), nameof(EmailRegex));
            password = Read(new PasswordConstraint(5, false), "Password");
        }

        public override bool IsValid(PlayerProfile value, out string? msg)
        {
            return DescriptionLength.IsValid(value.Description?.Length ?? 0, out msg)
                && nameLength.IsValid(value.Name?.Length ?? 0, out msg);
        }

        public bool IsValid(Account value, out string? msg)
        {
            msg = null;
            if (value.Login?.Contains(' ') ?? false) msg = "Login should not have spaces";
            if (value.Email != null) EmailRegex.IsValid(value.Email, out msg);
            return msg == null && NameRegex.IsValid(value.Login, out msg)
                && nameLength.IsValid(value.Login?.Length ?? 0, out msg);
        }

        public bool IsValid(AccountCreationData value, out string? msg)
        {
            return IsValid(value.Account, out msg) && password.IsValid(value.Password, out msg);
        }
    }
}
