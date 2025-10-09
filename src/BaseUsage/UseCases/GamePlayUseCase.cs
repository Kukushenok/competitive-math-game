using CompetitiveBackend.BackendUsage.Objects;
using CompetitiveBackend.BackendUsage.UseCases;
using CompetitiveBackend.BaseUsage.Converters;
using CompetitiveBackend.Services;

namespace CompetitiveBackend.BaseUsage.UseCases
{
    public class GamePlayUseCase : BaseAuthableUseCase<GamePlayUseCase>, IGamePlayUseCase
    {
        private IGameProviderService providerService;
        public GamePlayUseCase(IAuthService authService, IGameProviderService providerService) : base(authService)
        {
            this.providerService = providerService;
        }

        public async Task<CompetitionParticipationTaskDTO> DoPlay(int competitionID)
        {
            PlayerAuthCheck(out int idx);
            var task = await providerService.DoPlay(idx, competitionID);
            return task.Convert();
        }

        public async Task<ParticipationFeedbackDTO> DoSubmit(CompetitionParticipationRequestDTO request)
        {
            PlayerAuthCheck(out int idx);
            request.PlayerID = idx;
            var task = await providerService.DoSubmit(request.Convert());
            return task.Convert();
        }

        async Task<IGamePlayUseCase> IAuthableUseCase<IGamePlayUseCase>.Auth(string token) => await Auth(token);
    }
}
