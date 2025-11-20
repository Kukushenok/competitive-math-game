using CompetitiveBackend.Core.Objects;

namespace CompetitiveBackend.Services
{
    [Obsolete("There is no way you can use it")]
    public interface ICompetitionLevelService
    {
        Task<IEnumerable<LevelDataInfo>> GetAllLevelData(int competitionId);
        Task CreateLevelData(LevelDataInfo levelDataInfo, LargeData embeddedData);
        Task UpdateCompetitionLevelData(int levelDataID, LargeData largeData);

        Task UpdateCompetitionLevelInfo(LevelDataInfo levelDataInfo);
        Task DeleteLevelData(int levelData);
        Task<LargeData> GetCompetitionLevel(int competitionID, string? platform = null, int? maxVersion = null);
        Task<LargeData> GetSpecificCompetitionLevel(int levelDataID);
        Task<LevelDataInfo> GetSpecificCompetitionLevelInfo(int levelDataID);
    }
}
