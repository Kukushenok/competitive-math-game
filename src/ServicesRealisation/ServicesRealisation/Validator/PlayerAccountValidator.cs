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
        private MinMaxIntConstraint descriptionLength;
        private MinMaxIntConstraint nameLength;
        private PasswordConstraint password;
        public PlayerAccountValidator(IConfiguration configuration) : base(configuration, "Player")
        {
            descriptionLength = Read(new MinMaxIntConstraint(0, 64), nameof(descriptionLength));
            nameLength = Read(new MinMaxIntConstraint(4, 32), "NameLength");
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
            if (value.Login?.Contains(' ') ?? false) msg = "Login should not have spaces";
            return msg == null && nameLength.IsValid(value.Login?.Length ?? 0, out msg);
        }

        public bool IsValid(AccountCreationData value, out string? msg)
        {
            return IsValid(value.Account, out msg) && password.IsValid(value.Password, out msg);
        }
    }
}
