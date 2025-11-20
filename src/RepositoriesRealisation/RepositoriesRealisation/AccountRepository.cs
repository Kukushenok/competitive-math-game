using System.Data.Common;
using CompetitiveBackend.Core.Auth;
using CompetitiveBackend.Core.Objects;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RepositoriesRealisation;
using RepositoriesRealisation.Models;
using RepositoriesRealisation.RepositoriesRealisation;

namespace CompetitiveBackend.Repositories
{
    internal sealed class AccountRepository : BaseRepository<AccountRepository>, IAccountRepository
    {
        public AccountRepository(IDbContextFactory<BaseDbContext> contextFactory, ILogger<AccountRepository> logger)
            : base(contextFactory, logger)
        {
        }

        public async Task CreateAccount(Account acc, string passwordHash, Role accountRole)
        {
            using BaseDbContext context = await GetDbContext();
            try
            {
                await context.DoCreateAccount(new AccountModel(acc, passwordHash, accountRole));

                // await context.SaveChangesAsync();
                logger.LogInformation($"Account \"{acc.Login}\" created successfully");
            }
            catch (Exception ex) when (ex.IsDBException())
            {
                logger.LogError(ex, "Could not create account");
                throw new Exceptions.FailedOperationException(ex.Message);
            }
        }

        public async Task<Account> GetAccount(string login)
        {
            using BaseDbContext context = await GetDbContext();
            AccountModel? q = await context.AccountsReadOnly.Where((x) => x.Login == login).FirstOrDefaultAsync();
            if (q == null)
            {
                logger.LogInformation($"Attempted to get \"{login}\": failure - no account found");
                throw new Exceptions.MissingDataException($"No account with login \"{login}\"");
            }
            else
            {
                logger.LogInformation($"Attempted to get \"{login}\": success");
            }

            return q.ToCoreModel();
        }

        public async Task<Account> GetAccount(int identifier)
        {
            using BaseDbContext context = await GetDbContext();
            AccountModel? q = await context.AccountsReadOnly.FindAsync(identifier);
            if (q == null)
            {
                logger.LogInformation($"Attempted to get account with ID {identifier}: failure - no account found");
                throw new Exceptions.MissingDataException($"No account with id {identifier}");
            }
            else
            {
                logger.LogInformation($"Attempted to get {identifier}: success");
            }

            return q.ToCoreModel();
        }

        public async Task<bool> VerifyPassword(string accountLogin, string passwordHash)
        {
            if (accountLogin.Contains('\'') || passwordHash.Contains('\''))
            {
                logger.LogError("VerifyPassword denied to prevent SQL injection");
                throw new Exceptions.FailedOperationException("VerifyPassword denied to prevent SQL injection");
            }

            using BaseDbContext context = await GetDbContext();
            DbConnection conn = context.Database.GetDbConnection();
            bool result = false;
            await conn.OpenAsync();
            try
            {
                DbCommand command = conn.CreateCommand();
                command.CommandText = $"SELECT * FROM check_password_hash('{accountLogin}', '{passwordHash}')";
                result = (await command.ExecuteScalarAsync() as bool?) ?? false;
            }
            catch (Exception ex) when (ex.IsDBException())
            {
                logger.LogError($"Failed to verify password for {accountLogin}");
                throw new Exceptions.FailedOperationException("Failed to verify password");
            }
            finally
            {
                await conn.CloseAsync();
            }

            return result;
        }
    }
}
