
using ClientUsage.Client;
using CompetitiveBackend.BackendUsage.UseCases;
using CompetitiveBackend.BackendUsage.Objects;
using ClientUsage.Objects;

namespace ClientUsage.UseCases
{
    internal class RewardDescriptionEditUseCase : AuthableUseCaseBase<IRewardDescriptionEditUseCase>, IRewardDescriptionEditUseCase
    {
        public RewardDescriptionEditUseCase(IHttpClient client) : base(client) { }

        public override Task<IRewardDescriptionEditUseCase> Auth(string token)
        {
            var authClient = CreateAuthClient(token);
            IRewardDescriptionEditUseCase impl = new RewardDescriptionEditUseCase(authClient);
            return Task.FromResult(impl);
        }

        public Task CreateRewardDescription(RewardDescriptionDTO reward)
            => _client.PostNoContent("/api/v1/reward_descriptions", reward);

        public Task UpdateRewardDescription(RewardDescriptionDTO reward)
            => _client.PutNoContent($"/api/v1/reward_descriptions/{reward.ID}", reward);

        public Task SetRewardIcon(int id, LargeDataDTO rewardIcon)
        {
            using var ms = new MemoryStream(rewardIcon.Data ?? Array.Empty<byte>());
            return _client.PostMultipartNoContent($"/api/v1/reward_descriptions/{id}/image", ms, "file", "file.bin", "application/octet-stream");
        }
    }
}
