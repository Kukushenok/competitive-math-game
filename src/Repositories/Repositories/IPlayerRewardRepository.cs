using CompetitiveBackend.Core.Objects;
namespace CompetitiveBackend.Repositories
{
    public interface IPlayerRewardRepository
    {
        /// <summary>
        /// Создать награду. rw.Description и rw.Name игнорируются.
        /// </summary>
        /// <param name="rw"></param>
        /// <returns></returns>
        public Task CreateReward(PlayerReward rw);
        /// <summary>
        /// Удалить награду по идентификатору
        /// </summary>
        /// <param name="playerRewardID">Идентификатор награды</param>
        /// <exception cref="CompetitiveBackend.Repositories.Exceptions.MissingDataException">Соответствующей награды нет</exception>
        public Task DeleteReward(int playerRewardID);
        /// <summary>
        /// Получить все награды игрока
        /// </summary>
        /// <param name="accountId">Идентификатор аккаунта</param>
        /// <param name="limiter">Ограничитель данных</param>
        /// <returns></returns>
        public Task<IEnumerable<PlayerReward>> GetAllRewardsOf(int accountId, DataLimiter limiter);
        /// <summary>
        /// Выдать награды за прошедшее соревнование
        /// </summary>
        /// <param name="competitionID"></param>
        /// <returns>Награды за соревнование</returns>
        /// <exception cref="CompetitiveBackend.Repositories.Exceptions.IncorrectOperationException">Соревнование ещё не завершено</exception>
        /// <exception cref="CompetitiveBackend.Repositories.Exceptions.IncorrectOperationException">Награды за соревнование уже выданы</exception>
        public Task GrantRewardsFor(int competitionID);
    }
}
