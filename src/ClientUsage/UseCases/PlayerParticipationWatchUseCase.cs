// File: IHttpClient.cs
using ClientUsage.Client;

// File: HttpClientExtensions.cs
using CompetitiveBackend.BackendUsage.Objects;
using CompetitiveBackend.BackendUsage.UseCases;

// File: PlayerParticipationWatchUseCase.cs
namespace ClientUsage.UseCases
{
    internal sealed class PlayerParticipationWatchUseCase : IPlayerParticipationWatchUseCase
    {
        private readonly IHttpClient client;
        public PlayerParticipationWatchUseCase(IHttpClient client)
        {
            this.client = client ?? throw new ArgumentNullException(nameof(client));
        }

        public Task<PlayerParticipationDTO> GetParticipation(int competition, int accountID)
        {
            return client.Get<PlayerParticipationDTO>($"/api/v1/competitions/{competition}/participations/{accountID}");
        }

        public Task<IEnumerable<PlayerParticipationDTO>> GetLeaderboard(int competition, DataLimiterDTO limiter)
        {
            return client.Get<IEnumerable<PlayerParticipationDTO>>($"/api/v1/competitions/{competition}/participations?page={limiter.Page}&count={limiter.Count}");
        }
    }
}
