using CompetitiveBackend.BackendUsage.Objects;
using CompetitiveBackend.BackendUsage.UseCases;
using CompetitiveBackend.Core.Objects;
using Microsoft.AspNetCore.Mvc;

namespace CompetitiveBackend.Controllers
{
    [ApiController]
    [Produces("application/json")]
    [Route($"{APIConsts.ROOTV1}/{APIConsts.PLAYERS}/")]
    public class PlayerRewardsController : ControllerBase
    {
        private IPlayerRewardUseCase _useCase;

        public PlayerRewardsController(IPlayerRewardUseCase useCase)
        {
            _useCase = useCase;
        }

        /// <summary>
        /// Получить информацию о наградах текущего пользователя
        /// </summary>
        /// <param name="page">Индекс страницы</param>
        /// <param name="count">Количество наград на одной странице</param>
        /// <returns>Успешное выполнение</returns>
        /// <response code="200">Успешное выполнение</response>
        /// <response code="401">Пользователь не авторизован</response>
        /// <response code="500">Ошибка сервера</response>
        [ProducesResponseType(typeof(IEnumerable<PlayerRewardDTO>), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(typeof(ProblemDetails), 500)]
        [HttpGet($"{APIConsts.SELF}/{APIConsts.REWARDS}")]
        public async Task<ActionResult<PlayerRewardDTO[]>> GetMyRewards([FromQuery] int page, [FromQuery] int count)
        {
            using var self = await _useCase.Auth(HttpContext);
            return (await self.GetAllMineRewards(new DataLimiterDTO(page, count))).ToArray();
        }

        /// <summary>
        /// Получить информацию о наградах игрока
        /// </summary>
        /// <param name="playerID">Идентификатор игрока</param>
        /// <param name="page">Индекс страницы</param>
        /// <param name="count">Количество наград на одной странице</param>
        /// <returns>Успешное выполнение</returns>
        /// <response code="200">Успешное выполнение</response>
        /// <response code="404">Пользователь с таким ID не найден</response>
        /// <response code="500">Ошибка сервера</response>
        [ProducesResponseType(typeof(IEnumerable<PlayerRewardDTO>), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(typeof(ProblemDetails), 500)]
        [HttpGet($"{{playerID:int}}/{APIConsts.REWARDS}")]
        public async Task<ActionResult<PlayerRewardDTO[]>> GetRewards(int playerID, [FromQuery] int page, [FromQuery] int count)
        {
            using var self = await _useCase.Auth(HttpContext);
            return (await self.GetAllRewardsOf(playerID, new DataLimiterDTO(page, count))).ToArray();
        }

        /// <summary>
        /// Выдать награду игроку
        /// </summary>
        /// <param name="playerID">Идентификатор игрока</param>
        /// <param name="rewardDescriptionID">Идентификатор описания награды</param>
        /// <returns>Успешное выполнение</returns>
        /// <response code="204">Награда успешно выдана</response>
        /// <response code="400">Неверные параметры запроса</response>
        /// <response code="404">Игрок или описание награды не найдены</response>
        /// <response code="500">Ошибка сервера</response>
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(typeof(ProblemDetails), 500)]
        [HttpPut($"{{playerID}}/{APIConsts.REWARDS}")]
        public async Task<NoContentResult> GrantRewardTo(int playerID, int rewardDescriptionID)
        {
            using var self = await _useCase.Auth(HttpContext);
            await self.GrantRewardToPlayer(playerID, rewardDescriptionID);
            return NoContent();
        }

        /// <summary>
        /// Удалить награду у игрока
        /// </summary>
        /// <param name="rewardID">Идентификатор награды</param>
        /// <returns>Успешное выполнение</returns>
        /// <response code="204">Награда успешно удалена</response>
        /// <response code="404">Награда с указанным ID не найдена</response>
        /// <response code="500">Ошибка сервера</response>
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(typeof(ProblemDetails), 500)]
        [HttpDelete($"{{playerID}}/{APIConsts.REWARDS}/{{rewardID}}")]
        public async Task<NoContentResult> RemoveReward(int rewardID)
        {
            using var self = await _useCase.Auth(HttpContext);
            await self.DeleteReward(rewardID);
            return NoContent();
        }
    }
}
