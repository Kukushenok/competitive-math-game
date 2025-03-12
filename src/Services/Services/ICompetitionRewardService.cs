using CompetitiveBackend.Core.Objects;

namespace CompetitiveBackend.Services
{
    public interface ICompetitionRewardService
    {
        /// <summary>
        /// Получить множество наград, получаемых за соревнование
        /// </summary>
        /// <param name="competitionID">Идентификатор соревнования</param>
        /// <returns>Множество наград, получаемых за соревнование</returns>
        public Task<IEnumerable<CompetitionReward>> GetCompetitionRewards(int competitionID);
        /// <summary>
        /// Создать награду для соревнования
        /// </summary>
        /// <param name="reward">Награда за соревнование</param>
        /// <returns></returns>
        public Task CreateCompetitionReward(CompetitionReward reward);
        /// <summary>
        /// Обновить данные о соревновании
        /// </summary>
        /// <param name="compRewardID">Идентификатор награды соревнования</param>
        /// <param name="rewardDescriptionID">Описание награды (null, если менять не нужно)</param>
        /// <param name="condition">Название свойства, за что выдаётся награда</param>
        /// <param name="conditionDescription">Параметры свойства, за что выдаётся награда</param>
        /// <returns></returns>
        public Task UpdateCompetitionReward(int compRewardID, int? rewardDescriptionID = null, string? condition = null, string? conditionDescription = null);
        /// <summary>
        /// Удалить награду у соревнования
        /// </summary>
        /// <param name="compRewardID">Идентификатор награды соревнования</param>
        /// <returns></returns>
        public Task RemoveCompetitionReward(int compRewardID);
    }
}
