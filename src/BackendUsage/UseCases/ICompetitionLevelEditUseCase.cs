using System.Collections.Generic;
using System.Threading.Tasks;
using CompetitiveBackend.BackendUsage.Objects;

namespace CompetitiveBackend.BackendUsage.UseCases
{
    [System.Obsolete("Changed API")]
    public interface ICompetitionLevelEditUseCase : IAuthableUseCase<ICompetitionLevelEditUseCase>
    {
        Task<IEnumerable<LevelDataInfoDTO>> GetLevelInfos(int competitionID);
        Task AddLevelToCompetition(LevelDataInfoDTO levelDataInfo, LargeDataDTO levelContents);
        Task UpdateLevelDataInfo(LevelDataInfoDTO levelDataInfo);
        Task UpdateLevelData(int levelId, LargeDataDTO levelContents);
        Task DeleteLevel(int levelId);
        Task<LargeDataDTO> GetSpecificCompetitionData(int levelID);
        Task<LevelDataInfoDTO> GetSpecificCompetitionInfo(int levelID);
    }
}
