using CompetitiveBackend.Core.Objects;
using CompetitiveBackend.Core;

namespace CompetitiveBackend.Services.CompetitionService
{
    public interface ICompetitionService
    {
        /// <summary>
        /// Получить информацию о всех соревнованиях
        /// </summary>
        /// <param name="dataLimiter">Ограничитель данных</param>
        /// <returns>Множество соревнований</returns>
        public Task<IEnumerable<Competition>> GetAllCompetitions(DataLimiter dataLimiter);
        /// <summary>
        /// Получить множество активных соревнований (которые ещё не закончились)
        /// </summary>
        /// <param name="dataLimiter">Ограничитель данных</param>
        /// <returns>Множество активных соревнований</returns>
        public Task<IEnumerable<Competition>> GetActiveCompetitions();
        /// <summary>
        /// Создать соревнование
        /// </summary>
        /// <param name="competition">Данные о соревновании</param>
        /// <returns></returns>
        public Task CreateCompetition(Competition competition);
        /// <summary>
        /// Обновить данные о соревновании
        /// </summary>
        /// <param name="id">Идентификатор соревнования</param>
        /// <param name="name">Имя соревнования (null, если менять не нужно)</param>
        /// <param name="description">Описание соревнования (null, если менять не нужно)</param>
        /// <param name="startDate">Дата начала (null, если менять не нужно)</param>
        /// <param name="endDate">Дата конца (null, если менять не нужно)</param>
        /// <returns></returns>
        public Task UpdateCompetition(int id, string? name = null, string? description = null, DateTime? startDate = null, DateTime? endDate = null);
        /// <summary>
        /// Получить данные о соревновании по идентификатору
        /// </summary>
        /// <param name="competitionID">Идентификатор соревнования</param>
        /// <returns>Данные о соревновании</returns>
        public Task<Competition> GetCompetition(int competitionID);
        /// <summary>
        /// Получить данные об игровом представлении уровня соревнования
        /// </summary>
        /// <param name="competitionID">Идентификатор соревнования</param>
        /// <returns>Данные об игровом представлении уровня соревнования</returns>
        public Task<LargeData> GetCompetitionLevel(int competitionID);
        /// <summary>
        /// Задать данные об игровом представлении уровня соревнования
        /// </summary>
        /// <param name="competitionID">Идентификатор соревнования</param>
        /// <param name="levelData">Данные об игровом представлении уровня соревнования</param>
        /// <returns></returns>
        public Task SetCompetitionLevel(int competitionID, LargeData levelData);

    }
}
