using CompetitiveBackend.BackendUsage.Objects;
using CompetitiveBackend.BackendUsage.UseCases;
using CompetitiveBackend.BaseUsage.Converters;
using CompetitiveBackend.Services;
using System.Text;

namespace CompetitiveBackend.BaseUsage.UseCases
{
    public class RewardDescriptionWatchUseCase : IRewardDescriptionWatchUseCase
    {
        private IRewardDescriptionService _rewardDescriptionService;
        public RewardDescriptionWatchUseCase(IRewardDescriptionService rewardDescriptionService)
        {
            _rewardDescriptionService = rewardDescriptionService;
        }

        public async Task<IEnumerable<RewardDescriptionDTO>> GetAllRewardDescriptions(DataLimiterDTO dataLimiter)
        {
            return DTOConverter.Convert(await _rewardDescriptionService.GetAllRewardDescriptions(dataLimiter.Convert()), 
                x => x.Convert());
        }

        public async Task<RewardDescriptionDTO> GetRewardDescription(int id)
        {
            return (await _rewardDescriptionService.GetRewardDescription(id)).Convert();
        }

        public async Task<LargeDataDTO> GetRewardIcon(int id)
        {
            return (await _rewardDescriptionService.GetRewardIcon(id)).Convert();
        }
    }
}
