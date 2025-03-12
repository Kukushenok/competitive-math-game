using CompetitiveBackend.Core.Objects;

namespace CompetitiveBackend.Services
{
    public interface IPlayerProfileService
    {
        /// <summary>
        /// Найти профиль игрока по аккаунту
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        public Task<PlayerProfile> GetPlayerProfile(int playerID);
        /// <summary>
        /// Обновить профиль игрока
        /// </summary>
        /// <param name="p">Профиль игрока</param>
        /// <returns></returns>
        public Task UpdatePlayerProfile(PlayerProfile p);
        /// <summary>
        /// Получить изображение игрока
        /// </summary>
        /// <param name="p">Профиль игрока</param>
        /// <returns>Данные изображения игрока</returns>
        public Task<LargeData> GetPlayerProfileImage(int playerID);
        /// <summary>
        /// Обновить изображение у профиля игрока
        /// </summary>
        /// <param name="p">Профиль игрока</param>
        /// <param name="data">Изображение</param>
        /// <returns></returns>
        public Task SetPlayerProfileImage(int playerID, LargeData data);
    }
}
