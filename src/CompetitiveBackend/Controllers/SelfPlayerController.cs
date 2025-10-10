using CompetitiveBackend.BackendUsage.Exceptions;
using CompetitiveBackend.BackendUsage.Objects;
using CompetitiveBackend.BackendUsage.UseCases;
using CompetitiveBackend.Core.Auth;
using CompetitiveBackend.Core.Objects;
using CompetitiveBackend.Repositories;
using CompetitiveBackend.Services;
using CompetitiveBackend.Services.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CompetitiveBackend.Controllers
{
    [ApiController]
    [Produces("application/json")]
    [Route($"{APIConsts.ROOTV1}/{APIConsts.PLAYERS}/")]
    public class SelfPlayerController : ControllerBase
    {
        private ISelfUseCase _selfUseCase;
        public SelfPlayerController(ISelfUseCase service)
        {
            _selfUseCase = service;
        }
        /// <summary>
        /// Получить данные своего профиля
        /// </summary>
        /// <returns>Успешное выполнение</returns>
        /// <response code="200">Успешное выполнение</response>
        /// <response code="401">Пользователь не авторизован</response>
        /// <response code="403">Пользователь не авторизован как игрок</response>
        /// <response code="500">Ошибка сервера</response>
        [Authorize(Roles = "Player")]
        [ProducesResponseType(typeof(PlayerProfile), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(typeof(ProblemDetails), 500)]
        [HttpGet($"{APIConsts.SELF}")]
        public async Task<ActionResult<PlayerProfileDTO>> GetPlayerProfile()
        {
            using var self = await _selfUseCase.Auth(HttpContext);
            PlayerProfileDTO p = await self.GetMyProfile();
            return p;
        }
        /// <summary>
        /// Обновить данные своего профиля
        /// </summary>
        /// <param name="name">Имя</param>
        /// <param name="description">Описание профиля</param>
        /// <returns></returns>
        [HttpPatch($"{APIConsts.SELF}")]
        public async Task<NoContentResult> SetPlayerProfile(PlayerProfileDTO dto)
        {
            using var self = await _selfUseCase.Auth(HttpContext);
            await self.UpdateMyPlayerProfile(dto);
            return NoContent();
        }
        /// <summary>
        /// Получить изображение своего профиля
        /// </summary>
        /// <returns></returns>
        [HttpGet($"{APIConsts.SELF}/image")]
        public async Task<FileResult> GetPlayerImage()
        {
            using var self = await _selfUseCase.Auth(HttpContext);
            var data = await self.GetMyImage();
            return (await self.GetMyImage()).ToFileResult("self_image.jpg");
        }
        /// <summary>
        /// Обновить изображение своего профиля
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPut($"{APIConsts.SELF}/image")]
        public async Task<NoContentResult> SetPlayerImage(IFormFile file)
        {
            using var self = await _selfUseCase.Auth(HttpContext);
            await self.UpdateMyImage(await file.ToLargeData());
            return NoContent();
        }
    }
    public static class IFormFileConverter
    {
        public static async Task<LargeDataDTO> ToLargeData(this IFormFile file)
        {
            if (file == null || file.Length == 0)
                throw new RequestFailedException("File is null");
            using var memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream);
            var data = new LargeDataDTO(memoryStream.ToArray());
            return data;
        }
        public static FileContentResult ToFileResult(this LargeDataDTO dto, string downloadName = "Data")
        {
            var res = new FileContentResult(dto.Data ?? Array.Empty<byte>(), "application/octet-stream");
            res.FileDownloadName = downloadName;
            return res;
        }
    }
}
