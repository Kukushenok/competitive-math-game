using CompetitiveBackend.Core.Auth;
using CompetitiveBackend.Core.Objects;
using Microsoft.EntityFrameworkCore;
using RepositoriesRealisation;
using RepositoriesRealisation.DatabaseObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompetitiveBackend.Repositories
{
    class AccountRepository : IAccountRepository
    {
        private IDbContextFactory<BaseDbContext> contextFactory;
        public AccountRepository(IDbContextFactory<BaseDbContext> contextFactory)
        {
            this.contextFactory = contextFactory;
        }

        public async Task CreateAccount(Account acc, Role accountRole)
        {
            using BaseDbContext context = await contextFactory.CreateDbContextAsync();
            try
            {
                await context.Accounts.AddAsync(new AccountModel(acc, accountRole));
                await context.SaveChangesAsync();
            }
            catch(OperationCanceledException ex)
            {
                throw new Exceptions.FailedOperationException(ex.Message);
            }
        }

        public async Task<Account> GetAccount(string login)
        {
            using BaseDbContext context = await contextFactory.CreateDbContextAsync();
            AccountModel? Q = await context.Accounts.Where((x) => x.Name == login).FirstAsync();
            if (Q == null) throw new Exceptions.MissingDataException($"No account with login {login}");
            return Q.ToCore();
        }

        public async Task<Account> GetAccount(int identifier)
        {
            using BaseDbContext context = await contextFactory.CreateDbContextAsync();
            AccountModel? Q = await context.Accounts.FindAsync(identifier);
            if (Q == null) throw new Exceptions.MissingDataException($"No account with id {identifier}");
            return Q.ToCore();
        }
    }
}
