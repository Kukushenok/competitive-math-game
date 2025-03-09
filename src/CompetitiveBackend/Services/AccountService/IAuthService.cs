using CompetitiveBackend.Core.Objects;
using CompetitiveBackend.LogicComponents;
using System.Net;
using System.Security.Claims;
namespace CompetitiveBackend.Services.AccountSevice
{
    public record AuthSuccessResult(string Token, string RoleName,int AccountID)
    {
    }
    public interface IAuthService
    {
        /// <summary>
        /// Попытка входа в аккаунт
        /// </summary>
        /// <param name="login">Имя</param>
        /// <param name="password">Пароль</param>
        /// <returns>Результат авторизации</returns>
        public Task<AuthSuccessResult> LogIn(string login, string password);
        /// <summary>
        /// Создание аккаунта
        /// </summary>
        /// <param name="data">Данные аккаунта</param>
        /// <param name="password">Пароль</param>
        /// <returns></returns>
        public Task Register(Account data, string password);
        /// <summary>
        /// Получить логический сессионый токен
        /// </summary>
        /// <param name="token">Сессионый токен</param>
        /// <returns>Логический сессионый токен</returns>
        public Task<SessionToken> GetSessionToken(string token);
    }
    public class PlayerRoleCreator: IRoleCreator { public Role Create(Account data) => new PlayerRole(); }
    public class SpecificRoleCreator : IRoleCreator
    {
        public Role Create(Account data)
        {
            if (data.Login == "root") return new AdminRole();
            return new PlayerRole();
        }
    }
}
