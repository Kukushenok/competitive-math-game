using CompetitiveBackend.Core.Objects.Riddles;

namespace CompetitiveBackend.Services.ExtraTools
{
    public interface IRiddleSessionManager
    {
        public Task<RiddleSession> CreateSession(RiddleGameInfo gameInfo);
        public Task<RiddleSession> RetrieveSession(string sessionID);
        public Task<bool> DeleteSession(string sessionID);
    }
}
