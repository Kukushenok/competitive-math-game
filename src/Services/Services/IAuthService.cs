using CompetitiveBackend.Core.Auth;
using CompetitiveBackend.Core.Objects;
using CompetitiveBackend.Services.Objects;
namespace CompetitiveBackend.Services
{
    public interface IAuthService
    {
        /// <summary>
        /// Попытка входа в аккаунт.
        /// </summary>
        /// <param name="login">Имя.</param>
        /// <param name="password">Пароль.</param>
        /// <returns>Результат авторизации.</returns>
        Task<AuthSuccessResult> LogIn(string login, string password);

        /// <summary>
        /// Создание аккаунта.
        /// </summary>
        /// <param name="data">Данные аккаунта.</param>
        /// <param name="password">Пароль.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task Register(Account data, string password);

        /// <summary>
        /// Получить логический сессионый токен.
        /// </summary>
        /// <param name="token">Сессионый токен.</param>
        /// <returns>Логический сессионый токен.</returns>
        Task<SessionToken> GetSessionToken(string token);
    }
}
