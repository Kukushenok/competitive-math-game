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
        private MinMaxIntConstraint descriptionLength;
        private MinMaxIntConstraint nameLength;

        public CompetitionValidator(IConfiguration configuration) : base(configuration)
        {
            descriptionLength = Read(new MinMaxIntConstraint(0, 128), nameof(descriptionLength));
            nameLength = Read(new MinMaxIntConstraint(4, 64), nameof(nameLength));
        }

        public override bool IsValid(Competition value, out string? msg)
        {
            if(value.StartDate >= value.EndDate)
            {
                msg = "End date should be after start date";
                return false;
            }
            if (!nameLength.IsValid(value.Name.Length, out msg)) return false;
            if (!descriptionLength.IsValid(value.Description?.Length ?? 0, out msg)) return false;
            return true;
                
        }
    }
}
