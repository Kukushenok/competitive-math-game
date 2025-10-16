// File: IHttpClient.cs
using ClientUsage.Client;
using CompetitiveBackend.BackendUsage.UseCases;

// File: HttpClientExtensions.cs
using CompetitiveBackend.BackendUsage.Objects;



// File: PlayerParticipationWatchUseCase.cs

namespace ClientUsage.UseCases
{
    internal class PlayerParticipationWatchUseCase : IPlayerParticipationWatchUseCase
    {
        private readonly IHttpClient _client;
        public PlayerParticipationWatchUseCase(IHttpClient client) => _client = client ?? throw new ArgumentNullException(nameof(client));

        public Task<PlayerParticipationDTO> GetParticipation(int competition, int accountID)
            => _client.Get<PlayerParticipationDTO>($"/api/v1/competitions/{competition}/participations/{accountID}");

        public Task<IEnumerable<PlayerParticipationDTO>> GetLeaderboard(int competition, DataLimiterDTO limiter)
            => _client.Get<IEnumerable<PlayerParticipationDTO>>($"/api/v1/competitions/{competition}/participations?page={limiter.Page}&count={limiter.Count}");
    }
}
