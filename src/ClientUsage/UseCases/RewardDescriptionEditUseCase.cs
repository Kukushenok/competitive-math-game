using ClientUsage.Client;
using ClientUsage.Objects;
using CompetitiveBackend.BackendUsage.Objects;
using CompetitiveBackend.BackendUsage.UseCases;

namespace ClientUsage.UseCases
{
    internal sealed class RewardDescriptionEditUseCase : AuthableUseCaseBase<IRewardDescriptionEditUseCase>, IRewardDescriptionEditUseCase
    {
        public RewardDescriptionEditUseCase(IHttpClient client)
            : base(client)
        {
        }

        public override Task<IRewardDescriptionEditUseCase> Auth(string token)
        {
            IHttpClient authClient = CreateAuthClient(token);
            IRewardDescriptionEditUseCase impl = new RewardDescriptionEditUseCase(authClient);
            return Task.FromResult(impl);
        }

        public Task CreateRewardDescription(RewardDescriptionDTO reward)
        {
            return client.PostNoContent("/api/v1/reward_descriptions", reward);
        }

        public Task UpdateRewardDescription(RewardDescriptionDTO reward)
        {
            return client.PutNoContent($"/api/v1/reward_descriptions/{reward.ID}", reward);
        }

        public Task SetRewardIcon(int id, LargeDataDTO rewardIcon)
        {
            using var ms = new MemoryStream(rewardIcon.Data ?? []);
            return client.PostMultipartNoContent($"/api/v1/reward_descriptions/{id}/image", ms, "file", "file.bin", "application/octet-stream");
        }
    }
}
