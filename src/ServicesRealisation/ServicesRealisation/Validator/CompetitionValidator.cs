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
    public class CompetitionValidator : ConfigurationValidator<Competition>
    {
        private MinMaxIntConstraint DescriptionLength;
        private MinMaxIntConstraint NameLength;

        public CompetitionValidator(IConfiguration configuration) : base(configuration)
        {
            DescriptionLength = Read(new MinMaxIntConstraint(0, 128), nameof(DescriptionLength));
            NameLength = Read(new MinMaxIntConstraint(4, 64), nameof(NameLength));
        }

        public override bool IsValid(Competition value, out string? msg)
        {
            if(value.StartDate >= value.EndDate)
            {
                msg = "End date should be after start date";
                return false;
            }
            if (!NameLength.IsValid(value.Name.Length, out msg)) return false;
            if (!DescriptionLength.IsValid(value.Description?.Length ?? 0, out msg)) return false;
            return true;
                
        }
    }
}
