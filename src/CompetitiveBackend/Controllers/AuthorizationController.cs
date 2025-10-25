using CompetitiveBackend.BackendUsage.Objects;
using CompetitiveBackend.BackendUsage.UseCases;
using CompetitiveBackend.Services.Objects;
using Microsoft.AspNetCore.Mvc;

namespace CompetitiveBackend.Controllers
{
    [Route($"{APIConsts.ROOTV1}/{APIConsts.AUTH}/")]
    [ApiController]
    public class AuthorizationController : ControllerBase
    {
        private readonly IAuthUseCase authUseCase;
        public AuthorizationController(IAuthUseCase service)
        {
            authUseCase = service;
        }

        /// <summary>
        /// Логин.
        /// </summary>
        /// <param name="accountLogin">Данные для входа в аккаунт.</param>
        /// <returns>Параметры для входа в аккаунт.</returns>
        /// <response code="200">Успешный вход в аккаунт.</response>
        /// <response code="404">Пароль не подошёл или логин не найден.</response>
        /// <response code="500">Ошибка сервера.</response>
        [HttpPost(APIConsts.LOGIN)]
        [ProducesResponseType(typeof(AuthSuccessResult), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(typeof(ProblemDetails), 500)]
        public async Task<IActionResult> Login([FromBody] AccountLoginDTO accountLogin)
        {
            return Ok(await authUseCase.Login(accountLogin));
        }

        /// <summary>
        /// Регистрация нового аккаунта.
        /// </summary>
        /// <param name="accountCreation">Данные для регистрации.</param>
        /// <returns>Параметры для входа в аккаунт.</returns>
        /// <response code="200">Успешная регистрация.</response>
        /// <response code="409">Такой логин уже занят.</response>
        /// <response code="500">Ошибка сервера.</response>
        [HttpPost(APIConsts.REGISTER)]
        [ProducesResponseType(typeof(AuthSuccessResult), 200)]
        [ProducesResponseType(409)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<AuthSuccessResultDTO>> Register(AccountCreationDTO accountCreation)
        {
            return Ok(await authUseCase.Register(accountCreation));
        }
    }
}
