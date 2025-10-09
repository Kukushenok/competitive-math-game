using CompetitiveBackend.Core.Objects.Riddles;

namespace CompetitiveBackend.Services
{
    public interface IGameProviderService
    {
        public Task<CompetitionParticipationTask> DoPlay(int accountID, int competitionID);
        public Task<ParticipationFeedback> DoSubmit(CompetitionParticipationRequest request);
    }
}
