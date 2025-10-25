using CompetitiveBackend.Core.Objects;

namespace CompetitiveBackend.Repositories
{
    public abstract class LargeObject
    {
        protected abstract void Write(StreamWriter stream);
    }

    /// <summary>
    /// Хранилище профилей игроков.
    /// </summary>
    public interface IPlayerProfileRepository
    {
        /// <summary>
        /// Найти профиль игрока по аккаунту.
        /// </summary>
        /// <param name="accountId">ID аккаунта.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task<PlayerProfile> GetPlayerProfile(int accountId);

        /// <summary>
        /// Обновить профиль игрока.
        /// </summary>
        /// <param name="p">Профиль игрока.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        /// <exception cref="Exceptions.IncorrectOperationException">Соответствующего игрока не существует.</exception>
        Task UpdatePlayerProfile(PlayerProfile p);

        /// <summary>
        /// Получить изображение игрока.
        /// </summary>
        /// <param name="accountId">Идентификатор аккаунта.</param>
        /// <returns>Данные изображения игрока.</returns>
        /// <exception cref="Exceptions.MissingDataException">Игрок с таким идентификатором не найден.</exception>
        Task<LargeData> GetPlayerProfileImage(int accountId);

        /// <summary>
        /// Обновить изображение у профиля игрока.
        /// </summary>
        /// <param name="accountId">Идентификатор аккаунта.</param>
        /// <param name="data">Изображение.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        /// <exception cref="Exceptions.MissingDataException">Соответствующего игрока не существует.</exception>
        Task UpdatePlayerProfileImage(int accountId, LargeData data);
    }
}
