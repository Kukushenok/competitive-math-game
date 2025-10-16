using ClientUsage.Client;
using CompetitiveBackend.BackendUsage.UseCases;
using CompetitiveBackend.BackendUsage.Objects;

namespace ClientUsage.UseCases
{
    internal class CompetitionWatchUseCase : ICompetitionWatchUseCase
    {
        private readonly IHttpClient _client;
        public CompetitionWatchUseCase(IHttpClient client) => _client = client ?? throw new ArgumentNullException(nameof(client));

        public Task<IEnumerable<CompetitionDTO>> GetActiveCompetitions()
            => _client.Get<IEnumerable<CompetitionDTO>>("/api/v1/competitions?filter=active");

        public Task<IEnumerable<CompetitionDTO>> GetAllCompetitions(DataLimiterDTO limiter)
            => _client.Get<IEnumerable<CompetitionDTO>>($"/api/v1/competitions?page={limiter.Page}&count={limiter.Count}");

        public Task<CompetitionDTO> GetCompetition(int competitionID)
            => _client.Get<CompetitionDTO>($"/api/v1/competitions/{competitionID}");

        public Task<LargeDataDTO> GetCompetitionLevel(int competitionID, string? Platform = null, int? maxVersion = null)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<CompetitionRewardDTO>> GetRewardsFor(int competitionID)
            => _client.Get<IEnumerable<CompetitionRewardDTO>>($"/api/v1/competitions/{competitionID}/rewards");
    }
}
