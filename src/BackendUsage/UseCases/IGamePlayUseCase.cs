using CompetitiveBackend.BackendUsage.Objects;
using System.Threading.Tasks;

namespace CompetitiveBackend.BackendUsage.UseCases
{
    public interface IGamePlayUseCase: IAuthableUseCase<IGamePlayUseCase>
    {
        public Task<CompetitionParticipationTaskDTO> DoPlay(int competitionID);
        public Task<ParticipationFeedbackDTO> DoSubmit(CompetitionParticipationRequestDTO request);
    }
}
