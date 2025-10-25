using CompetitiveBackend.Core.Objects;
namespace CompetitiveBackend.Repositories
{
    public interface IPlayerParticipationRepository
    {
        /// <summary>
        /// Создать заявку на соревнование.
        /// </summary>
        /// <param name="participation">Заявка на соревнование.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        /// <exception cref="Exceptions.IncorrectOperationException">Соответствующая заявка уже существует.</exception>
        Task CreateParticipation(PlayerParticipation participation);

        /// <summary>
        /// Обновить заявку на соревновани.
        /// </summary>
        /// <param name="participation">Заявка на соревнование.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        /// <exception cref="Exceptions.IncorrectOperationException">Соответствующей заявки не существует.</exception>
        Task UpdateParticipation(PlayerParticipation participation);

        /// <summary>
        /// Удалить заявку игрока.
        /// </summary>
        /// <param name="accountID">Идентификатор аккаунта.</param>
        /// <param name="competitionID">Идентификатор соревнования.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task DeleteParticipation(int accountID, int competitionID);

        /// <summary>
        /// Получить заявку игрока.
        /// </summary>
        /// <param name="accountID">Идентификатор аккаунта.</param>
        /// <param name="competitionID">Идентификатор соревнования.</param>
        /// <param name="bindPlayer">Нужно ли возвращать информацию об игроке.</param>
        /// <param name="bindCompetition">Нужно ли возвращать информацию о соревновании.</param>
        /// <returns>Заявка игрока.</returns>
        Task<PlayerParticipation> GetParticipation(int accountID, int competitionID, bool bindPlayer = false, bool bindCompetition = false);

        /// <summary>
        /// Получить список заявок игрока.
        /// </summary>
        /// <param name="accountID">ID аккаунта.</param>
        /// <param name="limiter">Ограничитель данных.</param>
        /// <param name="bindPlayer">Нужно ли возвращать информацию об игроке.</param>
        /// <param name="bindCompetition">Нужно ли возвращать информацию о соревновании.</param>
        /// <returns>Заявки игрока.</returns>
        /// <exception cref="Exceptions.MissingDataException">Игрок с таким ID не найден.</exception>
        Task<IEnumerable<PlayerParticipation>> GetPlayerParticipations(int accountID, DataLimiter limiter, bool bindPlayer = false, bool bindCompetition = true);

        /// <summary>
        /// Получить список лидеров соревнования.
        /// </summary>
        /// <param name="competitionID">Идентификатор соревнования.</param>
        /// <param name="limiter">Ограничитель данных.</param>
        /// <param name="bindPlayer">Нужно ли возвращать информацию об игроке.</param>
        /// <param name="bindCompetition">Нужно ли возвращать информацию о соревновании.</param>
        /// <returns>Заявки игрока, расположенные по убыванию.</returns>
        /// <exception cref="Exceptions.MissingDataException">Соревнование с таким ID не найдено.</exception>
        Task<IEnumerable<PlayerParticipation>> GetLeaderboard(int competitionID, DataLimiter limiter, bool bindPlayer = true, bool bindCompetition = false);
    }
}
