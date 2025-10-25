using CompetitiveBackend.Core.Objects.Riddles;

namespace CompetitiveBackend.Services
{
    public interface IGameProviderService
    {
        Task<CompetitionParticipationTask> DoPlay(int accountID, int competitionID);
        Task<ParticipationFeedback> DoSubmit(CompetitionParticipationRequest request);
    }
}
