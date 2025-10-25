using System.Text.RegularExpressions;
using Microsoft.Extensions.Configuration;

namespace ServicesRealisation.ServicesRealisation.Validator.Constraints
{
    internal sealed class RegexConstraint : IValidator<string>, IConfigReadable
    {
        private string pattern = ".*";
        public RegexConstraint(string pattern = ".*")
        {
            this.pattern = pattern;
        }

        public bool IsValid(string value, out string? msg)
        {
            msg = null;
            if (!Regex.IsMatch(value ?? string.Empty, pattern))
            {
                msg = $"Not validating the regex pattern {msg}";
            }

            return msg == null;
        }

        public void Read(IConfigurationSection section)
        {
            pattern = section[nameof(pattern)] ?? pattern;
        }
    }
}
