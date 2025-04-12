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
    public class CompetitionWatchUseCase : ICompetitionWatchUseCase
    {
        private ICompetitionService _service;
        private ICompetitionRewardService _rewardService;
        public CompetitionWatchUseCase(ICompetitionService service, ICompetitionRewardService rewardService)
        {
            _service = service;
            _rewardService = rewardService;
        }

        public async Task<IEnumerable<CompetitionDTO>> GetActiveCompetitions()
        {
            return from n in (await _service.GetActiveCompetitions()) select n.Convert();
        }

        public async Task<IEnumerable<CompetitionDTO>> GetAllCompetitions(DataLimiterDTO limiter)
        {
            return from n in (await _service.GetAllCompetitions(limiter.Convert())) select n.Convert();
        }

        public async Task<CompetitionDTO> GetCompetition(int competitionID)
        {
            return (await _service.GetCompetition(competitionID)).Convert();
        }

        public async Task<LargeDataDTO> GetCompetitionLevel(int competitionID)
        {
            return (await _service.GetCompetitionLevel(competitionID)).Convert();
        }

        public async Task<IEnumerable<CompetitionRewardDTO>> GetRewardsFor(int competitionID)
        {
            return from n in (await _rewardService.GetCompetitionRewards(competitionID)) select n.Convert();
        }

    }
}
