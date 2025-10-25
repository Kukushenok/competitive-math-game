using CompetitiveBackend.Core.Objects.Riddles;

namespace CompetitiveBackend.Services.ExtraTools
{
    public interface IRiddleSessionManager
    {
        Task<RiddleSession> CreateSession(RiddleGameInfo gameInfo);
        Task<RiddleSession> RetrieveSession(string sessionID);
        Task<bool> DeleteSession(string sessionID);
    }
}
