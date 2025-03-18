using CompetitiveBackend.Core.Objects;
using CompetitiveBackend.Repositories;
using CompetitiveBackend.Services.Objects;

namespace CompetitiveBackend.Services.RewardDescriptionService
{
    public class RewardDescriptionService : IRewardDescriptionService
    {
        private readonly IRewardDescriptionRepository _repository;
        private readonly ILargeFileProcessor _imageProcessor;
        public RewardDescriptionService(IRewardDescriptionRepository repository, ILargeFileProcessor imageProcessor)
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
            await _repository.UpdateRewardDescription(d);
        }
    }
}
