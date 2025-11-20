using System.Threading.Tasks;
using CompetitiveBackend.BackendUsage.Objects;

namespace CompetitiveBackend.BackendUsage.UseCases
{
    public interface IRewardDescriptionEditUseCase : IAuthableUseCase<IRewardDescriptionEditUseCase>
    {
        Task CreateRewardDescription(RewardDescriptionDTO reward);
        Task UpdateRewardDescription(RewardDescriptionDTO reward);
        Task SetRewardIcon(int id, LargeDataDTO rewardIcon);
    }
}
