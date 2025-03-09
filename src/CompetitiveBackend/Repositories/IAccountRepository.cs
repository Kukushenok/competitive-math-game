using CompetitiveBackend.Core.Objects;
using CompetitiveBackend.LogicComponents;

namespace CompetitiveBackend.Repositories
{
    public interface IAccountRepository
    {
        /// <summary>
        /// Получить аккаунт по имени
        /// </summary>
        /// <param name="login">Имя аккаунта (логин)</param>
        /// <returns>Аккаунт</returns>
        /// <exception cref="Core.Exceptions.Repository.MissingDataException">Аккаунт с таким именем не найден</exception>
        public Task<Account> GetAccount(string login);
        /// <summary>
        /// Получить аккаунт по ID
        /// </summary>
        /// <param name="identifier">Идентификатор аккаунта</param>
        /// <returns>Аккаунт</returns>
        /// <exception cref="Core.Exceptions.Repository.MissingDataException">Аккаунт с таким ID не найден</exception>
        public Task<Account> GetAccount(int identifier);
        /// <summary>
        /// Добавить аккаунт
        /// </summary>
        /// <param name="acc">Аккаунт</param>
        /// <exception cref="Core.Exceptions.Repository.IncorrectOperationException">Аккаунт с таким именем или ID уже существует</exception>
        public Task CreateAccount(Account acc, Role accountRole);
    }
}
