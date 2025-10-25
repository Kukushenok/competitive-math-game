using CompetitiveBackend.BackendUsage.Objects;
using CompetitiveBackend.BackendUsage.UseCases;
using Microsoft.AspNetCore.Mvc;

namespace CompetitiveBackend.Controllers
{
    [Route($"{APIConsts.ROOTV1}/{APIConsts.COMPETITIONS}/")]
    [ApiController]
    public class CompetitionRiddleManagementController : ControllerBase
    {
        private readonly IGameManagementUseCase competitionLevelEditUseCase;
        public CompetitionRiddleManagementController(IGameManagementUseCase gameManagement)
        {
            competitionLevelEditUseCase = gameManagement;
        }

        /// <summary>
        /// Задать правила для выбора загадок.
        /// </summary>
        /// <param name="compID">Идентификатор соревнования.</param>
        /// <param name="putRequest">Запрос на установку правил.</param>
        /// <returns>Результат операции.</returns>
        /// <response code="204">Успешное выполнение.</response>
        /// <response code="404">Соревнование с таким ID не найдено.</response>
        /// <response code="401">Пользователь не авторизован.</response>
        /// <response code="403">Нет прав администратора.</response>
        /// <response code="500">Ошибка сервера.</response>
        [HttpPut("{compID:int}/game_settings")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> PutRiddleSet(
            int compID,
            [FromBody] RiddleGameSettingsDTO putRequest)
        {
            using IGameManagementUseCase self = await competitionLevelEditUseCase.Auth(HttpContext);
            await self.UpdateSettings(compID, putRequest);
            return NoContent();
        }

        /// <summary>
        /// Получить правила для выбора загадок.
        /// </summary>
        /// <param name="compID">Идентификатор соревнования.</param>
        /// <returns>Результат операции.</returns>
        /// <response code="201">Успешное выполнение.</response>
        /// <response code="404">Соревнование с таким ID не найдено.</response>
        /// <response code="401">Пользователь не авторизован.</response>
        /// <response code="403">Нет прав администратора.</response>
        /// <response code="500">Ошибка сервера.</response>
        [HttpGet("{compID:int}/game_settings")]

        [ProducesResponseType(typeof(RiddleGameSettingsDTO), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<RiddleGameSettingsDTO>> GetRiddleSet(
            int compID)
        {
            using IGameManagementUseCase self = await competitionLevelEditUseCase.Auth(HttpContext);
            return await self.GetSettings(compID);
        }

        /// <summary>
        /// Добавить задачу.
        /// </summary>
        /// <param name="compID">Идентификатор соревнования.</param>
        /// <param name="riddleInfo">Данные задачи для добавления.</param>
        /// <returns>Результат операции.</returns>
        /// <response code="200">Успешное выполнение.</response>
        /// <response code="404">Соревнование с таким ID не найдено.</response>
        /// <response code="401">Пользователь не авторизован.</response>
        /// <response code="403">Нет прав администратора.</response>
        /// <response code="500">Ошибка сервера.</response>
        [HttpPost("{compID:int}/riddles")]

        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> AddRiddle(int compID, [FromBody] RiddleInfoDTO riddleInfo)
        {
            using IGameManagementUseCase self = await competitionLevelEditUseCase.Auth(HttpContext);
            riddleInfo.CompetitionID = compID;
            await self.CreateRiddle(riddleInfo);
            return NoContent();
        }

        /// <summary>
        /// Получить весь список задач.
        /// </summary>
        /// <param name="compID">Идентификатор соревнования.</param>
        /// <param name="page">Индекс страницы.</param>
        /// <param name="count">Количество задач на одной странице.</param>
        /// <returns>Результат операции.</returns>
        /// <response code="200">Успешное выполнение.</response>
        /// <response code="404">Соревнование с таким ID не найдено.</response>
        /// <response code="401">Пользователь не авторизован.</response>
        /// <response code="403">Нет прав администратора.</response>
        /// <response code="500">Ошибка сервера.</response>
        [HttpGet("{compID:int}/riddles")]

        [ProducesResponseType(typeof(IEnumerable<RiddleInfoDTO>), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(typeof(ProblemDetails), 500)]
        public async Task<ActionResult<RiddleInfoDTO[]>> GetRiddles(
            int compID,
            [FromQuery] int page = 0,
            [FromQuery] int count = 0)
        {
            using IGameManagementUseCase self = await competitionLevelEditUseCase.Auth(HttpContext);
            return (await self.GetRiddles(compID, new DataLimiterDTO(page, count))).ToArray();
        }

        /// <summary>
        /// Удалить задачу.
        /// </summary>
        /// <param name="compID">Идентификатор соревнования.</param>
        /// <param name="id">Идентификатор задачи.</param>
        /// <returns>Результат операции.</returns>
        /// <response code="204">Успешное выполнение.</response>
        /// <response code="401">Пользователь не авторизован.</response>
        /// <response code="403">Нет прав администратора.</response>
        /// <response code="404">Соревнование или задача с таким ID не найдены.</response>
        /// <response code="500">Ошибка сервера.</response>
        [HttpDelete("{compID:int}/riddles/{id:int}")]

        [ProducesResponseType(204)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> DeleteRiddle(int compID, int id)
        {
            using IGameManagementUseCase self = await competitionLevelEditUseCase.Auth(HttpContext);
            await self.DeleteRiddle(id);
            return NoContent();
        }

        /// <summary>
        /// Обновить задачу.
        /// </summary>
        /// <param name="compID">Идентификатор соревнования.</param>
        /// <param name="id">ID загадки.</param>
        /// <param name="riddleInfo">Данные задачи для обновления.</param>
        /// <returns>Результат операции.</returns>
        /// <response code="204">Успешное выполнение.</response>
        /// <response code="401">Пользователь не авторизован.</response>
        /// <response code="403">Нет прав администратора.</response>
        /// <response code="404">Соревнование или задача с таким ID не найдены.</response>
        /// <response code="500">Ошибка сервера.</response>
        [HttpPut("{compID:int}/riddles/{id:int}")]

        [ProducesResponseType(204)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> UpdateRiddle(int compID, int id, [FromBody] RiddleInfoDTO riddleInfo)
        {
            using IGameManagementUseCase self = await competitionLevelEditUseCase.Auth(HttpContext);
            await self.UpdateRiddle(riddleInfo);
            return NoContent();
        }
    }
}
