using System.Collections.Generic;
using System.Threading.Tasks;
using CompetitiveBackend.BackendUsage.Objects;

namespace CompetitiveBackend.BackendUsage.UseCases
{
    public interface ICompetitionWatchUseCase
    {
        Task<IEnumerable<CompetitionDTO>> GetActiveCompetitions();
        Task<IEnumerable<CompetitionDTO>> GetAllCompetitions(DataLimiterDTO limiter);
        Task<CompetitionDTO> GetCompetition(int competitionID);
        Task<LargeDataDTO> GetCompetitionLevel(int competitionID, string? platform = null, int? maxVersion = null);
        Task<IEnumerable<CompetitionRewardDTO>> GetRewardsFor(int competitionID);
    }
}
