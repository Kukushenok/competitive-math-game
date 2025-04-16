using CompetitiveBackend.BackendUsage.Objects;
using System.Threading.Tasks;

namespace CompetitiveBackend.BackendUsage.UseCases
{
    public interface IRewardDescriptionEditUseCase : IAuthableUseCase<IRewardDescriptionEditUseCase>
    {
        public Task CreateRewardDescription(RewardDescriptionDTO reward);
        public Task UpdateRewardDescription(RewardDescriptionDTO reward);
        public Task SetRewardIcon(int id, LargeDataDTO rewardIcon);
        public Task SetRewardGameAsset(int id, LargeDataDTO gameAsset);
    }
}
