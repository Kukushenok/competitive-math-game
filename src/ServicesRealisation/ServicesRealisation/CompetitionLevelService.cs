using CompetitiveBackend.Core.Objects;
using CompetitiveBackend.Repositories;

namespace CompetitiveBackend.Services.ServicesRealisation
{
    [Obsolete("Deprecated functionality which doesnt work now")]
    public class CompetitionLevelService : ICompetitionLevelService
    {
        private readonly ICompetitionLevelRepository levelRepository;
        public CompetitionLevelService(ICompetitionLevelRepository levelRepository)
        {
            this.levelRepository = levelRepository;
        }

        [Obsolete("Deprecated functionality which doesnt work now")]
        public async Task CreateLevelData(LevelDataInfo levelDataInfo, LargeData embeddedData)
        {
            await levelRepository.AddCompetitionLevel(embeddedData, levelDataInfo);
        }

        public async Task DeleteLevelData(int levelData)
        {
            await levelRepository.DeleteCompetitionLevel(levelData);
        }

        public async Task<IEnumerable<LevelDataInfo>> GetAllLevelData(int competitionId)
        {
            return await levelRepository.GetAllLevelData(competitionId);
        }

        public Task<LargeData> GetCompetitionLevel(int competitionID, string? platform = null, int? maxVersion = null)
        {
            return levelRepository.GetCompetitionLevel(competitionID, platform, maxVersion);
        }

        public Task<LargeData> GetSpecificCompetitionLevel(int levelDataID)
        {
            return levelRepository.GetSpecificCompetitionLevel(levelDataID);
        }

        public Task<LevelDataInfo> GetSpecificCompetitionLevelInfo(int levelDataID)
        {
            return levelRepository.GetSpecificCompetitionLevelInfo(levelDataID);
        }

        public Task UpdateCompetitionLevelData(int levelDataID, LargeData largeData)
        {
            return levelRepository.UpdateCompetitionLevelData(levelDataID, largeData);
        }

        public Task UpdateCompetitionLevelInfo(LevelDataInfo levelDataInfo)
        {
            return levelRepository.UpdateCompetitionLevelInfo(levelDataInfo);
        }
    }
}
