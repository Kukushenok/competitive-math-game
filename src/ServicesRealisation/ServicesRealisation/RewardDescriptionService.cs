using CompetitiveBackend.Core.Objects;
using CompetitiveBackend.Repositories;
using CompetitiveBackend.Services.ExtraTools;
using CompetitiveBackend.Services.Objects;
using ServicesRealisation.ServicesRealisation.Validator;

namespace CompetitiveBackend.Services.RewardDescriptionService
{
    public class RewardDescriptionService : IRewardDescriptionService
    {
        private readonly IRewardDescriptionRepository _repository;
        private readonly IImageProcessor _imageProcessor;
        private readonly IValidator<RewardDescription> _rewardDescriptionValidator;
        public RewardDescriptionService(IRewardDescriptionRepository repository, IImageProcessor imageProcessor, IValidator<RewardDescription> rewardDescriptionValidator)
        {
            _rewardDescriptionValidator = rewardDescriptionValidator;
            _repository = repository;
            _imageProcessor = imageProcessor;
        }

        public async Task CreateRewardDescription(RewardDescription description)
        {
            if(!_rewardDescriptionValidator.IsValid(description, out string? msg))
                throw new Exceptions.InvalidArgumentsException(msg!);
            await _repository.CreateRewardDescription(description);
        }

        public async Task<IEnumerable<RewardDescription>> GetAllRewardDescriptions(DataLimiter limiter)
        {
            return await _repository.GetAllRewardDescriptions(limiter);
        }

        public async Task<RewardDescription> GetRewardDescription(int rewardID)
        {
            return await _repository.GetRewardDescription(rewardID);
        }

        public async Task<LargeData> GetRewardGameAsset(int rewardID)
        {
            return await _repository.GetRewardGameAsset(rewardID);
        }

        public async Task<LargeData> GetRewardIcon(int rewardID)
        {
            return await _repository.GetRewardIcon(rewardID);
        }

        public async Task SetRewardGameAsset(int rewardID, LargeData data)
        {
            await _repository.SetRewardGameAsset(rewardID, data);
        }

        public async Task SetRewardIcon(int rewardID, LargeData data)
        {
            LargeData processed = await _imageProcessor.Process(data);
            await _repository.SetRewardIcon(rewardID, processed);
        }

        public async Task UpdateRewardDescription(int rewardDescrID, string? name = null, string? description = null)
        {
            RewardDescription d = await _repository.GetRewardDescription(rewardDescrID);
            RewardDescription nw = new RewardDescription(name ?? d.Name, description ?? d.Description, rewardDescrID);
            if (!_rewardDescriptionValidator.IsValid(nw, out string? msg))
                throw new Exceptions.InvalidArgumentsException(msg!);
            await _repository.UpdateRewardDescription(nw);
        }
    }
}
