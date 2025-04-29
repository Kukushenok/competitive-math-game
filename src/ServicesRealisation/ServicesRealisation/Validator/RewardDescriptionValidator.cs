using CompetitiveBackend.Core.Objects;
using Microsoft.Extensions.Configuration;
using ServicesRealisation.ServicesRealisation.Validator.Constraints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicesRealisation.ServicesRealisation.Validator
{
    public class RewardDescriptionValidator : ConfigurationValidator<RewardDescription>
    {
        private MinMaxIntConstraint DescriptionLength;
        private MinMaxIntConstraint NameLength;
        public RewardDescriptionValidator(IConfiguration configuration) : base(configuration)
        {
            DescriptionLength = Read(new MinMaxIntConstraint(0, 128), nameof(DescriptionLength));
            NameLength = Read(new MinMaxIntConstraint(4, 64), nameof(NameLength));
        }
        public override bool IsValid(RewardDescription value, out string? msg)
        {
            return NameLength.IsValid(value.Name.Length, out msg) && 
                DescriptionLength.IsValid(value.Description?.Length ?? 0, out msg);
        }
    }
}
