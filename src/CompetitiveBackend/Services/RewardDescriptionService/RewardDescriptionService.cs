using CompetitiveBackend.Core.Objects;
using CompetitiveBackend.Core;
using CompetitiveBackend.Repositories;

namespace CompetitiveBackend.Services.RewardDescriptionService
{
    public class RewardDescriptionService : LogService<RewardDescriptionService>, IRewardDescriptionService
    {
        private readonly IRewardDescriptionRepository _repository;
        private readonly ILargeFileProcessor _imageProcessor;
        public RewardDescriptionService(ILogger<RewardDescriptionService> logger, IRewardDescriptionRepository repository, ILargeFileProcessor imageProcessor) : base(logger)
        {
            _repository = repository;
            _imageProcessor = imageProcessor;
        }

        public async Task CreateRewardDescription(RewardDescription description)
        {
            await _repository.CreateRewardDescription(description);
        }

        public async Task<IEnumerable<RewardDescription>> GetAllRewardDescriptions(DataLimiter limiter)
        {
            return await _repository.GetAllRewardDescriptions(limiter);
        }

        public async Task<RewardDescription> GetRewardDescription(int rewardID)
        {
            return await GetRewardDescription(rewardID);
        }

        public async Task<LargeData> GetRewardGameAsset(int rewardID)
        {
            return await GetRewardGameAsset(rewardID);
        }

        public async Task<LargeData> GetRewardIcon(int rewardID)
        {
            return await GetRewardIcon(rewardID);
        }

        public async Task SetRewardGameAsset(int rewardID, LargeData data)
        {
            await SetRewardGameAsset(rewardID, data);
        }

        public async Task SetRewardIcon(int rewardID, LargeData data)
        {
            LargeData processed = await _imageProcessor.Process(data);
            await _repository.SetRewardIcon(rewardID, processed);
        }
    }
}
