using System.Threading.Tasks;
using CompetitiveBackend.BackendUsage.Objects;

namespace CompetitiveBackend.BackendUsage.UseCases
{
    public interface ICompetitionEditUseCase : IAuthableUseCase<ICompetitionEditUseCase>
    {
        Task CreateCompetition(CompetitionDTO competition);
        Task UpdateCompetition(CompetitionPatchRequestDTO competition);
    }
}
