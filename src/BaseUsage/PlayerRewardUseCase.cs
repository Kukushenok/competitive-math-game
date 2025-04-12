using CompetitiveBackend.BackendUsage.Objects;
using CompetitiveBackend.BackendUsage.UseCases;
using CompetitiveBackend.BaseUsage.Converters;
using CompetitiveBackend.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompetitiveBackend.BaseUsage
{
    public class PlayerRewardUseCase : BaseAuthableUseCase<PlayerRewardUseCase>, IPlayerRewardUseCase
    {
        private IPlayerRewardService _service;

        public PlayerRewardUseCase(IAuthService authService, IPlayerRewardService service) : base(authService)
        {
            _service = service;
        }

        public async Task DeleteReward(int playerRewardID)
        {
            AdminAuthCheck(out _);
            await _service.DeleteReward(playerRewardID);
        }
        public async Task<IEnumerable<PlayerRewardDTO>> GetAllMineRewards(DataLimiterDTO limiter)
        {
            PlayerAuthCheck(out int id);
            return from q in await _service.GetAllRewardsOf(id, limiter.Convert()) select q.Convert();
        }

        public async Task<IEnumerable<PlayerRewardDTO>> GetAllRewardsOf(int playerID, DataLimiterDTO limiter)
        {
            AdminAuthCheck(out _);
            return from q in await _service.GetAllRewardsOf(playerID, limiter.Convert()) select q.Convert();
        }

        public async Task GrantRewardToPlayer(int playerID, int rewardDescriptionID)
        {
            AdminAuthCheck(out _);
            await _service.GrantRewardToPlayer(playerID, rewardDescriptionID);
        }

        async Task<IPlayerRewardUseCase> IAuthableUseCase<IPlayerRewardUseCase>.Auth(string token) => await Auth(token);
    }
}
