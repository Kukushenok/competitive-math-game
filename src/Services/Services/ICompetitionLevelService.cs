using CompetitiveBackend.Core.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompetitiveBackend.Services
{
    public interface ICompetitionLevelService
    {
        public Task<IEnumerable<LevelDataInfo>> GetAllLevelData(int competitionId);
        public Task CreateLevelData(LevelDataInfo levelDataInfo, LargeData embeddedData);
        public Task UpdateCompetitionLevelData(int levelDataID, LargeData largeData);
        public Task UpdateCompetitionLevelInfo(LevelDataInfo levelDataInfo);
        public Task DeleteLevelData(int levelData);
        /// <summary>
        /// Получить данные об игровом представлении уровня соревнования
        /// </summary>
        /// <param name="competitionID">Идентификатор соревнования</param>
        /// <returns>Данные об игровом представлении уровня соревнования</returns>
        public Task<LargeData> GetCompetitionLevel(int competitionID, string? platform = null, int? maxVersion = null);
        public Task<LargeData> GetSpecificCompetitionLevel(int levelDataID);
        public Task<LevelDataInfo> GetSpecificCompetitionLevelInfo(int levelDataID);
    }
}
