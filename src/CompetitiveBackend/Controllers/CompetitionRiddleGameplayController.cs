using CompetitiveBackend.BackendUsage.Objects;
using CompetitiveBackend.BackendUsage.UseCases;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CompetitiveBackend.Controllers
{
    [Route($"{APIConsts.ROOTV1}/{APIConsts.COMPETITIONS}/")]
    [ApiController]
    public class CompetitionRiddleGameplayController: ControllerBase
    {
        private IGamePlayUseCase gamePlay;
        public CompetitionRiddleGameplayController(IGamePlayUseCase gamePlay)
        {
            this.gamePlay = gamePlay;
        }
        /// <summary>
        /// Получить задание на игру
        /// </summary>
        /// <param name="compID">Идентификатор соревнования</param>
        /// <returns>Успешное выполнение</returns>
        /// <response code="200">Успешное выполнение</response>
        /// <response code="404">Соревнование с таким ID не найдено</response>
        /// <response code="401">Пользователь не авторизован</response>
        /// <response code="403">Не является игроком</response>
        /// <response code="500">Ошибка сервера</response>
        [HttpGet("{compID:int}/game_session")]
        [ProducesResponseType(typeof(CompetitionParticipationTaskDTO), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<CompetitionParticipationTaskDTO>> PutRiddleSet(
            int compID)
        {
            using var self = await gamePlay.Auth(HttpContext);
            return await self.DoPlay(compID);
        }
        /// <summary>
        /// Выполнить задание на игру
        /// </summary>
        /// <param name="compID">Идентификатор соревнования</param>
        /// <param name="session">Идентификатор игровой сессии</param>
        /// <param name="answers">Ответы на вопросы</param>
        /// <returns>Успешное выполнение</returns>
        /// <response code="200">Успешное выполнение</response>
        /// <response code="404">Соревнование с таким ID не найдено</response>
        /// <response code="401">Пользователь не авторизован</response>
        /// <response code="403">Не игрок</response>
        /// <response code="500">Ошибка сервера</response>
        [HttpPost("{compID:int}/game_session/{session:guid}")]

        [ProducesResponseType(typeof(ParticipationFeedbackDTO), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<ParticipationFeedbackDTO>> DoPlay(
            int compID,
            string session,
            [FromBody] List<RiddleAnswerDTO> answers)
        {
            using var self = await gamePlay.Auth(HttpContext);
            return await self.DoSubmit(new CompetitionParticipationRequestDTO(session, answers));
        }
    }
}
