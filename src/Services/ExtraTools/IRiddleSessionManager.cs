using CompetitiveBackend.Core.Objects.Riddles;

namespace CompetitiveBackend.Services.ExtraTools
{
    public interface IRiddleSessionManager
    {
        public Task<RiddleSession> CreateSession(RiddleGameInfo gameInfo);
        public Task<RiddleSession> RetrieveSession(CompetitionParticipationRequest session);
    }
}
