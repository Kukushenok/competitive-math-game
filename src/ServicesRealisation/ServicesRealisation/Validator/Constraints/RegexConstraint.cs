using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ServicesRealisation.ServicesRealisation.Validator.Constraints
{
    internal class RegexConstraint : IValidator<string>, IConfigReadable
    {
        private string pattern = "*";
        public RegexConstraint(string pattern = "*")
        {
            this.pattern = pattern;
        }

        public bool IsValid(string value, out string? msg)
        {
            msg = null;
            if (!Regex.IsMatch(value, pattern)) msg = $"Not validating the regex pattern {msg}";
            return msg == null;
        }

        public void Read(IConfigurationSection section)
        {
            pattern = section[nameof(pattern)] ?? pattern;
        }
    }
}
