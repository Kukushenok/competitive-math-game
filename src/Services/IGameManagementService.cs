using CompetitiveBackend.Core.Objects;
using CompetitiveBackend.Core.Objects.Riddles;

namespace CompetitiveBackend.Services
{
    public interface IGameManagementService
    {
        public Task<IEnumerable<RiddleInfo>> GetRiddles(int competitionID, DataLimiter limiter);
        public Task CreateRiddle(RiddleInfo riddle);
        public Task UpdateRiddle(RiddleInfo riddle);
        public Task DeleteRiddle(int riddleID);
        public Task<RiddleGameSettings> GetSettings(int competitionID);
        public Task UpdateSettings(int competitionID, RiddleGameSettings settings);
    }
}
