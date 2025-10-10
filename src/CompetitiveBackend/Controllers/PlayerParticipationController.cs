using CompetitiveBackend.BackendUsage.Objects;
using CompetitiveBackend.BackendUsage.UseCases;
using CompetitiveBackend.Core.Objects;
using Microsoft.AspNetCore.Mvc;

namespace CompetitiveBackend.Controllers
{
    [ApiController]
    [Produces("application/json")]
    [Route($"{APIConsts.ROOTV1}/")]
    public class PlayerParticipationController : ControllerBase
    {
        private IPlayerParticipationWatchUseCase _watchUseCase;
        private IPlayerParticipationUseCase _editUseCase;
        public PlayerParticipationController(IPlayerParticipationWatchUseCase watchUseCase, IPlayerParticipationUseCase useCase)
        {
            _editUseCase = useCase;
            _watchUseCase = watchUseCase;
        }

        /// <summary>
        /// Получить таблицу лидеров соревнования
        /// </summary>
        /// <param name="compID">Идентификатор соревнования</param>
        /// <param name="page">Индекс страницы</param>
        /// <param name="count">Количество соревнований на одной странице</param>
        /// <returns>Успешное выполнение</returns>
        /// <response code="200">Успешное выполнение</response>
        /// <response code="404">Соревнование с таким ID не найдено</response>
        /// <response code="500">Ошибка сервера</response>
        [ProducesResponseType(typeof(IEnumerable<PlayerParticipationDTO>), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(typeof(ProblemDetails), 500)]
        [HttpGet($"{APIConsts.COMPETITIONS}/{{compID:int}}/{APIConsts.COMP_PARTICIPATIONS}")]
        public async Task<ActionResult<PlayerParticipationDTO[]>> GetLeaderboard(int compID, [FromQuery] int page = 0, [FromQuery] int count = 0)
        {
            return (await _watchUseCase.GetLeaderboard(compID, new DataLimiterDTO(page, count))).ToArray();
        }

        /// <summary>
        /// Получить полную информации об участии в соревновании
        /// </summary>
        /// <param name="compID">Идентификатор соревнования</param>
        /// <param name="profileID">Идентификатор профиля</param>
        /// <returns>Успешное выполнение</returns>
        /// <response code="200">Успешное выполнение</response>
        /// <response code="404">Участие пользователя profileID в соревновании compID не найдено</response>
        /// <response code="500">Ошибка сервера</response>
        [ProducesResponseType(typeof(PlayerParticipation), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(typeof(ProblemDetails), 500)]
        [HttpGet($"{APIConsts.COMPETITIONS}/{{compID}}/{APIConsts.COMP_PARTICIPATIONS}/{{profileID}}")]
        public async Task<ActionResult<PlayerParticipationDTO>> GetParticipationInfo(int profileID, int compID)
        {
            return (await _watchUseCase.GetParticipation(compID, profileID));
        }

        /// <summary>
        /// Удалить участие игрока в соревновании
        /// </summary>
        /// <param name="profileID">Идентификатор профиля игрока</param>
        /// <param name="compID">Идентификатор соревнования</param>
        /// <returns>Успешное выполнение</returns>
        /// <response code="204">Участие успешно удалено</response>
        /// <response code="404">Участие не найдено</response>
        /// <response code="500">Ошибка сервера</response>
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(typeof(ProblemDetails), 500)]
        [HttpDelete($"{APIConsts.COMPETITIONS}/{{compID}}/{APIConsts.COMP_PARTICIPATIONS}/{{profileID}}")]
        public async Task<NoContentResult> DeleteParticipation(int profileID, int compID)
        {
            using var self = await _editUseCase.Auth(HttpContext);
            await self.DeleteParticipation(compID, profileID);
            return NoContent();
        }

        /// <summary>
        /// Получить список участий текущего пользователя в соревнованиях
        /// </summary>
        /// <param name="page">Индекс страницы</param>
        /// <param name="count">Количество участий на одной странице</param>
        /// <returns>Успешное выполнение</returns>
        /// <response code="200">Успешное выполнение</response>
        /// <response code="401">Пользователь не авторизован</response>
        /// <response code="500">Ошибка сервера</response>
        [ProducesResponseType(typeof(IEnumerable<PlayerParticipationDTO>), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(typeof(ProblemDetails), 500)]
        [HttpGet($"{APIConsts.PLAYERS}/{APIConsts.SELF}/{APIConsts.PLAYER_PARTICIPATIONS}")]
        public async Task<ActionResult<PlayerParticipationDTO[]>> GetMyParticipations(int page = 0, int count = 0)
        {
            using var self = await _editUseCase.Auth(HttpContext);
            return (await self.GetMyParticipations(new DataLimiterDTO(page, count))).ToArray();
        }
    }
}
