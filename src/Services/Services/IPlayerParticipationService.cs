using CompetitiveBackend.Core.Objects;
using CompetitiveBackend.Core;

namespace CompetitiveBackend.Services
{
    public interface IPlayerParticipationService
    {
        /// <summary>
        /// Отправить данные об участии.
        /// Если текущий результат меньше имеющегося, запись не происходит.
        /// </summary>
        /// <param name="userID">Идентификатор игрока</param>
        /// <param name="competitionID">Идентификатор соревнования</param>
        /// <param name="score">Результат</param>
        /// <returns></returns>
        public Task SubmitParticipation(int playerID, int competitionID, int score);
        /// <summary>
        /// Удалить участие игрока
        /// </summary>
        /// <param name="userID">Идентификатор игрока</param>
        /// <param name="competitionID">Идентификатор соревнования</param>
        /// <returns></returns>
        public Task DeleteParticipation(int playerID, int competitionID);
        /// <summary>
        /// Получить сведения участия игрока
        /// </summary>
        /// <param name="playerID">Идентификатор игрока</param>
        /// <param name="competitionID">Идентификатор соревнования</param>
        /// <returns></returns>
        public Task<PlayerParticipation> GetParticipation(int playerID, int competitionID);
        /// <summary>
        /// Получить множество данных об участии игрока в соревнованиях
        /// </summary>
        /// <param name="playerID">Идентификатор игрока</param>
        /// <param name="limiter">Ограничитель данных</param>
        /// <returns>Множество данных об участий</returns>
        public Task<IEnumerable<PlayerParticipation>> GetPlayerParticipations(int playerID, DataLimiter limiter);
        /// <summary>
        /// Получить упорядоченное множество данных об участии игроков в соревновании
        /// </summary>
        /// <param name="competitionID">Идентификатор соревнования</param>
        /// <param name="limiter">Ограничитель данных</param>
        /// <returns>Упорядоченное множество данных об участий игроков</returns>
        public Task<IEnumerable<PlayerParticipation>> GetLeaderboard(int competitionID, DataLimiter limiter);
    }
}
