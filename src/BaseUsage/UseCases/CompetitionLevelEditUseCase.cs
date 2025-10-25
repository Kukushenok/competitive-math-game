using CompetitiveBackend.BackendUsage.Objects;
using CompetitiveBackend.BackendUsage.UseCases;
using CompetitiveBackend.BaseUsage.Converters;
using CompetitiveBackend.Services;

namespace CompetitiveBackend.BaseUsage.UseCases
{
    [Obsolete("Defunct")]
    public class CompetitionLevelEditUseCase : BaseAuthableUseCase<CompetitionLevelEditUseCase>, ICompetitionLevelEditUseCase
    {
        private readonly ICompetitionLevelService competitionLevelService;
        public CompetitionLevelEditUseCase(IAuthService authService, ICompetitionLevelService competitionLevelService)
            : base(authService)
        {
            this.competitionLevelService = competitionLevelService;
        }

        public async Task AddLevelToCompetition(LevelDataInfoDTO levelDataInfo, LargeDataDTO levelContents)
        {
            AdminAuthCheck(out _);
            await competitionLevelService.CreateLevelData(levelDataInfo.Convert(), levelContents.Convert());
        }

        public async Task DeleteLevel(int levelId)
        {
            AdminAuthCheck(out _);
            await competitionLevelService.DeleteLevelData(levelId);
        }

        public async Task<IEnumerable<LevelDataInfoDTO>> GetLevelInfos(int competitionID)
        {
            AdminAuthCheck(out _);
            return from a in await competitionLevelService.GetAllLevelData(competitionID) select a.Convert();
        }

        public async Task<LargeDataDTO> GetSpecificCompetitionData(int levelID)
        {
            AdminAuthCheck(out _);
            return (await competitionLevelService.GetSpecificCompetitionLevel(levelID)).Convert();
        }

        public async Task<LevelDataInfoDTO> GetSpecificCompetitionInfo(int levelID)
        {
            AdminAuthCheck(out _);
            return (await competitionLevelService.GetSpecificCompetitionLevelInfo(levelID)).Convert();
        }

        public async Task UpdateLevelData(int levelId, LargeDataDTO levelContents)
        {
            AdminAuthCheck(out _);
            await competitionLevelService.UpdateCompetitionLevelData(levelId, levelContents.Convert());
        }

        public async Task UpdateLevelDataInfo(LevelDataInfoDTO levelDataInfo)
        {
            AdminAuthCheck(out _);
            await competitionLevelService.UpdateCompetitionLevelInfo(levelDataInfo.Convert());
        }

        [Obsolete]
        async Task<ICompetitionLevelEditUseCase> IAuthableUseCase<ICompetitionLevelEditUseCase>.Auth(string token)
        {
            return await Auth(token);
        }
    }
}
