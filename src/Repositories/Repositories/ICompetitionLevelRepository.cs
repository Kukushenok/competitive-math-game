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
        /// <summary>
        /// Получить данные об уровне соревнования
        /// </summary>
        /// <param name="competitionID">Идентификатор соревнования</param>
        /// <returns>Данные</returns>
        /// <exception cref="CompetitiveBackend.Repositories.Exceptions.MissingDataException">Соревнование с таким ID не найдено</exception>
        public Task<LargeData> GetCompetitionLevel(int competitionID, string? Platform = null, int? MaxVersion = null);
        public Task<LargeData> GetCompetitionLevel(int levelDataID);
        /// <summary>
        /// Загрузить уровень соревнования
        /// </summary>
        /// <param name="competitionID">Идентификатор соревнования</param>
        /// <param name="levelData">Данные об уровне соревнования</param>
        /// <exception cref="CompetitiveBackend.Repositories.Exceptions.MissingDataException">Соревнование с таким ID не найдено</exception>
        public Task<IEnumerable<LevelDataInfo>> GetAllLevelData(int competitionID);

        public Task AddCompetitionLevel(LargeData data, LevelDataInfo levelData);
        public Task UpdateCompetitionLevelInfo(LevelDataInfo levelData);
        public Task UpdateCompetitionLevelData(int levelDataID, LargeData data);
        public Task DeleteCompetitionLevel(int levelDataID);
    }
}
