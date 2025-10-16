// File: IHttpClient.cs
using ClientUsage.Client;
using CompetitiveBackend.BackendUsage.UseCases;

// File: HttpClientExtensions.cs
using CompetitiveBackend.BackendUsage.Objects;
using ClientUsage.Objects;

namespace ClientUsage.UseCases
{
    internal class PlayerParticipationUseCase : AuthableUseCaseBase<IPlayerParticipationUseCase>, IPlayerParticipationUseCase
    {
        public PlayerParticipationUseCase(IHttpClient client) : base(client) { }

        public override Task<IPlayerParticipationUseCase> Auth(string token)
        {
            var authClient = CreateAuthClient(token);
            IPlayerParticipationUseCase impl = new PlayerParticipationUseCase(authClient);
            return Task.FromResult(impl);
        }

        public Task DeleteParticipation(int competition, int accountID)
            => _client.DeleteNoContent($"/api/v1/competitions/{competition}/participations/{accountID}");

        public Task<IEnumerable<PlayerParticipationDTO>> GetMyParticipations(DataLimiterDTO limiter)
            => _client.Get<IEnumerable<PlayerParticipationDTO>>($"/api/v1/players/me/participations?page={limiter.Page}&count={limiter.Count}");

        public Task SubmitScoreTo(int competition, int score)
        {
            throw new NotImplementedException();
        }
    }
}

