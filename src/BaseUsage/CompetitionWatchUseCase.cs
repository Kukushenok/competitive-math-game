using CompetitiveBackend.BackendUsage.Objects;
using CompetitiveBackend.BackendUsage.UseCases;
using CompetitiveBackend.Core.Objects;
using CompetitiveBackend.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompetitiveBackend.BaseUsage
{
    public class CompetitionWatchUseCase : ICompetitionWatchUseCase
    {
        private ICompetitionService _service;
        public CompetitionWatchUseCase(ICompetitionService service)
        {
            _service = service;
        }

        public async Task<IEnumerable<CompetitionDTO>> GetActiveCompetitions()
        {
            var Q = await _service.GetActiveCompetitions();
            return UseCaseDTOConverter.Convert(Q, x => x.Convert());
        }

        public async Task<IEnumerable<CompetitionDTO>> GetAllCompetitions(DataLimiterDTO limiter)
        {
            var Q = await _service.GetAllCompetitions(limiter.Convert());
            return UseCaseDTOConverter.Convert(Q, x => x.Convert());
        }

        public async Task<CompetitionDTO> GetCompetition(int competitionID)
        {
            return (await _service.GetCompetition(competitionID)).Convert();
        }

        public async Task<LargeDataDTO> GetCompetitionLevel(int competitionID)
        {
            return (await _service.GetCompetitionLevel(competitionID)).Convert();
        }
    }
}
