using CompetitiveBackend.BackendUsage;
using CompetitiveBackend.BackendUsage.Objects;
using CompetitiveBackend.Repositories.Exceptions;
using CompetitiveBackend.Services;
using CompetitiveBackend.Services.Exceptions;
using CompetitiveBackend.Services.Objects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CompetitiveBackend.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthorizationController : ControllerBase
    {
        private IAuthUseCase AuthUseCase;
        public AuthorizationController(IAuthUseCase service)
        {
            AuthUseCase = service;
        }
        [HttpPost(Name = "login")]
        public async Task<IActionResult> Login(string login, string password)
        {
            AuthSuccessResultDTO result = await AuthUseCase.Login(login, password);
            return Ok(result);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="login"></param>
        /// <param name="password"></param>
        /// <response code=200>Account's registered successfully</response>
        /// <response code=403>Could not register account</response>
        /// <returns></returns>
        [HttpPost(Name = "register")]
        public async Task<ActionResult<AuthSuccessResultDTO>> Register(string login, string password, string? email = null)
        {
            AuthSuccessResultDTO dto = await AuthUseCase.Register(new AccountCreationDTO(login, password, email));
            return Ok(dto);
        }
    }
}
