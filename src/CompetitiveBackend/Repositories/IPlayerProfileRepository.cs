using CompetitiveBackend.Core.Objects;

namespace CompetitiveBackend.Repositories
{
    public abstract class LargeObject
    {
        protected abstract void Write(StreamWriter stream);
    }
    /// <summary>
    /// Хранилище профилей игроков
    /// </summary>
    public interface IPlayerProfileRepository
    {
        /// <summary>
        /// Найти профиль игрока по аккаунту
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        public Task<PlayerProfile> GetPlayerProfile(int accountId);
        /// <summary>
        /// Обновить профиль игрока
        /// </summary>
        /// <param name="p">Профиль игрока</param>
        /// <returns></returns>
        /// <exception cref="Core.Exceptions.Repository.IncorrectOperationException">Соответствующего игрока не существует</exception>
        public Task UpdatePlayerProfile(PlayerProfile p);
        /// <summary>
        /// Получить изображение игрока
        /// </summary>
        /// <param name="accountId">Идентификатор аккаунта</param>
        /// <returns>Данные изображения игрока</returns>
        /// <exception cref="Core.Exceptions.Repository.MissingDataException">Игрок с таким идентификатором не найден</exception>
        public Task<LargeData> GetPlayerProfileImage(int accountId);
        /// <summary>
        /// Обновить изображение у профиля игрока
        /// </summary>
        /// <param name="accountId">Идентификатор аккаунта</param>
        /// <param name="data">Изображение</param>
        /// <returns></returns>
        /// <exception cref="Core.Exceptions.Repository.IncorrectOperationException">Соответствующего игрока не существует</exception>
        public Task UpdatePlayerProfileImage(int accountId, LargeData data);
    }
}
