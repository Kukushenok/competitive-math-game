using System.Collections.Generic;
using System.Threading.Tasks;
using CompetitiveBackend.BackendUsage.Objects;

namespace CompetitiveBackend.BackendUsage.UseCases
{
    public interface IRewardDescriptionWatchUseCase
    {
        Task<LargeDataDTO> GetRewardIcon(int id);
        Task<RewardDescriptionDTO> GetRewardDescription(int id);
        Task<IEnumerable<RewardDescriptionDTO>> GetAllRewardDescriptions(DataLimiterDTO dataLimiter);
    }
}
