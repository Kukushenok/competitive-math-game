using CompetitiveBackend.BackendUsage.Objects;
using CompetitiveBackend.BackendUsage.UseCases;
using CompetitiveBackend.BaseUsage.Converters;
using CompetitiveBackend.Core.Objects;
using CompetitiveBackend.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompetitiveBackend.BaseUsage
{
    public class RewardDescriptionEditUseCase : BaseAuthableUseCase<RewardDescriptionEditUseCase>, IRewardDescriptionEditUseCase
    {
        private IRewardDescriptionService _rewardDescriptionService;
        public RewardDescriptionEditUseCase(IAuthService authService, IRewardDescriptionService rewardDescriptionService) : base(authService)
        {
            _rewardDescriptionService = rewardDescriptionService;
        }

        public async Task CreateRewardDescription(RewardDescriptionDTO reward)
        {
            AdminAuthCheck(out _);
            await _rewardDescriptionService.CreateRewardDescription(new RewardDescription(reward.Name, reward.Description));
        }

        public async Task SetRewardGameAsset(int id, LargeDataDTO gameAsset)
        {
            AdminAuthCheck(out _);
            await _rewardDescriptionService.SetRewardGameAsset(id, gameAsset.Convert());
        }

        public async Task SetRewardIcon(int id, LargeDataDTO rewardIcon)
        {
            AdminAuthCheck(out _);
            await _rewardDescriptionService.SetRewardIcon(id, rewardIcon.Convert());
        }

        public async Task UpdateRewardDescription(RewardDescriptionDTO reward)
        {
            AdminAuthCheck(out _);
            await _rewardDescriptionService.UpdateRewardDescription(reward.ID!.Value, reward.Name, reward.Description);
        }

        async Task<IRewardDescriptionEditUseCase> IAuthableUseCase<IRewardDescriptionEditUseCase>.Auth(string token) => await Auth(token);
    }
}
