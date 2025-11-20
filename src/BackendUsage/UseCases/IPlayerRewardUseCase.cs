using System.Collections.Generic;
using System.Threading.Tasks;
using CompetitiveBackend.BackendUsage.Objects;

namespace CompetitiveBackend.BackendUsage.UseCases
{
    public interface IPlayerRewardUseCase : IAuthableUseCase<IPlayerRewardUseCase>
    {
        Task GrantRewardToPlayer(int playerID, int rewardDescriptionID);

        Task DeleteReward(int playerRewardID);
        Task<IEnumerable<PlayerRewardDTO>> GetAllRewardsOf(int playerID, DataLimiterDTO limiter);
        Task<IEnumerable<PlayerRewardDTO>> GetAllMineRewards(DataLimiterDTO limiter);
    }
}
