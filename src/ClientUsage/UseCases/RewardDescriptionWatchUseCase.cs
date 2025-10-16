using ClientUsage.Client;
using CompetitiveBackend.BackendUsage.UseCases;
using CompetitiveBackend.BackendUsage.Objects;
namespace ClientUsage.UseCases
{
    internal class RewardDescriptionWatchUseCase : IRewardDescriptionWatchUseCase
    {
        private readonly IHttpClient _client;
        public RewardDescriptionWatchUseCase(IHttpClient client) => _client = client ?? throw new ArgumentNullException(nameof(client));

        public async Task<LargeDataDTO> GetRewardIcon(int id)
        {
            var dto = await _client.Get<LargeDataDTO>($"/api/v1/reward_descriptions/{id}/image");
            return dto;
        }

        public Task<RewardDescriptionDTO> GetRewardDescription(int id)
            => _client.Get<RewardDescriptionDTO>($"/api/v1/reward_descriptions/{id}");

        public Task<IEnumerable<RewardDescriptionDTO>> GetAllRewardDescriptions(DataLimiterDTO dataLimiter)
            => _client.Get<IEnumerable<RewardDescriptionDTO>>($"/api/v1/reward_descriptions?page={dataLimiter.Page}&count={dataLimiter.Count}");
    }
}

