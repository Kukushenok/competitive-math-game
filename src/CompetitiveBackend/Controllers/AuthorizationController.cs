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
    [Route($"{APIConsts.ROOTV1}/auth/[action]")]
    [ApiController]
    public class AuthorizationController : ControllerBase
    {
        private IAuthUseCase AuthUseCase;
        public AuthorizationController(IAuthUseCase service)
        {
            AuthUseCase = service;
        }
        [HttpPost(Name = "login")]
        public async Task<IActionResult> Login(AccountLoginDTO dto)
        {
            return Ok(await AuthUseCase.Login(dto));
        }
        [HttpPost(Name = "register")]
        public async Task<ActionResult<AuthSuccessResultDTO>> Register(AccountCreationDTO dto)
        {
            return Ok(await AuthUseCase.Register(dto));
        }
    }
}
