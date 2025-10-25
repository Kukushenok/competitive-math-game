using CompetitiveBackend.Core.Objects;
namespace CompetitiveBackend.Repositories
{
    public interface ICompetitionRepository
    {
        /// <summary>
        /// Получить все соревнования.
        /// </summary>
        /// <param name="dataLimiter">Лимит запроса.</param>
        /// <returns>Соревнования.</returns>
        Task<IEnumerable<Competition>> GetAllCompetitions(DataLimiter dataLimiter);

        /// <summary>
        /// Получить актуальные соревнования, чей срок на данный момент не истёк.
        /// </summary>
        /// <returns>Соревнования.</returns>
        Task<IEnumerable<Competition>> GetActiveCompetitions();

        /// <summary>
        /// Создать новое соревнование.
        /// </summary>
        /// <param name="c">Соревнование. ID задан в null.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task<int> CreateCompetition(Competition c);

        /// <summary>
        /// Обновить данные о соревновании.
        /// </summary>
        /// <param name="c">Соревнование, где ID явно указан.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        /// <exception cref="Exceptions.IncorrectOperationException">Соревнование с таким ID уже существует.</exception>
        Task UpdateCompetition(Competition c);

        /// <summary>
        /// Получить данные о соревновании по идентификатору.
        /// </summary>
        /// <param name="competitionID">Идентификатор соревнования.</param>
        /// <returns>Данные о соревновании.</returns>
        /// <exception cref="Exceptions.MissingDataException">Соревнование с таким ID не найдено.</exception>
        Task<Competition> GetCompetition(int competitionID);
    }
}
