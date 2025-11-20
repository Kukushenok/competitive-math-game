using CompetitiveBackend.BackendUsage.Objects;
using CompetitiveBackend.BackendUsage.UseCases;
using CompetitiveBackend.Core.Objects;
using Microsoft.AspNetCore.Mvc;

namespace CompetitiveBackend.Controllers
{
    [ApiController]
    [Produces("application/json")]
    [Route($"{APIConsts.ROOTV1}/{APIConsts.PLAYERS}/")]
    public class PlayerController : ControllerBase
    {
        private readonly IPlayerProfileUseCase profileUseCase;
        public PlayerController(IPlayerProfileUseCase useCase)
        {
            profileUseCase = useCase;
        }

        /// <summary>
        /// Получить информацию о профиле.
        /// </summary>
        /// <param name="profileID">Идентификатор игрока.</param>
        /// <returns>Информация о профиле.</returns>
        /// <response code="200">Success.</response>
        /// <response code="500">Ошибка сервера.</response>
        [HttpGet("{profileID:int}")]
        [ProducesResponseType(typeof(PlayerProfile), 200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<PlayerProfileDTO>> GetPlayerProfile(int profileID)
        {
            return await profileUseCase.GetProfile(profileID);
        }

        /// <summary>
        /// Получить информацию о изображении.
        /// </summary>
        /// <param name="profileID">Идентификатор игрока.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [HttpGet($"{{profileID:int}}/{APIConsts.IMAGE}")]
        public async Task<FileResult> GetPlayerImage(int profileID)
        {
            LargeDataDTO data = await profileUseCase.GetProfileImage(profileID);
            return data.ToFileResult($"player_{profileID}.jpg");
        }
    }
}
