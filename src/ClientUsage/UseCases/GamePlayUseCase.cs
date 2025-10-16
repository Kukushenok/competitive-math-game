// File: IHttpClient.cs
using ClientUsage.Client;
using CompetitiveBackend.BackendUsage.UseCases;

// File: HttpClientExtensions.cs
using CompetitiveBackend.BackendUsage.Objects;
using ClientUsage.Objects;

namespace ClientUsage.UseCases
{
    internal class GamePlayUseCase : AuthableUseCaseBase<IGamePlayUseCase>, IGamePlayUseCase
    {
        public GamePlayUseCase(IHttpClient client) : base(client) { }

        public override Task<IGamePlayUseCase> Auth(string token)
        {
            var authClient = CreateAuthClient(token);
            IGamePlayUseCase impl = new GamePlayUseCase(authClient);
            return Task.FromResult(impl);
        }

        public Task<CompetitionParticipationTaskDTO> DoPlay(int competitionID)
            => _client.Get<CompetitionParticipationTaskDTO>($"/api/v1/competitions/{competitionID}/game_session");

        public Task<ParticipationFeedbackDTO> DoSubmit(CompetitionParticipationRequestDTO request)
            => _client.Post<ParticipationFeedbackDTO>($"/api/v1/competitions/2/game_session", request);
    }
}
