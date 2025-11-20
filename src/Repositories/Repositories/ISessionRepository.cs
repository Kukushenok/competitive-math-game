using CompetitiveBackend.Core.Auth;
namespace CompetitiveBackend.Repositories
{
    public interface ISessionRepository
    {
        /// <summary>
        /// Создаёт токен сессии для аккаунта.
        /// </summary>
        /// <param name="accountID">ID аккаунта.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        /// <exception cref="Exceptions.IncorrectOperationException">Аккаунт с таким ID не найден.</exception>
        Task<string> CreateSessionFor(int accountID);

        /// <summary>
        /// Получить логическое представление сессионого токена.
        /// </summary>
        /// <param name="token">Строковый токен сессии.</param>
        /// <returns>Логический токен сессии.</returns>
        /// <exception cref="Exceptions.MissingDataException">Аккаунт не найден.</exception>
        Task<SessionToken> GetSessionToken(string token);
    }
}
