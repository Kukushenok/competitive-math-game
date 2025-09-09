using CompetitiveBackend.BackendUsage.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompetitiveBackend.BackendUsage.UseCases
{
    public interface IPlayerRewardUseCase: IAuthableUseCase<IPlayerRewardUseCase>
    {
        public Task GrantRewardToPlayer(int playerID, int rewardDescriptionID);

        public Task DeleteReward(int playerRewardID);
        public Task<IEnumerable<PlayerRewardDTO>> GetAllRewardsOf(int playerID, DataLimiterDTO limiter);
        public Task<IEnumerable<PlayerRewardDTO>> GetAllMineRewards(DataLimiterDTO limiter);
    }
}
