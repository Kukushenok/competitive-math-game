using ClientUsage.Client;
using ClientUsage.Objects;
using CompetitiveBackend.BackendUsage.Objects;
using CompetitiveBackend.BackendUsage.UseCases;

namespace ClientUsage.UseCases
{
    internal sealed class PlayerRewardUseCase : AuthableUseCaseBase<IPlayerRewardUseCase>, IPlayerRewardUseCase
    {
        public PlayerRewardUseCase(IHttpClient client)
            : base(client)
        {
        }

        public override Task<IPlayerRewardUseCase> Auth(string token)
        {
            IHttpClient authClient = CreateAuthClient(token);
            IPlayerRewardUseCase impl = new PlayerRewardUseCase(authClient);
            return Task.FromResult(impl);
        }

        public Task GrantRewardToPlayer(int playerID, int rewardDescriptionID)
        {
            return client.PutNoContent($"/api/v1/players/{playerID}/rewards?rewardDescriptionID={rewardDescriptionID}");
        }

        public Task DeleteReward(int playerRewardID)
        {
            return client.DeleteNoContent($"/api/v1/players/0/rewards/{playerRewardID}"); // API expects playerID in path; missing here
        }

        public Task<IEnumerable<PlayerRewardDTO>> GetAllRewardsOf(int playerID, DataLimiterDTO limiter)
        {
            return client.Get<IEnumerable<PlayerRewardDTO>>($"/api/v1/players/{playerID}/rewards?page={limiter.Page}&count={limiter.Count}");
        }

        public Task<IEnumerable<PlayerRewardDTO>> GetAllMineRewards(DataLimiterDTO limiter)
        {
            return client.Get<IEnumerable<PlayerRewardDTO>>($"/api/v1/players/me/rewards?page={limiter.Page}&count={limiter.Count}");
        }
    }
}
