using CompetitiveBackend.Core.Objects;

namespace CompetitiveBackend.Repositories
{
    [Obsolete("Disfunct")]
    public interface ICompetitionLevelRepository
    {
        Task<LargeData> GetCompetitionLevel(int competitionID, string? platform = null, int? maxVersion = null);
        Task<LargeData> GetSpecificCompetitionLevel(int levelDataID);

        Task<LevelDataInfo> GetSpecificCompetitionLevelInfo(int levelDataID);

        Task<IEnumerable<LevelDataInfo>> GetAllLevelData(int competitionID);

        Task AddCompetitionLevel(LargeData data, LevelDataInfo levelData);
        Task UpdateCompetitionLevelInfo(LevelDataInfo levelData);
        Task UpdateCompetitionLevelData(int levelDataID, LargeData data);
        Task DeleteCompetitionLevel(int levelDataID);
    }
}
