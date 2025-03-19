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
        private MinMaxIntConstraint descriptionLength;
        private MinMaxIntConstraint nameLength;
        public RewardDescriptionValidator(IConfiguration configuration) : base(configuration)
        {
            descriptionLength = Read(new MinMaxIntConstraint(0, 128), nameof(descriptionLength));
            nameLength = Read(new MinMaxIntConstraint(4, 64), nameof(nameLength));
        }
        public override bool IsValid(RewardDescription value, out string? msg)
        {
            return nameLength.IsValid(value.Name.Length, out msg) && 
                descriptionLength.IsValid(value.Description?.Length ?? 0, out msg);
        }
    }
}
