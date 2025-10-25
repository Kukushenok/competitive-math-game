using System.Threading.Tasks;
using CompetitiveBackend.BackendUsage.Objects;

namespace CompetitiveBackend.BackendUsage.UseCases
{
    public interface ICompetitionRewardEditUseCase : IAuthableUseCase<ICompetitionRewardEditUseCase>
    {
        Task CreateCompetitionReward(CreateCompetitionRewardDTO reward);
        Task UpdateCompetitionReward(UpdateCompetitionRewardDTO reward);
        Task RemoveCompetitionReward(int compRewardID);
    }
}
