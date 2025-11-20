using CompetitiveBackend.Core.Objects;

namespace CompetitiveBackend.Services
{
    public interface IPlayerProfileService
    {
        /// <summary>
        /// Найти профиль игрока по аккаунту.
        /// </summary>
        /// <param name="playerID">ID игрока.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task<PlayerProfile> GetPlayerProfile(int playerID);

        /// <summary>
        /// Обновить профиль игрока.
        /// </summary>
        /// <param name="p">Профиль игрока.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task UpdatePlayerProfile(PlayerProfile p);

        /// <summary>
        /// Получить изображение игрока.
        /// </summary>
        /// <param name="playerID">ID игрока.</param>
        /// <returns>Данные изображения игрока.</returns>
        Task<LargeData> GetPlayerProfileImage(int playerID);

        /// <summary>
        /// Обновить изображение у профиля игрока.
        /// </summary>
        /// <param name="playerID">ID игрока.</param>
        /// <param name="data">Изображение.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task SetPlayerProfileImage(int playerID, LargeData data);
    }
}
