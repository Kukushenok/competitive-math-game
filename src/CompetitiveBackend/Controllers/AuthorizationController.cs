using CompetitiveBackend.BackendUsage.Objects;
using CompetitiveBackend.BackendUsage.UseCases;
using CompetitiveBackend.Repositories.Exceptions;
using CompetitiveBackend.Services;
using CompetitiveBackend.Services.Exceptions;
using CompetitiveBackend.Services.Objects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CompetitiveBackend.Controllers
{
    [Route($"{APIConsts.ROOTV1}/{APIConsts.AUTH}/")]
    [ApiController]
    public class AuthorizationController : ControllerBase
    {
        private IAuthUseCase AuthUseCase;
        public AuthorizationController(IAuthUseCase service)
        {
            AuthUseCase = service;
        }
        /// <summary>
        /// Login request
        /// </summary>
        /// <param name="dto">Data for login request</param>
        /// <response code="200">Returns the information about session</response>
        /// <response code="404">Could not find the user and password</response>
        [HttpPost(APIConsts.LOGIN)]
        [ProducesResponseType(typeof(AuthSuccessResultDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Login(AccountLoginDTO dto)
        {
            return Ok(await AuthUseCase.Login(dto));
        }
        [HttpPost(APIConsts.REGISTER)]
        [ProducesResponseType(typeof(AuthSuccessResultDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<AuthSuccessResultDTO>> Register(AccountCreationDTO dto)
        {
            return Ok(await AuthUseCase.Register(dto));
        }
    }
}
