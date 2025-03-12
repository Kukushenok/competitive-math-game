using CompetitiveBackend.Core.Objects;
namespace CompetitiveBackend.Services
{
    public interface IRewardDescriptionService
    {
        /// <summary>
        /// Добавить описание награды
        /// </summary>
        /// <param name="description">Описание награды</param>
        /// <returns></returns>
        public Task CreateRewardDescription(RewardDescription description);
        /// <summary>
        /// Обновить описание награды
        /// </summary>
        /// <param name="rewardDescrID">Идентификатор описания награды</param>
        /// <param name="name">Имя награды (null, если не нужно менять)</param>
        /// <param name="description">Описание награды (null, если не нужно менять)</param>
        /// <returns></returns>
        public Task UpdateRewardDescription(int rewardDescrID, string? name = null, string? description = null);
        /// <summary>
        /// Получить описание награды по идентификатору
        /// </summary>
        /// <param name="rewardDescrID">Идентификатор описания награды</param>
        /// <returns></returns>
        public Task<RewardDescription> GetRewardDescription(int rewardDescrID);
        /// <summary>
        /// Получить множество всех существующих описаний наград
        /// </summary>
        /// <param name="limiter">Ограничитель данных</param>
        /// <returns>Множество всех существующих описаний наград</returns>
        public Task<IEnumerable<RewardDescription>> GetAllRewardDescriptions(DataLimiter limiter);
        /// <summary>
        /// Получить картинку награды
        /// </summary>
        /// <param name="rewardID">Идентификатор описания награды</param>
        /// <returns>Данные о картинке</returns>
        public Task<LargeData> GetRewardIcon(int rewardID);
        /// <summary>
        /// Задать картинку награды
        /// </summary>
        /// <param name="rewardID">Идентификатор описания награды</param>
        /// <param name="data">Данные о картинке</param>
        /// <returns></returns>
        public Task SetRewardIcon(int rewardID, LargeData data);
        /// <summary>
        /// Получить внутриигровое представление награды
        /// </summary>
        /// <param name="rewardID">Идентификатор описания награды</param>
        /// <returns>Данные о внутриигрововом представлении награды</returns>
        public Task<LargeData> GetRewardGameAsset(int rewardID);
        /// <summary>
        /// Задать внутриигровое представление награды
        /// </summary>
        /// <param name="rewardID">Идентификатор описания награды</param>
        /// <param name="data">Данные о картинке</param>
        /// <returns></returns>
        public Task SetRewardGameAsset(int rewardID, LargeData data);
    }
}
