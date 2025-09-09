using CompetitiveBackend.BackendUsage.Objects;
using System.Threading.Tasks;

namespace CompetitiveBackend.BackendUsage.UseCases
{
    public interface ICompetitionEditUseCase : IAuthableUseCase<ICompetitionEditUseCase>
    {
        public Task CreateCompetition(CompetitionDTO competition);
        public Task UpdateCompetition(CompetitionUpdateRequestDTO competition);
    }
}
