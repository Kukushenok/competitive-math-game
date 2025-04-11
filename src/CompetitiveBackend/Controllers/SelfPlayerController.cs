using CompetitiveBackend.BackendUsage;
using CompetitiveBackend.Core.Auth;
using CompetitiveBackend.Core.Objects;
using CompetitiveBackend.Repositories;
using CompetitiveBackend.Services;
using CompetitiveBackend.Services.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace CompetitiveBackend.Controllers
{
    public record PlayerProfileDto(int id, string Name, string? Description, string ProfileImageLink);
    [ApiController]
    [Produces("application/json")]
    [Route("api/player/")]
    public class SelfPlayerController : ControllerBase
    {
        private ISelfUseCase _selfUseCase;
        private LinkGenerator _linkGenerator;
        public SelfPlayerController(LinkGenerator generator, ISelfUseCase service)
        {
            _selfUseCase = service;
            _linkGenerator = generator;
        }
        /// <summary>
        /// Получить данные своего профиля
        /// </summary>
        /// <returns>Данные профиля</returns>
        [HttpGet("self/profile")]
        public async Task<ActionResult<PlayerProfileDto>> GetPlayerProfile()
        {
            using var self = await _selfUseCase.Auth(HttpContext);
            PlayerProfile p = await self.GetMyProfile();
            string? pic = _linkGenerator.GetUriByAction(HttpContext, nameof(GetPlayerImage), "SelfPlayer");
            return new ObjectResult(new PlayerProfileDto(p.Id!.Value, p.Name, p.Description, pic!));
        }
        /// <summary>
        /// Обновить данные своего профиля
        /// </summary>
        /// <param name="name">Имя</param>
        /// <param name="description">Описание профиля</param>
        /// <returns></returns>
        [HttpPatch("self/profile")]
        public async Task<ActionResult> SetPlayerProfile(string name, string? description)
        {
            using var self = await _selfUseCase.Auth(HttpContext);
            await self.UpdateMyPlayerProfile(new PlayerProfile(name, description));
            return NoContent();
        }
        /// <summary>
        /// Получить изображение своего профиля
        /// </summary>
        /// <returns></returns>
        [HttpGet("self/pic")]
        public async Task<FileResult> GetPlayerImage()
        {
            using var self = await _selfUseCase.Auth(HttpContext);
            LargeData data = await self.GetMyImage();
            return File(data.Data, "application/octet-stream", "Image");
        }
        /// <summary>
        /// Обновить изображение своего профиля
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost("self/pic")]
        public async Task<ActionResult> SetPlayerImage(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded.");
            using var self = await _selfUseCase.Auth(HttpContext);
            using var memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream);
            LargeData data = new LargeData(memoryStream.ToArray());
            await self.UpdateMyImage(data);
            return NoContent();
        }
    }
}
