using System.Text;
using CompetitiveBackend.BackendUsage.Objects;
using CompetitiveBackend.BackendUsage.UseCases;
using CompetitiveBackend.BaseUsage.Converters;
using CompetitiveBackend.Services;

namespace CompetitiveBackend.BaseUsage.UseCases
{
    public class CompetitionWatchUseCase : ICompetitionWatchUseCase
    {
        private readonly ICompetitionService service;

        // private readonly ICompetitionLevelService competitionLevelService;
        private readonly ICompetitionRewardService rewardService;
        public CompetitionWatchUseCase(ICompetitionService service, ICompetitionRewardService rewardService)
        {
            this.service = service;
            this.rewardService = rewardService;
        }

        public async Task<IEnumerable<CompetitionDTO>> GetActiveCompetitions()
        {
            return from n in await service.GetActiveCompetitions() select n.Convert();
        }

        public async Task<IEnumerable<CompetitionDTO>> GetAllCompetitions(DataLimiterDTO limiter)
        {
            return from n in await service.GetAllCompetitions(limiter.Convert()) select n.Convert();
        }

        public async Task<CompetitionDTO> GetCompetition(int competitionID)
        {
            return (await service.GetCompetition(competitionID)).Convert();
        }

        public Task<LargeDataDTO> GetCompetitionLevel(int competitionID, string? platform = null, int? maxVersion = null)
        {
            throw new NotImplementedException();

            // return (await competitionLevelService.GetCompetitionLevel(competitionID, platform, maxVersion)).Convert();
        }

        public async Task<IEnumerable<CompetitionRewardDTO>> GetRewardsFor(int competitionID)
        {
            return from n in await rewardService.GetCompetitionRewards(competitionID) select n.Convert();
        }
    }
}
