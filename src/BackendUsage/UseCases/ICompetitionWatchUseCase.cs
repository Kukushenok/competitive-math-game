using CompetitiveBackend.BackendUsage.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompetitiveBackend.BackendUsage.UseCases
{
    public interface ICompetitionWatchUseCase
    {
        public Task<IEnumerable<CompetitionDTO>> GetActiveCompetitions();
        public Task<IEnumerable<CompetitionDTO>> GetAllCompetitions(DataLimiterDTO limiter);
        public Task<CompetitionDTO> GetCompetition(int competitionID);
        public Task<LargeDataDTO> GetCompetitionLevel(int competitionID);
    }
}
