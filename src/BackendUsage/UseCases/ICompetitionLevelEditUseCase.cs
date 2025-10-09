using CompetitiveBackend.BackendUsage.Objects;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CompetitiveBackend.BackendUsage.UseCases
{
    [System.Obsolete("Changed API")]
    public interface ICompetitionLevelEditUseCase: IAuthableUseCase<ICompetitionLevelEditUseCase>
    {
        public Task<IEnumerable<LevelDataInfoDTO>> GetLevelInfos(int competitionID);
        public Task AddLevelToCompetition(LevelDataInfoDTO levelDataInfo, LargeDataDTO levelContents);
        public Task UpdateLevelDataInfo(LevelDataInfoDTO levelDataInfo);
        public Task UpdateLevelData(int levelId, LargeDataDTO levelContents);
        public Task DeleteLevel(int levelId);
        public Task<LargeDataDTO> GetSpecificCompetitionData(int levelID);
        public Task<LevelDataInfoDTO> GetSpecificCompetitionInfo(int levelID);
    }
}
