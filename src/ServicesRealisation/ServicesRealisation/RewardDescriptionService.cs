using CompetitiveBackend.Core.Objects;
using CompetitiveBackend.Repositories;
using CompetitiveBackend.Services.ExtraTools;
using ServicesRealisation.ServicesRealisation.Validator;

namespace CompetitiveBackend.Services.RewardDescriptionService
{
    public class RewardDescriptionService : IRewardDescriptionService
    {
        private readonly IRewardDescriptionRepository repository;
        private readonly IImageProcessor imageProcessor;
        private readonly IValidator<RewardDescription> rewardDescriptionValidator;
        public RewardDescriptionService(IRewardDescriptionRepository repository, IImageProcessor imageProcessor, IValidator<RewardDescription> rewardDescriptionValidator)
        {
            this.rewardDescriptionValidator = rewardDescriptionValidator;
            this.repository = repository;
            this.imageProcessor = imageProcessor;
        }

        public async Task CreateRewardDescription(RewardDescription description)
        {
            if (!rewardDescriptionValidator.IsValid(description, out string? msg))
            {
                throw new Exceptions.InvalidArgumentsException(msg!);
            }

            await repository.CreateRewardDescription(description);
        }

        public async Task<IEnumerable<RewardDescription>> GetAllRewardDescriptions(DataLimiter limiter)
        {
            return await repository.GetAllRewardDescriptions(limiter);
        }

        public async Task<RewardDescription> GetRewardDescription(int rewardDescrID)
        {
            return await repository.GetRewardDescription(rewardDescrID);
        }

        public async Task<LargeData> GetRewardIcon(int rewardID)
        {
            return await repository.GetRewardIcon(rewardID);
        }

        public async Task SetRewardIcon(int rewardID, LargeData data)
        {
            LargeData processed = await imageProcessor.Process(data);
            await repository.SetRewardIcon(rewardID, processed);
        }

        public async Task UpdateRewardDescription(int rewardDescrID, string? name = null, string? description = null)
        {
            RewardDescription d = await repository.GetRewardDescription(rewardDescrID);
            var nw = new RewardDescription(name ?? d.Name, description ?? d.Description, rewardDescrID);
            if (!rewardDescriptionValidator.IsValid(nw, out string? msg))
            {
                throw new Exceptions.InvalidArgumentsException(msg!);
            }

            await repository.UpdateRewardDescription(nw);
        }
    }
}
