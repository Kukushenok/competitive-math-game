using CompetitiveBackend.Core.Objects;
using Microsoft.Extensions.Configuration;
using ServicesRealisation.ServicesRealisation.Validator.Constraints;

namespace ServicesRealisation.ServicesRealisation.Validator
{
    public class CompetitionValidator : ConfigurationValidator<Competition>
    {
        private readonly MinMaxIntConstraint descriptionLength;
        private readonly MinMaxIntConstraint nameLength;

        public CompetitionValidator(IConfiguration configuration)
            : base(configuration)
        {
            descriptionLength = Read(new MinMaxIntConstraint(0, 128), nameof(descriptionLength));
            nameLength = Read(new MinMaxIntConstraint(4, 64), nameof(nameLength));
        }

        public override bool IsValid(Competition value, out string? msg)
        {
            if (value.StartDate >= value.EndDate)
            {
                msg = "End date should be after start date";
                return false;
            }

            return nameLength.IsValid(value.Name.Length, out msg) && descriptionLength.IsValid(value.Description?.Length ?? 0, out msg);
        }
    }
}
