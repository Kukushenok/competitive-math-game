using CompetitiveBackend.Core.Auth;
using CompetitiveBackend.Core.Objects;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RepositoriesRealisation;
using RepositoriesRealisation.DatabaseObjects;
using RepositoriesRealisation.RepositoriesRealisation;

namespace CompetitiveBackend.Repositories
{
    class AccountRepository : BaseRepository<AccountRepository>, IAccountRepository
    {
        public AccountRepository(IDbContextFactory<BaseDbContext> contextFactory, ILogger<AccountRepository> logger) : base(contextFactory, logger) { }

        public async Task CreateAccount(Account acc, Role accountRole)
        {
            using BaseDbContext context = await GetDbContext();
            try
            {
                await context.Accounts.AddAsync(new AccountModel(acc, accountRole));
                await context.SaveChangesAsync();
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
            AccountModel? Q = await context.Accounts.Where((x) => x.Name == login).FirstOrDefaultAsync();
            if (Q == null)
            {
                _logger.LogInformation($"Attempted to get \"{login}\": failure - no account found");
                throw new Exceptions.MissingDataException($"No account with login \"{login}\"");
            }
            else
            {
                _logger.LogInformation($"Attempted to get \"{login}\": success");
            }
            return Q.ToCoreAccount();
        }

        public async Task<Account> GetAccount(int identifier)
        {
            using BaseDbContext context = await GetDbContext();
            AccountModel? Q = await context.Accounts.FindAsync(identifier);
            if (Q == null)
            {
                _logger.LogInformation($"Attempted to get account with ID {identifier}: failure - no account found");
                throw new Exceptions.MissingDataException($"No account with id {identifier}");
            }
            else
            {
                _logger.LogInformation($"Attempted to get {identifier}: success");
            }
            return Q.ToCoreAccount();
        }
    }
}
