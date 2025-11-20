using CompetitiveBackend.Core.Objects;
using CompetitiveBackend.Core.Objects.Riddles;

namespace CompetitiveBackend.Services
{
    public interface IGameManagementService
    {
        Task<IEnumerable<RiddleInfo>> GetRiddles(int competitionID, DataLimiter limiter);
        Task CreateRiddle(RiddleInfo riddle);
        Task UpdateRiddle(RiddleInfo riddle);
        Task DeleteRiddle(int riddleID);
        Task<RiddleGameSettings> GetSettings(int competitionID);
        Task UpdateSettings(int competitionID, RiddleGameSettings settings);
    }
}
