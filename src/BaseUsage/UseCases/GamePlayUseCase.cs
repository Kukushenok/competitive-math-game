using CompetitiveBackend.BackendUsage.Objects;
using CompetitiveBackend.BackendUsage.UseCases;
using CompetitiveBackend.BaseUsage.Converters;
using CompetitiveBackend.Services;

namespace CompetitiveBackend.BaseUsage.UseCases
{
    public class GamePlayUseCase : BaseAuthableUseCase<GamePlayUseCase>, IGamePlayUseCase
    {
        private readonly IGameProviderService providerService;
        public GamePlayUseCase(IAuthService authService, IGameProviderService providerService)
            : base(authService)
        {
            this.providerService = providerService;
        }

        public async Task<CompetitionParticipationTaskDTO> DoPlay(int competitionID)
        {
            PlayerAuthCheck(out int idx);
            Core.Objects.Riddles.CompetitionParticipationTask task = await providerService.DoPlay(idx, competitionID);
            return task.Convert();
        }

        public async Task<ParticipationFeedbackDTO> DoSubmit(CompetitionParticipationRequestDTO request)
        {
            PlayerAuthCheck(out int idx);
            Core.Objects.Riddles.ParticipationFeedback task = await providerService.DoSubmit(request.Convert(idx));
            return task.Convert();
        }

        async Task<IGamePlayUseCase> IAuthableUseCase<IGamePlayUseCase>.Auth(string token)
        {
            return await Auth(token);
        }
    }
}
