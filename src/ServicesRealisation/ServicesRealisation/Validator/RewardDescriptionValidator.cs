using CompetitiveBackend.Core.Objects;
using Microsoft.Extensions.Configuration;
using ServicesRealisation.ServicesRealisation.Validator.Constraints;

namespace ServicesRealisation.ServicesRealisation.Validator
{
    public class RewardDescriptionValidator : ConfigurationValidator<RewardDescription>
    {
        private readonly MinMaxIntConstraint descriptionLength;
        private readonly MinMaxIntConstraint nameLength;
        public RewardDescriptionValidator(IConfiguration configuration)
            : base(configuration)
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
