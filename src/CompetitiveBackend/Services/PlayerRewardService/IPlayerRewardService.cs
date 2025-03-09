using CompetitiveBackend.Core.Objects;
using CompetitiveBackend.Core;

namespace CompetitiveBackend.Services.PlayerRewardService
{
    public interface IPlayerRewardService
    {
        /// <summary>
        /// Выдать игроку награду вручную
        /// </summary>
        /// <param name="playerID">Игрок</param>
        /// <param name="rewardDescriptionID">Описание награды</param>
        /// <returns></returns>
        public Task GrantRewardToPlayer(int playerID, int rewardDescriptionID);
        /// <summary>
        /// Удалить награду игрока
        /// </summary>
        /// <param name="playerRewardID">Идентификатор награды игрока</param>
        /// <returns></returns>
        public Task DeleteReward(int playerRewardID);
        /// <summary>
        /// Получить все награды игрока
        /// </summary>
        /// <param name="playerID">Идентификатор игрока</param>
        /// <param name="limiter">Ограничитель данных</param>
        /// <returns>Множество наград игрока</returns>
        public Task<IEnumerable<PlayerReward>> GetAllRewardsOf(int playerID, DataLimiter limiter);
    }
}
