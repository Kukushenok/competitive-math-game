using CompetitiveBackend.BackendUsage;
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
    [Route($"{APIConsts.ROOTV}/player/")]
    public class PlayerController : ControllerBase
    {
        private IPlayerProfileUseCase _profileUseCase;
        private LinkGenerator _linkGenerator;
        public PlayerController(LinkGenerator generator, IPlayerProfileUseCase useCase)
        {
            _linkGenerator = generator;
            _profileUseCase = useCase;
        }
        /// <summary>
        /// Получить информацию о профиле
        /// </summary>
        /// <param name="profileID">Идентификатор игрока</param>
        /// <returns>Данные о профиле</returns>
        [HttpGet("{profileID}/profile")]
        public async Task<ActionResult<PlayerProfileDto>> GetPlayerProfile(int profileID)
        {
            PlayerProfile p = await _profileUseCase.GetProfile(profileID);
            string? pic = _linkGenerator.GetUriByAction(HttpContext, nameof(GetPlayerImage), "Player", new { profileID });
            return new ObjectResult(new PlayerProfileDto(p.Id!.Value, p.Name, p.Description, pic!));
        }
        /// <summary>
        /// Получить информацию о изображении
        /// </summary>
        /// <param name="profileID">Идентификатор игрока</param>
        /// <returns></returns>
        [HttpGet("{profileID}/pic")]
        public async Task<FileResult> GetPlayerImage(int profileID)
        {
            LargeData data = await _profileUseCase.GetProfileImage(profileID);
            return File(data.Data, "application/octet-stream", "Image.jpg");
        }
    }
}
