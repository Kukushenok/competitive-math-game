using CompetitiveBackend.Core.Objects;

namespace CompetitiveBackend.Repositories
{
    public interface ICompetitionRepository
    {
        /// <summary>
        /// Получить все соревнования
        /// </summary>
        /// <param name="dataLimiter">Лимит запроса</param>
        /// <returns>Соревнования</returns>
        public Task<IEnumerable<Competition>> GetAllCompetitions(DataLimiter dataLimiter);
        /// <summary>
        /// Получить актуальные соревнования, чей срок на данный момент не истёк.
        /// </summary>
        /// <returns>Соревнования</returns>
        public Task<IEnumerable<Competition>> GetActiveCompetitions();
        /// <summary>
        /// Создать новое соревнование.
        /// </summary>
        /// <param name="c">Соревнование. ID задан в null</param>
        /// <returns></returns>
        public Task CreateCompetition(Competition c);
        /// <summary>
        /// Обновить данные о соревновании
        /// </summary>
        /// <param name="c">Соревнование, где ID явно указан</param>
        /// <returns></returns>
        /// <exception cref="CompetitiveBackend.Repositories.Exceptions.IncorrectOperationException">Соревнование с таким ID уже существует</exception>
        public Task UpdateCompetition(Competition c);
        /// <summary>
        /// Получить данные о соревновании по идентификатору
        /// </summary>
        /// <param name="competitionID">Идентификатор соревнования</param>
        /// <returns>Данные о соревновании</returns>
        /// <exception cref="CompetitiveBackend.Repositories.Exceptions.MissingDataException">Соревнование с таким ID не найдено</exception>
        public Task<Competition> GetCompetition(int competitionID);
        /// <summary>
        /// Получить данные об уровне соревнования
        /// </summary>
        /// <param name="competitionID">Идентификатор соревнования</param>
        /// <returns>Данные</returns>
        /// <exception cref="CompetitiveBackend.Repositories.Exceptions.MissingDataException">Соревнование с таким ID не найдено</exception>
        public Task<LargeData> GetCompetitionLevel(int competitionID);
        /// <summary>
        /// Загрузить уровень соревнования
        /// </summary>
        /// <param name="competitionID">Идентификатор соревнования</param>
        /// <param name="levelData">Данные об уровне соревнования</param>
        /// <exception cref="CompetitiveBackend.Repositories.Exceptions.MissingDataException">Соревнование с таким ID не найдено</exception>
        public Task SetCompetitionLevel(int competitionID, LargeData levelData);
    }
}
