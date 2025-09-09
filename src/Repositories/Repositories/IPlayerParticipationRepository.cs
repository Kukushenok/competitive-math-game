using CompetitiveBackend.Core.Objects;
namespace CompetitiveBackend.Repositories
{
    public interface IPlayerParticipationRepository
    {
        /// <summary>
        /// Создать заявку на соревнование
        /// </summary>
        /// <param name="participation">Заявка на соревнование</param>
        /// <returns></returns>
        /// <exception cref="CompetitiveBackend.Repositories.Exceptions.IncorrectOperationException">Соответствующая заявка уже существует</exception>
        public Task CreateParticipation(PlayerParticipation participation);
        /// <summary>
        /// Обновить заявку на соревновани
        /// </summary>
        /// <param name="participation">Заявка на соревнование</param>
        /// <returns></returns>
        ///<exception cref="CompetitiveBackend.Repositories.Exceptions.IncorrectOperationException">Соответствующей заявки не существует</exception>
        public Task UpdateParticipation(PlayerParticipation participation);
        /// <summary>
        /// Удалить заявку игрока
        /// </summary>
        /// <param name="accountID">Идентификатор аккаунта</param>
        /// <param name="competitionID">Идентификатор соревнования</param>
        public Task DeleteParticipation(int accountID, int competitionID);
        /// <summary>
        /// Получить заявку игрока
        /// </summary>
        /// <param name="accountID">Идентификатор аккаунта</param>
        /// <param name="competitionID">Идентификатор соревнования</param>
        /// <returns>Заявка игрока</returns>
        public Task<PlayerParticipation> GetParticipation(int accountID, int competitionID, bool bindPlayer = false, bool bindCompetition = false);
        /// <summary>
        /// Получить список заявок игрока.
        /// </summary>
        /// <param name="playerID">Идентификатор аккаунта</param>
        /// <param name="limiter">Ограничитель данных</param>
        /// <returns>Заявки игрока</returns>
        /// <exception cref="CompetitiveBackend.Repositories.Exceptions.MissingDataException">Игрок с таким ID не найден</exception>
        public Task<IEnumerable<PlayerParticipation>> GetPlayerParticipations(int accountID, DataLimiter limiter, bool bindPlayer = false, bool bindCompetition = true);
        /// <summary>
        /// Получить список лидеров соревнования
        /// </summary>
        /// <param name="competitionID">Идентификатор соревнования</param>
        /// <param name="limiter">Ограничитель данных</param>
        /// <returns>Заявки игрока, расположенные по убыванию</returns>
        /// <exception cref="CompetitiveBackend.Repositories.Exceptions.MissingDataException">Соревнование с таким ID не найдено</exception>
        public Task<IEnumerable<PlayerParticipation>> GetLeaderboard(int competitionID, DataLimiter limiter, bool bindPlayer = true, bool bindCompetition = false);
    }
}
