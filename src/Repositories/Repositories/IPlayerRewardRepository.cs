using CompetitiveBackend.Core.Objects;
namespace CompetitiveBackend.Repositories
{
    public interface IPlayerRewardRepository
    {
        /// <summary>
        /// Создать награду. rw.Description и rw.Name игнорируются.
        /// </summary>
        /// <param name="rw">Награда игрока.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task CreateReward(PlayerReward rw);

        /// <summary>
        /// Удалить награду по идентификатору.
        /// </summary>
        /// <param name="playerRewardID">Идентификатор награды.</param>
        /// <exception cref="Exceptions.MissingDataException">Соответствующей награды нет.</exception>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task DeleteReward(int playerRewardID);

        /// <summary>
        /// Получить все награды игрока.
        /// </summary>
        /// <param name="accountId">Идентификатор аккаунта.</param>
        /// <param name="limiter">Ограничитель данных.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task<IEnumerable<PlayerReward>> GetAllRewardsOf(int accountId, DataLimiter limiter);

        /// <summary>
        /// Выдать награды за прошедшее соревнование.
        /// </summary>
        /// <param name="competitionID">ID соревнования.</param>
        /// <returns>Награды за соревнование.</returns>
        /// <exception cref="Exceptions.IncorrectOperationException">Соревнование ещё не завершено.</exception>
        /// <exception cref="Exceptions.IncorrectOperationException">Награды за соревнование уже выданы.</exception>
        Task GrantRewardsFor(int competitionID);
    }
}
