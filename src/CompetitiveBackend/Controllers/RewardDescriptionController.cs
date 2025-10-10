using CompetitiveBackend.BackendUsage.Objects;
using CompetitiveBackend.BackendUsage.UseCases;
using CompetitiveBackend.Core.Objects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CompetitiveBackend.Controllers
{
    [Route($"{APIConsts.ROOTV1}/{APIConsts.REWARD_DESCRIPTIONS}/")]
    [ApiController]
    public class RewardDescriptionController : ControllerBase
    {
        private IRewardDescriptionEditUseCase editUseCase;
        private IRewardDescriptionWatchUseCase watchUseCase;
        public RewardDescriptionController(IRewardDescriptionEditUseCase editUseCase, IRewardDescriptionWatchUseCase watchUseCase)
        {
            this.editUseCase = editUseCase;
            this.watchUseCase = watchUseCase;
        }

        /// <summary>
        /// Получить описание награды
        /// </summary>
        /// <param name="id">Идентификатор описания награды</param>
        /// <returns>Успешное выполнение</returns>
        /// <response code="200">Успешное выполнение</response>
        /// <response code="500">Ошибка сервера</response>
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(RewardDescriptionDTO), 200)]
        [ProducesResponseType(typeof(ProblemDetails), 500)]
        public async Task<ActionResult<RewardDescriptionDTO>> GetRewardDescription(int id)
        {
            return await watchUseCase.GetRewardDescription(id);
        }

        /// <summary>
        /// Получить список всех описаний наград
        /// </summary>
        /// <param name="page">Индекс страницы</param>
        /// <param name="count">Количество наград на одной странице</param>
        /// <returns>Успешное выполнение</returns>
        /// <response code="200">Успешное выполнение</response>
        /// <response code="401">Пользователь не авторизован</response>
        /// <response code="403">Пользователь не авторизован как администратор</response>
        /// <response code="500">Ошибка сервера</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<RewardDescriptionDTO>), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(typeof(ProblemDetails), 500)]
        public async Task<ActionResult<RewardDescriptionDTO[]>> GetAllRewardDescriptions(int page, int count)
        {
            return (await watchUseCase.GetAllRewardDescriptions(new DataLimiterDTO(page, count))).ToArray();
        }

        /// <summary>
        /// Получить иконку награды
        /// </summary>
        /// <param name="id">Идентификатор награды</param>
        /// <returns>Файл иконки</returns>
        /// <response code="200">Успешное выполнение</response>
        /// <response code="404">Награда или иконка не найдены</response>
        /// <response code="500">Ошибка сервера</response>
        [HttpGet($"{{id}}/{APIConsts.IMAGE}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(typeof(ProblemDetails), 500)]
        public async Task<FileContentResult> GetRewardIcon(int id)
        {
            return (await watchUseCase.GetRewardIcon(id)).ToFileResult($"reward_{id}.jpg");
        }

        //[HttpGet($"{{id}}/{APIConsts.ASSET}")]
        //public async Task<FileContentResult> GetRewardGameAsset(int id)
        //{
        //    return (await watchUseCase.GetRewardGameAsset(id)).ToFileResult($"reward_{id}_asset.bytes");
        //}

        /// <summary>
        /// Создать новое описание награды
        /// </summary>
        /// <param name="dto">Данные для создания награды</param>
        /// <returns>Успешное выполнение</returns>
        /// <response code="204">Награда успешно создана</response>
        /// <response code="400">Неверные данные запроса</response>
        /// <response code="401">Пользователь не авторизован</response>
        /// <response code="403">Пользователь не авторизован как администратор</response>
        /// <response code="500">Ошибка сервера</response>
        [HttpPost("")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(typeof(ProblemDetails), 500)]
        public async Task<NoContentResult> CreateReward(RewardDescriptionDTO dto)
        {
            using var self = await editUseCase.Auth(HttpContext);
            await self.CreateRewardDescription(dto);
            return NoContent();
        }

        /// <summary>
        /// Обновить описание награды
        /// </summary>
        /// <param name="dto">Обновленные данные награды</param>
        /// <returns>Успешное выполнение</returns>
        /// <response code="204">Награда успешно обновлена</response>
        /// <response code="400">Неверные данные запроса</response>
        /// <response code="401">Пользователь не авторизован</response>
        /// <response code="403">Пользователь не авторизован как администратор</response>
        /// <response code="404">Награда не найдена</response>
        /// <response code="500">Ошибка сервера</response>
        [HttpPut("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        [ProducesResponseType(typeof(ProblemDetails), 500)]
        public async Task<NoContentResult> UpdateRewardDescription(RewardDescriptionDTO dto)
        {
            using var self = await editUseCase.Auth(HttpContext);
            await self.UpdateRewardDescription(dto);
            return NoContent();
        }

        /// <summary>
        /// Обновить иконку награды
        /// </summary>
        /// <param name="id">Идентификатор награды</param>
        /// <param name="file">Файл иконки</param>
        /// <returns>Успешное выполнение</returns>
        /// <response code="204">Иконка успешно обновлена</response>
        /// <response code="400">Неверный файл</response>
        /// <response code="401">Пользователь не авторизован</response>
        /// <response code="403">Пользователь не авторизован как администратор</response>
        /// <response code="404">Награда не найдена</response>
        /// <response code="500">Ошибка сервера</response>
        [HttpPut($"{{id}}/{APIConsts.IMAGE}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        [ProducesResponseType(typeof(ProblemDetails), 500)]
        public async Task<NoContentResult> UpdateRewardDescriptionIcon(int id, IFormFile file)
        {
            using var self = await editUseCase.Auth(HttpContext);
            await self.SetRewardIcon(id, await file.ToLargeData());
            return NoContent();
        }

        //[HttpPatch($"{{id}}/{APIConsts.ASSET}")]
        //public async Task<ActionResult> UpdateRewardGameAsset(int id, IFormFile file)
        //{
        //    using var self = await editUseCase.Auth(HttpContext);
        //    await self.SetRewardGameAsset(id, await file.ToLargeData());
        //    return NoContent();
        //}
    }
}
