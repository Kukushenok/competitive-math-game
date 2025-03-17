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
    class AccountRepository : BaseRepository, IAccountRepository
    {
        public AccountRepository(IDbContextFactory<BaseDbContext> contextFactory) : base(contextFactory) { }

        public async Task CreateAccount(Account acc, Role accountRole)
        {
            using BaseDbContext context = await GetDbContext();
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
            using BaseDbContext context = await GetDbContext();
            AccountModel? Q = await context.Accounts.Where((x) => x.Name == login).FirstOrDefaultAsync();
            if (Q == null) throw new Exceptions.MissingDataException($"No account with login {login}");
            return Q.ToCoreAccount();
        }

        public async Task<Account> GetAccount(int identifier)
        {
            using BaseDbContext context = await GetDbContext();
            AccountModel? Q = await context.Accounts.FindAsync(identifier);
            if (Q == null) throw new Exceptions.MissingDataException($"No account with id {identifier}");
            return Q.ToCoreAccount();
        }
    }
}
