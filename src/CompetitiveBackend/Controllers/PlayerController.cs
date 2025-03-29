using CompetitiveBackend.Core.Auth;
using CompetitiveBackend.Core.Objects;
using CompetitiveBackend.Repositories;
using CompetitiveBackend.Repositories.Exceptions;
using CompetitiveBackend.Services;
using CompetitiveBackend.Services.Exceptions;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace CompetitiveBackend.Controllers
{
    [ApiController]
    [Produces("application/json")]
    [Route("api/player/")]
    public class PlayerController : ControllerBase
    {
        private IPlayerProfileService _profileService;
        private LinkGenerator _linkGenerator;
        public PlayerController(LinkGenerator generator, IPlayerProfileService service)
        {
            _linkGenerator = generator;
            _profileService = service;
        }
        /// <summary>
        /// Получить информацию о профиле
        /// </summary>
        /// <param name="profileID">Идентификатор игрока</param>
        /// <returns>Данные о профиле</returns>
        [HttpGet("{profileID}/profile")]
        public async Task<IActionResult> GetPlayerProfile(int profileID)
        {
            var uri = HttpContext.Request.GetDisplayUrl();
            SessionToken tok = User.GetSessionToken();
            try 
            { 
                PlayerProfile p = await _profileService.GetPlayerProfile(profileID);
                string? pic = _linkGenerator.GetUriByAction(HttpContext,nameof(GetPlayerImage), "Player", new {profileID});
                return new ObjectResult(new PlayerProfileDto(p.Id!.Value, p.Name, p.Description, pic!));
            }
            catch (MissingDataException e)
            {
                return NotFound(e.Message);
            }
            catch(ServiceException e)
            {
                return BadRequest(e.Message);
            }

        }
        /// <summary>
        /// Получить информацию о изображении
        /// </summary>
        /// <param name="profileID">Идентификатор игрока</param>
        /// <returns></returns>
        [HttpGet("{profileID}/pic")]
        public async Task<IActionResult> GetPlayerImage(int profileID)
        {
            try
            {
                LargeData data = await _profileService.GetPlayerProfileImage(profileID);
                return File(data.Data, "application/octet-stream", "Image");
            }
            catch (MissingDataException e)
            {
                return NotFound(e.Message);
            }
            catch (ServiceException e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
