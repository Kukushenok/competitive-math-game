using CompetitiveBackend.Core.Objects;
using CompetitiveBackend.Repositories;

namespace CompetitiveBackend.Services.PlayerRewardService
{
    public class PlayerRewardService : IPlayerRewardService
    {
        private readonly IPlayerRewardRepository repository;
        private readonly IRewardDescriptionRepository rewardDescriptionRepository;
        public PlayerRewardService(IPlayerRewardRepository repository, IRewardDescriptionRepository rewardDescriptionRepository)
        {
            this.repository = repository;
            this.rewardDescriptionRepository = rewardDescriptionRepository;
        }

        public async Task DeleteReward(int playerRewardID)
        {
            await repository.DeleteReward(playerRewardID);
        }

        public async Task<IEnumerable<PlayerReward>> GetAllRewardsOf(int playerID, DataLimiter limiter)
        {
            return await repository.GetAllRewardsOf(playerID, limiter);
        }

        public async Task GrantRewardToPlayer(int playerID, int rewardDescriptionID)
        {
            RewardDescription rw = await rewardDescriptionRepository.GetRewardDescription(rewardDescriptionID);
            var reward = new PlayerReward(
                playerID,
                rewardDescriptionID,
                rw.Name,
                rw.Description,
                rewardDate: DateTime.Now);
            await repository.CreateReward(reward);
        }
    }
}
