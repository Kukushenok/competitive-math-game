using CompetitiveBackend.Core.Auth;
using CompetitiveBackend.Core.Objects;
using CompetitiveBackend.Repositories;
using DnsClient;
using Microsoft.Extensions.Logging;
using Model;
using MongoDB.Driver;
using MongoDBRepositoryRealisation.RepositoriesImplementation.MongoConnectionSetup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoDBRepositoryRealisation.RepositoriesImplementation
{
    class AccountRepository : BaseRepository<AccountRepository>, IAccountRepository
    {
        public AccountRepository(IMongoConnectionCreator contextFactory, ILogger<AccountRepository> logger, IAutoIncrementManager manager) : base(contextFactory, logger, manager)
        {
        }
        private async Task CreateIndexes(IMongoConnection conn)
        {
            var indexKeys = Builders<AccountEntity>.IndexKeys.Text(x => x.Login);
            var indexOptions = new CreateIndexOptions { Name = "login", Unique = true};
            var indexModel = new CreateIndexModel<AccountEntity>(indexKeys, indexOptions);

            await conn.Account().Indexes.CreateOneAsync(indexModel);
        }
        public async Task CreateAccount(Account acc, string passwordHash, Role accountRole)
        {
            using var context = await GetMongoConnection();
            try
            {
                await CreateIndexes(context);
                await context.Account().InsertOneAsync(new AccountEntity(await NewID(context), acc, accountRole, passwordHash));
                await context.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                throw new CompetitiveBackend.Repositories.Exceptions.FailedOperationException("Could not create account", ex);
            }
        }

        public async Task<Account> GetAccount(string login)
        {
            using var context = await GetMongoConnection();
            try
            {
                return (await context.Account().FindAsync(x => x.Login == login)).Single().ConvertToAccount();
            } 
            catch(Exception ex)
            {
                throw new CompetitiveBackend.Repositories.Exceptions.MissingDataException("No account found", ex);
            }
        }

        public async Task<Account> GetAccount(int identifier)
        {
            using var context = await GetMongoConnection();
            try
            {
                return (await context.Account().FindAsync(x => x.Id == identifier)).Single().ConvertToAccount();
            }
            catch (Exception ex)
            {
                throw new CompetitiveBackend.Repositories.Exceptions.MissingDataException("No account found", ex);
            }
        }

        public async Task<bool> VerifyPassword(string accountLogin, string passwordHash)
        {
            using var context = await GetMongoConnection();
            try
            {
                var acc = (await context.Account().FindAsync(x => x.Login == accountLogin)).Single();
                return (acc.PasswordHash == passwordHash);
            }
            catch (Exception ex)
            {
                throw new CompetitiveBackend.Repositories.Exceptions.MissingDataException("No account found", ex);
            }
        }
    }
}
