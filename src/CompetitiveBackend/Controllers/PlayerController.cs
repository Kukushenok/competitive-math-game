using CompetitiveBackend.BackendUsage.Objects;
using CompetitiveBackend.BackendUsage.UseCases;
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
    [Route($"{APIConsts.ROOTV1}/{APIConsts.PLAYERS}/")]
    public class PlayerController : ControllerBase
    {
        private IPlayerProfileUseCase _profileUseCase;
        public PlayerController(IPlayerProfileUseCase useCase)
        {
            _profileUseCase = useCase;
        }
        /// <summary>
        /// Получить информацию о профиле
        /// </summary>
        /// <param name="profileID">Идентификатор игрока</param>
        /// <returns>Данные о профиле</returns>
        [HttpGet("{profileID}")]
        public async Task<ActionResult<PlayerProfileDTO>> GetPlayerProfile(int profileID)
        {
            return await _profileUseCase.GetProfile(profileID);
        }
        /// <summary>
        /// Получить информацию о изображении
        /// </summary>
        /// <param name="profileID">Идентификатор игрока</param>
        /// <returns></returns>
        [HttpGet($"{{profileID}}/{APIConsts.IMAGE}")]
        public async Task<FileResult> GetPlayerImage(int profileID)
        {
            var data = await _profileUseCase.GetProfileImage(profileID);
            return data.ToFileResult($"player_{profileID}.jpg");
        }
    }
}
