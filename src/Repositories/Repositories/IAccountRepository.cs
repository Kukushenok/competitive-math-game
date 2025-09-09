using CompetitiveBackend.Core.Auth;
using CompetitiveBackend.Core.Objects;

namespace CompetitiveBackend.Repositories
{
    public interface IAccountRepository
    {
        /// <summary>
        /// Получить аккаунт по имени
        /// </summary>
        /// <param name="login">Имя аккаунта (логин)</param>
        /// <returns>Аккаунт</returns>
        /// <exception cref="CompetitiveBackend.Repositories.Exceptions.MissingDataException">Аккаунт с таким именем не найден</exception>
        public Task<Account> GetAccount(string login);
        /// <summary>
        /// Получить аккаунт по ID
        /// </summary>
        /// <param name="identifier">Идентификатор аккаунта</param>
        /// <returns>Аккаунт</returns>
        /// <exception cref="CompetitiveBackend.Repositories.Exceptions.MissingDataException">Аккаунт с таким ID не найден</exception>
        public Task<Account> GetAccount(int identifier);
        /// <summary>
        /// Добавить аккаунт
        /// </summary>
        /// <param name="acc">Аккаунт</param>
        /// <exception cref="CompetitiveBackend.Repositories.Exceptions.IncorrectOperationException">Аккаунт с таким именем или ID уже существует</exception>
        public Task CreateAccount(Account acc, string passwordHash, Role accountRole);
        public Task<bool> VerifyPassword(string accountLogin, string passwordHash);
    }
}
