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
        /// Логин
        /// </summary>
        /// <param name="accountLogin">Данные для входа в аккаунт</param>
        /// <returns>Успешный вход в аккаунт</returns>
        /// <response code="200">Успешный вход в аккаунт</response>
        /// <response code="404">Пароль не подошёл или логин не найден</response>
        /// <response code="500">Ошибка сервера</response>
        [HttpPost(APIConsts.LOGIN)]
        [ProducesResponseType(typeof(AuthSuccessResult), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(typeof(ProblemDetails), 500)]
        public async Task<IActionResult> Login([FromBody] AccountLoginDTO accountLogin)
        {
            return Ok(await AuthUseCase.Login(accountLogin));
        }
        /// <summary>
        /// Регистрация нового аккаунта
        /// </summary>
        /// <param name="accountCreation">Данные для регистрации</param>
        /// <returns>Успешная регистрация</returns>
        /// <response code="200">Успешная регистрация</response>
        /// <response code="409">Такой логин уже занят</response>
        /// <response code="500">Ошибка сервера</response>
        [HttpPost(APIConsts.REGISTER)]
        [ProducesResponseType(typeof(AuthSuccessResult), 200)]
        [ProducesResponseType(409)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<AuthSuccessResultDTO>> Register(AccountCreationDTO accountCreation)
        {
            return Ok(await AuthUseCase.Register(accountCreation));
        }
    }
}
