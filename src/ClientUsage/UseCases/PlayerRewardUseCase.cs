
using ClientUsage.Client;
using CompetitiveBackend.BackendUsage.UseCases;
using CompetitiveBackend.BackendUsage.Objects;
using ClientUsage.Objects;

namespace ClientUsage.UseCases
{
    internal class PlayerRewardUseCase : AuthableUseCaseBase<IPlayerRewardUseCase>, IPlayerRewardUseCase
    {
        public PlayerRewardUseCase(IHttpClient client) : base(client) { }

        public override Task<IPlayerRewardUseCase> Auth(string token)
        {
            var authClient = CreateAuthClient(token);
            IPlayerRewardUseCase impl = new PlayerRewardUseCase(authClient);
            return Task.FromResult(impl);
        }

        public Task GrantRewardToPlayer(int playerID, int rewardDescriptionID)
            => _client.PutNoContent($"/api/v1/players/{playerID}/rewards?rewardDescriptionID={rewardDescriptionID}");

        public Task DeleteReward(int playerRewardID)
            => _client.DeleteNoContent($"/api/v1/players/0/rewards/{playerRewardID}"); // API expects playerID in path; missing here

        public Task<IEnumerable<PlayerRewardDTO>> GetAllRewardsOf(int playerID, DataLimiterDTO limiter)
            => _client.Get<IEnumerable<PlayerRewardDTO>>($"/api/v1/players/{playerID}/rewards?page={limiter.Page}&count={limiter.Count}");

        public Task<IEnumerable<PlayerRewardDTO>> GetAllMineRewards(DataLimiterDTO limiter)
            => _client.Get<IEnumerable<PlayerRewardDTO>>($"/api/v1/players/me/rewards?page={limiter.Page}&count={limiter.Count}");
    }
}

