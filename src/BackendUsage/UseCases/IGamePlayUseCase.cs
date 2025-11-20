using System.Threading.Tasks;
using CompetitiveBackend.BackendUsage.Objects;

namespace CompetitiveBackend.BackendUsage.UseCases
{
    public interface IGamePlayUseCase : IAuthableUseCase<IGamePlayUseCase>
    {
        Task<CompetitionParticipationTaskDTO> DoPlay(int competitionID);
        Task<ParticipationFeedbackDTO> DoSubmit(CompetitionParticipationRequestDTO request);
    }
}
