// File: IHttpClient.cs
using ClientUsage.Client;
using ClientUsage.Objects;

// File: HttpClientExtensions.cs
using CompetitiveBackend.BackendUsage.Objects;
using CompetitiveBackend.BackendUsage.UseCases;

namespace ClientUsage.UseCases
{
    internal sealed class GamePlayUseCase : AuthableUseCaseBase<IGamePlayUseCase>, IGamePlayUseCase
    {
        public GamePlayUseCase(IHttpClient client)
            : base(client)
        {
        }

        public override Task<IGamePlayUseCase> Auth(string token)
        {
            IHttpClient authClient = CreateAuthClient(token);
            IGamePlayUseCase impl = new GamePlayUseCase(authClient);
            return Task.FromResult(impl);
        }

        public Task<CompetitionParticipationTaskDTO> DoPlay(int competitionID)
        {
            return client.Get<CompetitionParticipationTaskDTO>($"/api/v1/competitions/{competitionID}/game_session");
        }

        public Task<ParticipationFeedbackDTO> DoSubmit(CompetitionParticipationRequestDTO request)
        {
            return client.Post<ParticipationFeedbackDTO>($"/api/v1/competitions/2/game_session", request);
        }
    }
}
