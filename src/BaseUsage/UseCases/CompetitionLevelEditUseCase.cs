using CompetitiveBackend.BackendUsage.Objects;
using CompetitiveBackend.BackendUsage.UseCases;
using CompetitiveBackend.BaseUsage.Converters;
using CompetitiveBackend.Services;

namespace CompetitiveBackend.BaseUsage.UseCases
{
    public class CompetitionLevelEditUseCase : BaseAuthableUseCase<CompetitionLevelEditUseCase>, ICompetitionLevelEditUseCase
    {
        private ICompetitionLevelService _competitionLevelService;
        public CompetitionLevelEditUseCase(IAuthService authService, ICompetitionLevelService competitionLevelService) : base(authService)
        {
            _competitionLevelService = competitionLevelService;
        }

        public async Task AddLevelToCompetition(LevelDataInfoDTO levelDataInfo, LargeDataDTO levelContents)
        {
            AdminAuthCheck(out _);
            await _competitionLevelService.CreateLevelData(levelDataInfo.Convert(), levelContents.Convert());
        }

        public async Task DeleteLevel(int levelId)
        {
            AdminAuthCheck(out _);
            await _competitionLevelService.DeleteLevelData(levelId);
        }

        public async Task<IEnumerable<LevelDataInfoDTO>> GetLevelInfos(int competitionID)
        {
            AdminAuthCheck(out _);
            return from a in await _competitionLevelService.GetAllLevelData(competitionID) select a.Convert();
        }

        public async Task UpdateLevelData(int levelId, LargeDataDTO levelContents)
        {
            AdminAuthCheck(out _);
            await _competitionLevelService.UpdateCompetitionLevelData(levelId, levelContents.Convert());
        }

        public async Task UpdateLevelDataInfo(LevelDataInfoDTO levelDataInfo)
        {
            AdminAuthCheck(out _);
            await _competitionLevelService.UpdateCompetitionLevelInfo(levelDataInfo.Convert());
        }

        async Task<ICompetitionLevelEditUseCase> IAuthableUseCase<ICompetitionLevelEditUseCase>.Auth(string token) => await Auth(token);
    }
}
