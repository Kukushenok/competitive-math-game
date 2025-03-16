using CompetitiveBackend.Core.Auth;
using CompetitiveBackend.Core.Objects;
namespace CompetitiveBackend.Repositories
{
    public interface ISessionRepository
    {
        /// <summary>
        /// Создаёт токен сессии для аккаунта
        /// </summary>
        /// <param name="acc">Аккаунт</param>
        /// <returns></returns>
        /// <exception cref="CompetitiveBackend.Repositories.Exceptions.IncorrectOperationException">Аккаунт с таким ID не найден</exception>
        public Task<string> CreateSessionFor(int accountID);
        /// <summary>
        /// Получить логическое представление сессионого токена
        /// </summary>
        /// <param name="token">Строковый токен сессии</param>
        /// <returns>Логический токен сессии</returns>
        /// <exception cref="CompetitiveBackend.Repositories.Exceptions.MissingDataException">Аккаунт не найден</exception>
        public Task<SessionToken> GetSessionToken(string token);
    }
}
