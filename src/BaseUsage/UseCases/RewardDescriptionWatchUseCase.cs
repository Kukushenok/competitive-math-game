using System.Text;
using CompetitiveBackend.BackendUsage.Objects;
using CompetitiveBackend.BackendUsage.UseCases;
using CompetitiveBackend.BaseUsage.Converters;
using CompetitiveBackend.Services;

namespace CompetitiveBackend.BaseUsage.UseCases
{
    public class RewardDescriptionWatchUseCase : IRewardDescriptionWatchUseCase
    {
        private readonly IRewardDescriptionService rewardDescriptionService;
        public RewardDescriptionWatchUseCase(IRewardDescriptionService rewardDescriptionService)
        {
            this.rewardDescriptionService = rewardDescriptionService;
        }

        public async Task<IEnumerable<RewardDescriptionDTO>> GetAllRewardDescriptions(DataLimiterDTO dataLimiter)
        {
            return DTOConverter.Convert(
                await rewardDescriptionService.GetAllRewardDescriptions(dataLimiter.Convert()),
                x => x.Convert());
        }

        public async Task<RewardDescriptionDTO> GetRewardDescription(int id)
        {
            return (await rewardDescriptionService.GetRewardDescription(id)).Convert();
        }

        public async Task<LargeDataDTO> GetRewardIcon(int id)
        {
            return (await rewardDescriptionService.GetRewardIcon(id)).Convert();
        }
    }
}
