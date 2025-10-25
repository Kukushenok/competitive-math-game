using ClientUsage.Client;
using CompetitiveBackend.BackendUsage.Objects;
using CompetitiveBackend.BackendUsage.UseCases;
namespace ClientUsage.UseCases
{
    internal sealed class RewardDescriptionWatchUseCase : IRewardDescriptionWatchUseCase
    {
        private readonly IHttpClient client;
        public RewardDescriptionWatchUseCase(IHttpClient client)
        {
            this.client = client ?? throw new ArgumentNullException(nameof(client));
        }

        public async Task<LargeDataDTO> GetRewardIcon(int id)
        {
            LargeDataDTO dto = await client.Get<LargeDataDTO>($"/api/v1/reward_descriptions/{id}/image");
            return dto;
        }

        public Task<RewardDescriptionDTO> GetRewardDescription(int id)
        {
            return client.Get<RewardDescriptionDTO>($"/api/v1/reward_descriptions/{id}");
        }

        public Task<IEnumerable<RewardDescriptionDTO>> GetAllRewardDescriptions(DataLimiterDTO dataLimiter)
        {
            return client.Get<IEnumerable<RewardDescriptionDTO>>($"/api/v1/reward_descriptions?page={dataLimiter.Page}&count={dataLimiter.Count}");
        }
    }
}
