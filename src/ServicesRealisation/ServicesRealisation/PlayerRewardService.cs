using CompetitiveBackend.Core;
using CompetitiveBackend.Core.Objects;
using CompetitiveBackend.Repositories;
using Microsoft.Extensions.Logging;

namespace CompetitiveBackend.Services.PlayerRewardService
{
    public class PlayerRewardService : IPlayerRewardService
    {
        private IPlayerRewardRepository _repository;
        private IRewardDescriptionRepository _rewardDescriptionRepository;
        public PlayerRewardService(IPlayerRewardRepository repository, IRewardDescriptionRepository rewardDescriptionRepository)
        {
            _repository = repository;
            _rewardDescriptionRepository = rewardDescriptionRepository;
        }

        public async Task DeleteReward(int playerRewardID)
        {
            await _repository.DeleteReward(playerRewardID);
        }

        public async Task<IEnumerable<PlayerReward>> GetAllRewardsOf(int playerID, DataLimiter limiter)
        {
            return await _repository.GetAllRewardsOf(playerID, limiter);
        }

        public async Task GrantRewardToPlayer(int playerID, int rewardDescriptionID)
        {
            RewardDescription rw = await _rewardDescriptionRepository.GetRewardDescription(rewardDescriptionID);
            PlayerReward reward = new PlayerReward(playerID,
                rewardDescriptionID,
                rw.Name,
                rw.Description,
                rewardDate: DateTime.Now);
            await _repository.CreateReward(reward);
        }
    }
}
