using CompetitiveBackend.BackendUsage.Objects;
using CompetitiveBackend.BackendUsage.UseCases;
using CompetitiveBackend.Core.Objects;
using CompetitiveBackend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CompetitiveBackend.Controllers
{
    [Route($"{APIConsts.ROOTV1}/{APIConsts.COMPETITIONS}/")]
    [ApiController]
    public class CompetitionController : ControllerBase
    {
        private ICompetitionEditUseCase editUseCase;
        private ICompetitionWatchUseCase watchUseCase;
        public CompetitionController(ICompetitionEditUseCase editUseCase, ICompetitionWatchUseCase watchUseCase)
        {
            this.editUseCase = editUseCase;
            this.watchUseCase = watchUseCase;
        }
        /// <summary>
        /// Информация о конкретном соревновании
        /// </summary>
        /// <param name="id">Идентификатор соревнования</param>
        /// <returns>Успешное получение информации</returns>
        /// <response code="200">Успешное получение информации</response>
        /// <response code="404">Соревнование не найдено</response>
        /// <response code="500">Ошибка сервера</response>
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(CompetitionDTO), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(typeof(ProblemDetails), 500)]
        public async Task<ActionResult<CompetitionDTO>> GetCompetition(int id)
        {
            return await watchUseCase.GetCompetition(id);
        }

        /// <summary>
        /// Получить информацию о множестве соревнований
        /// </summary>
        /// <param name="page">Индекс страницы</param>
        /// <param name="count">Количество соревнований на одной странице</param>
        /// <param name="filter">Выбрать только те соревнования, которые на данный момент времени активны</param>
        /// <returns>Успешное выполнение</returns>
        /// <response code="200">Успешное выполнение</response>
        /// <response code="500">Ошибка сервера</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<CompetitionDTO>), 200)]
        [ProducesResponseType(typeof(ProblemDetails), 500)]
        public async Task<ActionResult<CompetitionDTO[]>> GetAllCompetitions([FromQuery] int page, [FromQuery] int count, [FromQuery] string? filter = null)
        {
            CompetitionDTO[] dtos;
            if (filter == "active") dtos = (await watchUseCase.GetActiveCompetitions()).ToArray();
            else dtos = (await watchUseCase.GetAllCompetitions(new DataLimiterDTO(page, count))).ToArray();
            return dtos;
        }
        /// <summary>
        /// Добавить новое соревнование
        /// </summary>
        /// <returns>Успешное выполнение</returns>
        /// <response code="204">Успешное выполнение</response>
        /// <response code="400">Некорректные данные запроса</response>
        /// <response code="401">Пользователь не авторизован</response>
        /// <response code="403">Пользователь не имеет прав администратора</response>
        /// <response code="500">Ошибка сервера</response>
        [HttpPost]

        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(typeof(ProblemDetails), 500)]
        public async Task<NoContentResult> CreateCompetition(CompetitionDTO dto)
        {
            using var self = await editUseCase.Auth(HttpContext);
            await self.CreateCompetition(dto);
            return NoContent();
        }
        /// <summary>
        /// Частичное обновление соревнования
        /// </summary>
        /// <param name="id">Идентификатор соревнования</param>
        /// <param name="patchRequest">Описание частичного изменения соревнования</param>
        /// <returns>Успешное изменение</returns>
        /// <response code="204">Успешное изменение</response>
        /// <response code="400">Некорректные данные запроса</response>
        /// <response code="500">Ошибка сервера</response>
        [HttpPatch("{id:int}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(typeof(ProblemDetails), 500)]
        public async Task<NoContentResult> UpdateCompetition(int id, [FromBody] CompetitionPatchRequestDTO patchRequest)
        {
            using var self = await editUseCase.Auth(HttpContext);
            patchRequest.ID = id;
            await self.UpdateCompetition(patchRequest);
            return NoContent();
        }
    }
}
