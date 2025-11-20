using System.Text;
using CompetitiveBackend.BackendUsage.Objects;
using CompetitiveBackend.BackendUsage.UseCases;
using CompetitiveBackend.BaseUsage.Converters;
using CompetitiveBackend.Services;

namespace CompetitiveBackend.BaseUsage.UseCases
{
    public class PlayerRewardUseCase : BaseAuthableUseCase<PlayerRewardUseCase>, IPlayerRewardUseCase
    {
        private readonly IPlayerRewardService service;

        public PlayerRewardUseCase(IAuthService authService, IPlayerRewardService service)
            : base(authService)
        {
            this.service = service;
        }

        public async Task DeleteReward(int playerRewardID)
        {
            AdminAuthCheck(out _);
            await service.DeleteReward(playerRewardID);
        }

        public async Task<IEnumerable<PlayerRewardDTO>> GetAllMineRewards(DataLimiterDTO limiter)
        {
            PlayerAuthCheck(out int id);
            return from q in await service.GetAllRewardsOf(id, limiter.Convert()) select q.Convert();
        }

        public async Task<IEnumerable<PlayerRewardDTO>> GetAllRewardsOf(int playerID, DataLimiterDTO limiter)
        {
            AdminAuthCheck(out _);
            return from q in await service.GetAllRewardsOf(playerID, limiter.Convert()) select q.Convert();
        }

        public async Task GrantRewardToPlayer(int playerID, int rewardDescriptionID)
        {
            AdminAuthCheck(out _);
            await service.GrantRewardToPlayer(playerID, rewardDescriptionID);
        }

        async Task<IPlayerRewardUseCase> IAuthableUseCase<IPlayerRewardUseCase>.Auth(string token)
        {
            return await Auth(token);
        }
    }
}
