using CompetitiveBackend.Core.Objects;

namespace CompetitiveBackend.Repositories
{
    public interface ICompetitionRewardRepository
    {
        /// <summary>
        /// Получить данные о наградах соревнования.
        /// </summary>
        /// <param name="competitionID">Идентификатор соревнования.</param>
        /// <returns>Награды за соревнование.</returns>
        Task<IEnumerable<CompetitionReward>> GetCompetitionRewards(int competitionID);

        /// <summary>
        /// Получить данные о конкретной награде соревнования.
        /// </summary>
        /// <param name="competitionRewardID">Идентификатор награды соревнования.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        /// <exception cref="Exceptions.MissingDataException">Соревнование с таким ID не найдено.</exception>
        Task<CompetitionReward> GetCompetitionReward(int competitionRewardID);

        /// <summary>
        /// Создать данные о награде соревнования. description.Name и description.Description игнорируются.
        /// </summary>
        /// <param name="description">Обновлённые данные о соревновании. description.Name и description.Description игнорируются.</param>
        /// <exception cref="Exceptions.MissingDataException">Награда с таким ID не найдена.</exception>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task CreateCompetitionReward(CompetitionReward description);

        /// <summary>
        /// Обновить данные о награде соревнования. description.Name и description.Description игнорируются.
        /// </summary>
        /// <param name="description">Обновлённые данные о соревновании. description.Name и description.Description игнорируются.</param>
        /// <exception cref="Exceptions.IncorrectOperationException">Соответствующей награды соревнования не существует.</exception>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task UpdateCompetitionReward(CompetitionReward description);

        /// <summary>
        /// Удалить награду у соревнования.
        /// </summary>
        /// <param name="competitionRewardID">ID соревнования.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        /// <exception cref="Exceptions.MissingDataException">Награда с таким ID не найдена.</exception>
        Task RemoveCompetitionReward(int competitionRewardID);
    }
}
