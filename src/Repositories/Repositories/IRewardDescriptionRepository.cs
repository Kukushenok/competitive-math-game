using CompetitiveBackend.Core.Objects;

namespace CompetitiveBackend.Repositories
{
    public interface IRewardDescriptionRepository
    {
        /// <summary>
        /// Добавить описание награды
        /// </summary>
        /// <param name="description">Описание награды. ID может быть null</param>
        public Task CreateRewardDescription(RewardDescription description);
        /// <summary>
        /// Обновить описание награды
        /// </summary>
        /// <param name="description">Описание награды. ID не может быть null</param>
        /// <exception cref="CompetitiveBackend.Repositories.Exceptions.IncorrectOperationException">Описания награды с таким ID не существует</exception>
        public Task UpdateRewardDescription(RewardDescription description);
        /// <summary>
        /// Получить описание награды по идентификатору
        /// </summary>
        /// <param name="rewardID">Идентификатор описания награды</param>
        /// <returns>Описание награды</returns>
        /// <exception cref="CompetitiveBackend.Repositories.Exceptions.IncorrectOperationException">Описания награды с таким ID не существует</exception>
        public Task<RewardDescription> GetRewardDescription(int rewardID);
        /// <summary>
        /// Получить описания всех существующих наград
        /// </summary>
        /// <param name="limiter">Ограничитель данных</param>
        /// <returns>Описания всех существующих наград</returns>
        public Task<IEnumerable<RewardDescription>> GetAllRewardDescriptions(DataLimiter limiter);
        /// <summary>
        /// Получить данные об иконке награды
        /// </summary>
        /// <param name="rewardID">Идентификатор описания награды</param>
        /// <returns>Данные</returns>
        /// <exception cref="CompetitiveBackend.Repositories.Exceptions.MissingDataException">Описания награды с таким ID не существует</exception>
        public Task<LargeData> GetRewardIcon(int rewardID);
        /// <summary>
        /// Задать данные об иконке награды
        /// </summary>
        /// <param name="rewardID">Идентификатор описания награды</param>
        /// <param name="data">Данные</param>
        /// <exception cref="CompetitiveBackend.Repositories.Exceptions.MissingDataException">Описания награды с таким ID не существует</exception>
        public Task SetRewardIcon(int rewardID, LargeData data);
        /// <summary>
        /// Получить данные об игровом представлении награды
        /// </summary>
        /// <param name="rewardID">Идентификатор описания награды</param>
        /// <returns>Данные</returns>
        /// <exception cref="CompetitiveBackend.Repositories.Exceptions.IncorrectOperationException">Описания награды с таким ID не существует</exception>
        public Task<LargeData> GetRewardGameAsset(int rewardID);
        /// <summary>
        /// Задать данные об игровом представлении награды
        /// </summary>
        /// <param name="rewardID">Идентификатор описания награды</param>
        /// <param name="data">Данные</param>
        /// <exception cref="CompetitiveBackend.Repositories.Exceptions.MissingDataException">Описания награды с таким ID не существует</exception>
        public Task SetRewardGameAsset(int rewardID, LargeData data);
    }
}
