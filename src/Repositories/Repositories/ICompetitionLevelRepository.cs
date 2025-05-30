using CompetitiveBackend.Core.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompetitiveBackend.Repositories
{
    public interface ICompetitionLevelRepository
    {
       
        public Task<LargeData> GetCompetitionLevel(int competitionID, string? Platform = null, int? MaxVersion = null);
        public Task<LargeData> GetSpecificCompetitionLevel(int levelDataID);
        public Task<LevelDataInfo> GetSpecificCompetitionLevelInfo(int levelDataID);
      
        public Task<IEnumerable<LevelDataInfo>> GetAllLevelData(int competitionID);

        public Task AddCompetitionLevel(LargeData data, LevelDataInfo levelData);
        public Task UpdateCompetitionLevelInfo(LevelDataInfo levelData);
        public Task UpdateCompetitionLevelData(int levelDataID, LargeData data);
        public Task DeleteCompetitionLevel(int levelDataID);
    }
}
