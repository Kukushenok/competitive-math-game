using CompetitiveBackend.BackendUsage.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompetitiveBackend.BackendUsage.UseCases
{
    public interface IRewardDescriptionWatchUseCase
    {
        public Task<LargeDataDTO> GetRewardIcon(int id);
        public Task<LargeDataDTO> GetRewardGameAsset(int id);
        public Task<RewardDescriptionDTO> GetRewardDescription(int id);
        public Task<IEnumerable<RewardDescriptionDTO>> GetAllRewardDescriptions(DataLimiterDTO dataLimiter);
    }
}
