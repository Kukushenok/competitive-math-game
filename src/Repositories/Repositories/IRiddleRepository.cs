using CompetitiveBackend.Core.Objects;
using CompetitiveBackend.Core.Objects.Riddles;
namespace CompetitiveBackend.Repositories
{
    public interface IRiddleRepository
    {
        Task<IEnumerable<RiddleInfo>> GetRiddles(int competitionID, DataLimiter limiter);
        Task CreateRiddle(RiddleInfo riddle);
        Task UpdateRiddle(RiddleInfo riddle);
        Task DeleteRiddle(int riddleID);
    }
}
