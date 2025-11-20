using System.Text;
using CompetitiveBackend.BackendUsage.Objects;
using CompetitiveBackend.BackendUsage.UseCases;
using CompetitiveBackend.BaseUsage.Converters;
using CompetitiveBackend.Core.Objects;
using CompetitiveBackend.Services;

namespace CompetitiveBackend.BaseUsage.UseCases
{
    public class RewardDescriptionEditUseCase : BaseAuthableUseCase<RewardDescriptionEditUseCase>, IRewardDescriptionEditUseCase
    {
        private readonly IRewardDescriptionService rewardDescriptionService;
        public RewardDescriptionEditUseCase(IAuthService authService, IRewardDescriptionService rewardDescriptionService)
            : base(authService)
        {
            this.rewardDescriptionService = rewardDescriptionService;
        }

        public async Task CreateRewardDescription(RewardDescriptionDTO reward)
        {
            AdminAuthCheck(out _);
            await rewardDescriptionService.CreateRewardDescription(new RewardDescription(reward.Name, reward.Description));
        }

        public async Task SetRewardIcon(int id, LargeDataDTO rewardIcon)
        {
            AdminAuthCheck(out _);
            await rewardDescriptionService.SetRewardIcon(id, rewardIcon.Convert());
        }

        public async Task UpdateRewardDescription(RewardDescriptionDTO reward)
        {
            AdminAuthCheck(out _);
            await rewardDescriptionService.UpdateRewardDescription(reward.ID!.Value, reward.Name, reward.Description);
        }

        async Task<IRewardDescriptionEditUseCase> IAuthableUseCase<IRewardDescriptionEditUseCase>.Auth(string token)
        {
            return await Auth(token);
        }
    }
}
