using CompetitiveBackend.Core.Objects;
using CompetitiveBackend.Core.RewardCondition;

namespace CompetitiveBackend.Services
{
    public interface ICompetitionRewardService
    {
        /// <summary>
        /// Получить множество наград, получаемых за соревнование.
        /// </summary>
        /// <param name="competitionID">Идентификатор соревнования.</param>
        /// <returns>Множество наград, получаемых за соревнование.</returns>
        Task<IEnumerable<CompetitionReward>> GetCompetitionRewards(int competitionID);

        /// <summary>
        /// Создать награду для соревнования.
        /// </summary>
        /// <param name="reward">Награда за соревнование.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task CreateCompetitionReward(CompetitionReward reward);

        /// <summary>
        /// Обновить данные о соревновании.
        /// </summary>
        /// <param name="compRewardID">Идентификатор награды соревнования.</param>
        /// <param name="rewardDescriptionID">Описание награды (null, если менять не нужно).</param>
        /// <param name="condition">Свойство, за что выдаётся награда.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task UpdateCompetitionReward(int compRewardID, int? rewardDescriptionID = null, GrantCondition? condition = null);

        /// <summary>
        /// Удалить награду у соревнования.
        /// </summary>
        /// <param name="compRewardID">Идентификатор награды соревнования.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task RemoveCompetitionReward(int compRewardID);
    }
}
