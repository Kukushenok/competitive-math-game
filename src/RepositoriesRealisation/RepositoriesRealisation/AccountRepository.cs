using CompetitiveBackend.Core.Auth;
using CompetitiveBackend.Core.Objects;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RepositoriesRealisation;
using RepositoriesRealisation.DatabaseObjects;
using RepositoriesRealisation.RepositoriesRealisation;
using System.Data.Common;

namespace CompetitiveBackend.Repositories
{
    class AccountRepository : BaseRepository<AccountRepository>, IAccountRepository
    {
        public AccountRepository(IDbContextFactory<BaseDbContext> contextFactory, ILogger<AccountRepository> logger) : base(contextFactory, logger) { }

        public async Task CreateAccount(Account acc, string passwordHash, Role accountRole)
        {
            using BaseDbContext context = await GetDbContext();
            try
            {
                await context.DoCreateAccount(new AccountModel(acc, passwordHash, accountRole));
                //await context.SaveChangesAsync();
                _logger.LogInformation($"Account \"{acc.Login}\" created successfully");
            }
            catch (Exception ex) when(ex.IsDBException())
            {
                _logger.LogError(ex, "Could not create account");
                throw new Exceptions.FailedOperationException(ex.Message);
            }
        }

        public async Task<Account> GetAccount(string login)
        {
            using BaseDbContext context = await GetDbContext();
            AccountModel? Q = await context.AccountsReadOnly.Where((x) => x.Login == login).FirstOrDefaultAsync();
            if (Q == null)
            {
                _logger.LogInformation($"Attempted to get \"{login}\": failure - no account found");
                throw new Exceptions.MissingDataException($"No account with login \"{login}\"");
            }
            else
            {
                _logger.LogInformation($"Attempted to get \"{login}\": success");
            }
            return Q.ToCoreModel();
        }

        public async Task<Account> GetAccount(int identifier)
        {
            using BaseDbContext context = await GetDbContext();
            AccountModel? Q = await context.AccountsReadOnly.FindAsync(identifier);
            if (Q == null)
            {
                _logger.LogInformation($"Attempted to get account with ID {identifier}: failure - no account found");
                throw new Exceptions.MissingDataException($"No account with id {identifier}");
            }
            else
            {
                _logger.LogInformation($"Attempted to get {identifier}: success");
            }
            return Q.ToCoreModel();
        }

        public async Task<bool> VerifyPassword(string accountLogin, string passwordHash)
        {
            if (accountLogin.Contains('\'') || passwordHash.Contains('\''))
            {
                _logger.LogError("VerifyPassword denied to prevent SQL injection");
                throw new Exceptions.FailedOperationException("VerifyPassword denied to prevent SQL injection");
            }
            using BaseDbContext context = await GetDbContext();
            DbConnection conn = context.Database.GetDbConnection();
            bool result = false;
            await conn.OpenAsync();
            try
            {
                var command = conn.CreateCommand();
                command.CommandText = $"SELECT * FROM check_password_hash('{accountLogin}', '{passwordHash}')";
                result = (await command.ExecuteScalarAsync() as bool?)!.Value;
            }
            catch(Exception ex) when (ex.IsDBException())
            {
                _logger.LogError($"Failed to verify password for {accountLogin}");
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
