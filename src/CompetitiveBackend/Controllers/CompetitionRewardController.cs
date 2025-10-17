using CompetitiveBackend.BackendUsage.Objects;
using CompetitiveBackend.BackendUsage.UseCases;
using CompetitiveBackend.Core.Objects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CompetitiveBackend.Controllers
{
    [Route($"{APIConsts.ROOTV1}/{APIConsts.COMPETITIONS}/")]
    [ApiController]
    public class CompetitionRewardController: ControllerBase
    {
        private ICompetitionRewardEditUseCase _editRewards;
        private ICompetitionWatchUseCase _watchRewards;
        public CompetitionRewardController(ICompetitionRewardEditUseCase editRewards, ICompetitionWatchUseCase service)
        {
            _editRewards = editRewards;
            _watchRewards = service;
        }
        /// <summary>
        /// Получить информацию о выдаваемых наградах
        /// </summary>
        /// <param name="compID">Идентификатор соревнования</param>
        /// <returns>Успешное выполнение</returns>
        /// <response code="200">Успешное выполнение</response>
        /// <response code="404">Соревнование с таким ID не найдено</response>
        /// <response code="500">Ошибка сервера</response>
        [HttpGet($"{{compID:int}}/{APIConsts.REWARDS}")]
        [ProducesResponseType(typeof(IEnumerable<CompetitionRewardDTO>), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IEnumerable<CompetitionRewardDTO>> GetCompetitionRewards(int compID)
        {
            return await _watchRewards.GetRewardsFor(compID);
        }

        /// <summary>
        /// Добавить выдаваемую награду
        /// </summary>
        /// <param name="compID">Идентификатор соревнования</param>
        /// <param name="rewardDTO">Описание выдаваемой награды</param>
        /// <returns>Успешное добавление</returns>
        /// <response code="200">Успешное добавление</response>
        /// <response code="401">Пользователь не авторизован</response>
        /// <response code="403">Пользователь не авторизован как администратор</response>
        /// <response code="500">Ошибка сервера</response>
        [HttpPost($"{{compID}}/{APIConsts.REWARDS}")]

        [ProducesResponseType(typeof(CompetitionReward), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(typeof(ProblemDetails), 500)]
        public async Task<NoContentResult> CreateCompetitionReward(int compID, [FromBody] UpdateCompetitionRewardDTO rewardDTO)
        {
            CreateCompetitionRewardDTO creating = new CreateCompetitionRewardDTO(null, rewardDTO.RewardDescriptionID, compID, rewardDTO.ConditionByRank, rewardDTO.ConditionByPlace);
            using var self = await _editRewards.Auth(HttpContext);
            await self.CreateCompetitionReward(creating);
            return NoContent();
        }
        /// <summary>
        /// Удалить выдаваемую награду
        /// </summary>
        /// <param name="compID">Идентификатор соревнования</param>
        /// <param name="rewardID">Идентификатор выдаваемой награды</param>
        /// <returns>Успешное удаление</returns>
        /// <response code="204">Успешное удаление</response>
        /// <response code="401">Пользователь не авторизован</response>
        /// <response code="403">Пользователь не авторизован как администратор</response>
        /// <response code="404">Награда или соревнование с таким ID не найдены</response>
        /// <response code="500">Ошибка сервера</response>
        [HttpDelete($"{{compID:int}}/{APIConsts.REWARDS}/{{rewardID:int}}")]

        [ProducesResponseType(204)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        [ProducesResponseType(typeof(ProblemDetails), 500)]
        public async Task<NoContentResult> DeleteCompetitionReward(int compID, int rewardID)
        {
            using var self = await _editRewards.Auth(HttpContext);
            await self.RemoveCompetitionReward(rewardID);
            return NoContent();
        }
        /// <summary>
        /// Обновить выдаваемую награду
        /// </summary>
        /// <param name="compID">Идентификатор соревнования</param>
        /// <param name="rewardID">Идентификатор выдаваемой награды</param>
        /// <returns>Успешное обновление</returns>
        /// <response code="204">Успешное удаление</response>
        /// <response code="401">Пользователь не авторизован</response>
        /// <response code="403">Пользователь не авторизован как администратор</response>
        /// <response code="404">Награда или соревнование с таким ID не найдены</response>
        /// <response code="500">Ошибка сервера</response>
        [ProducesResponseType(204)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        [HttpPatch($"{{compID}}/{APIConsts.REWARDS}/{{rewardID}}")]
        public async Task<NoContentResult> UpdateCompetitionReward(int compID, int rewardID, UpdateCompetitionRewardDTO rewardDTO)
        {
            UpdateCompetitionRewardDTO dto = new UpdateCompetitionRewardDTO(rewardID, rewardDTO.RewardDescriptionID, rewardDTO.ConditionByRank, rewardDTO.ConditionByPlace);
            using var self = await _editRewards.Auth(HttpContext);
            await self.UpdateCompetitionReward(dto);
            return NoContent();
        }
    }
}
