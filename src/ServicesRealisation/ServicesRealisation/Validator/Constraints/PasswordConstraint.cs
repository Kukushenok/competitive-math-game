using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicesRealisation.ServicesRealisation.Validator.Constraints
{
    public class PasswordConstraint : IValidator<string>, IConfigReadable
    {
        public int MinLength { get; private set; }
        public bool RequiresLetters {  get; private set; }
        public PasswordConstraint(int minLength, bool requiresLetters)
        {
            MinLength = minLength;
            RequiresLetters = requiresLetters;
        }

        public bool IsValid(string value, out string? msg)
        {
            msg = null;
            if (value.Length <= MinLength) { msg = $"Password should be at least {MinLength} symbols long"; return false; }
            string upper = value.ToUpper();
            string lower = value.ToLower();
            if (RequiresLetters && upper == lower) { msg = "There should be letters in password"; return false; }
            return true;
        }

        public void Read(IConfigurationSection section)
        {
            MinLength = section.GetValue(nameof(MinLength), MinLength);
            RequiresLetters = section.GetValue(nameof(RequiresLetters), RequiresLetters);
        }
    }
}
