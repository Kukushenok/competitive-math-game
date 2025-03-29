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
        private IPlayerProfileService _profileService;
        private LinkGenerator _linkGenerator;
        public SelfPlayerController(LinkGenerator generator, IPlayerProfileService service)
        {
            _profileService = service;
            _linkGenerator = generator;
        }
        /// <summary>
        /// Получить данные своего профиля
        /// </summary>
        /// <returns>Данные профиля</returns>
        [HttpGet("self/profile")]
        public async Task<IActionResult> GetPlayerProfile()
        {
            SessionToken tok = User.GetSessionToken();
            if (tok.TryGetAccountIdentifier(out int identifier) && tok.Role.IsPlayer())
            {
                PlayerProfile p = await _profileService.GetPlayerProfile(identifier);
                string? pic = _linkGenerator.GetUriByAction(HttpContext,nameof(GetPlayerImage), "SelfPlayer");
                return new ObjectResult(new PlayerProfileDto(p.Id!.Value, p.Name, p.Description, pic!));
            }
            return Forbid();
        }
        /// <summary>
        /// Обновить данные своего профиля
        /// </summary>
        /// <param name="name">Имя</param>
        /// <param name="description">Описание профиля</param>
        /// <returns></returns>
        [HttpPost("self/profile")]
        public async Task<IActionResult> SetPlayerProfile(string name, string? description)
        {
            SessionToken tok = User.GetSessionToken();
            if (tok.TryGetAccountIdentifier(out int identifier) && tok.Role.IsPlayer())
            {
                try
                {
                    await _profileService.UpdatePlayerProfile(new PlayerProfile(name, description, identifier));
                }
                catch (ServiceException e)
                {
                    return BadRequest(e.Message);
                }
                return Ok();
            }
            return Forbid();
        }
        /// <summary>
        /// Получить изображение своего профиля
        /// </summary>
        /// <returns></returns>
        [HttpGet("self/pic")]
        public async Task<IActionResult> GetPlayerImage()
        {
            SessionToken tok = User.GetSessionToken();
            if (tok.TryGetAccountIdentifier(out int identifier) && tok.Role.IsPlayer())
            {
                try
                {
                    LargeData data = await _profileService.GetPlayerProfileImage(identifier);
                    return File(data.Data, "application/octet-stream", "Image");
                }
                catch (ServiceException e)
                {
                    return BadRequest(e.Message);
                }
            }
            return Forbid();
        }
        /// <summary>
        /// Обновить изображение своего профиля
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost("self/pic")]
        public async Task<IActionResult> SetPlayerImage(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded.");
            SessionToken tok = User.GetSessionToken();
            if (tok.TryGetAccountIdentifier(out int identifier) && tok.Role.IsPlayer())
            {
                using var memoryStream = new MemoryStream();
                await file.CopyToAsync(memoryStream);
                LargeData data = new LargeData(memoryStream.ToArray());
                try
                {
                    await _profileService.SetPlayerProfileImage(identifier, data);
                    return Ok();
                }
                catch (ServiceException e)
                {
                    return BadRequest(e.Message);
                }
            }
            return Forbid();
        }
    }
}
