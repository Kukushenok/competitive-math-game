using ClientUsage.Client;
using CompetitiveBackend.BackendUsage.Objects;
using CompetitiveBackend.BackendUsage.UseCases;

namespace ClientUsage.UseCases
{
    internal sealed class CompetitionWatchUseCase : ICompetitionWatchUseCase
    {
        private readonly IHttpClient client;
        public CompetitionWatchUseCase(IHttpClient client)
        {
            this.client = client ?? throw new ArgumentNullException(nameof(client));
        }

        public Task<IEnumerable<CompetitionDTO>> GetActiveCompetitions()
        {
            return client.Get<IEnumerable<CompetitionDTO>>("/api/v1/competitions?filter=active");
        }

        public Task<IEnumerable<CompetitionDTO>> GetAllCompetitions(DataLimiterDTO limiter)
        {
            return client.Get<IEnumerable<CompetitionDTO>>($"/api/v1/competitions?page={limiter.Page}&count={limiter.Count}");
        }

        public Task<CompetitionDTO> GetCompetition(int competitionID)
        {
            return client.Get<CompetitionDTO>($"/api/v1/competitions/{competitionID}");
        }

        public Task<LargeDataDTO> GetCompetitionLevel(int competitionID, string? platform = null, int? maxVersion = null)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<CompetitionRewardDTO>> GetRewardsFor(int competitionID)
        {
            return client.Get<IEnumerable<CompetitionRewardDTO>>($"/api/v1/competitions/{competitionID}/rewards");
        }
    }
}
