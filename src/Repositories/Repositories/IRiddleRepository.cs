using CompetitiveBackend.Core.Objects;
using CompetitiveBackend.Core.Objects.Riddles;
namespace CompetitiveBackend.Repositories
{
    public interface IRiddleRepository
    {
        public Task<IEnumerable<RiddleInfo>> GetRiddles(int competitionID, DataLimiter limiter);
        public Task CreateRiddle(RiddleInfo riddle);
        public Task UpdateRiddle(RiddleInfo riddle);
        public Task DeleteRiddle(int riddleID);
    }
}
