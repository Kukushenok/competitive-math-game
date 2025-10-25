using CompetitiveBackend.BackendUsage.Objects;
using CompetitiveBackend.BackendUsage.UseCases;
using Microsoft.AspNetCore.Mvc;

namespace CompetitiveBackend.Controllers
{
    [Route($"{APIConsts.ROOTV1}/{APIConsts.COMPETITIONS}/")]
    [ApiController]
    public class CompetitionRiddleGameplayController : ControllerBase
    {
        private readonly IGamePlayUseCase gamePlay;
        public CompetitionRiddleGameplayController(IGamePlayUseCase gamePlay)
        {
            this.gamePlay = gamePlay;
        }

        /// <summary>
        /// Получить задание на игру.
        /// </summary>
        /// <param name="compID">Идентификатор соревнования.</param>
        /// <returns>Результат операции.</returns>
        /// <response code="200">Успешное выполнение.</response>
        /// <response code="404">Соревнование с таким ID не найдено.</response>
        /// <response code="401">Пользователь не авторизован.</response>
        /// <response code="403">Не является игроком.</response>
        /// <response code="500">Ошибка сервера.</response>
        [HttpGet("{compID:int}/game_session")]
        [ProducesResponseType(typeof(CompetitionParticipationTaskDTO), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<CompetitionParticipationTaskDTO>> PutRiddleSet(
            int compID)
        {
            using IGamePlayUseCase self = await gamePlay.Auth(HttpContext);
            return await self.DoPlay(compID);
        }

        /// <summary>
        /// Выполнить задание на игру.
        /// </summary>
        /// <param name="compID">Идентификатор соревнования.</param>
        /// <param name="answers">Ответы на вопросы.</param>
        /// <returns>Результат операции.</returns>
        /// <response code="200">Успешное выполнение.</response>
        /// <response code="404">Соревнование с таким ID не найдено.</response>
        /// <response code="401">Пользователь не авторизован.</response>
        /// <response code="403">Не игрок.</response>
        /// <response code="500">Ошибка сервера.</response>
        [HttpPost("{compID:int}/game_session/")]

        [ProducesResponseType(typeof(ParticipationFeedbackDTO), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<ParticipationFeedbackDTO>> DoPlay(
            int compID,
            [FromBody] CompetitionParticipationRequestDTO answers)
        {
            using IGamePlayUseCase self = await gamePlay.Auth(HttpContext);
            return await self.DoSubmit(answers);
        }
    }
}
