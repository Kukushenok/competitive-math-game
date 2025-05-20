using CompetitiveBackend.Core.Objects;
using CompetitiveBackend.Repositories;
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
    class PlayerProfileRepository : BaseRepository<PlayerProfileRepository>, IPlayerProfileRepository
    {
        public PlayerProfileRepository(IMongoConnectionCreator contextFactory, ILogger<PlayerProfileRepository> logger) : base(contextFactory, logger)
        {
        }

        public async Task<PlayerProfile> GetPlayerProfile(int accountId)
        {
            using var conn = await GetMongoConnection();
            try
            {
                AccountEntity e = (await conn.Account().FindAsync(x => x.Id == accountId)).Single();
                return new PlayerProfile(e.Username, e.Description, accountId);
            }
            catch(Exception ex)
            {
                _logger.LogError($"Cannot find profile with ID {accountId}");
                throw new CompetitiveBackend.Repositories.Exceptions.MissingDataException($"Cannot find profile with ID {accountId}", ex);
            }
        }

        public async Task<LargeData> GetPlayerProfileImage(int accountId)
        {
            using var conn = await GetMongoConnection();
            try
            {
                AccountEntity e = (await conn.Account().FindAsync(x => x.Id == accountId)).Single();
                return new LargeData(e.ProfileImage);
            }
            catch(Exception ex)
            {
                _logger.LogError($"Cannot find profile with ID {accountId}");
                throw new CompetitiveBackend.Repositories.Exceptions.MissingDataException($"Cannot find profile with ID {accountId}", ex);
            }
        }

        public async Task UpdatePlayerProfile(PlayerProfile p)
        {
            if (p.Id == null) throw new CompetitiveBackend.Repositories.Exceptions.IncorrectOperationException("ID is null");
            using var conn = await GetMongoConnection();
            try
            {
                await conn.Account().UpdateOneAsync(Builders<AccountEntity>.Filter.Eq(x=>x.Id, p.Id), Builders<AccountEntity>.Update.Combine(
                    Builders<AccountEntity>.Update.Set(x => x.Username, p.Name),
                    Builders<AccountEntity>.Update.Set(x => x.Description, p.Description)
                    ));
                await conn.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Cannot find profile with ID {p.Id}");
                throw new CompetitiveBackend.Repositories.Exceptions.MissingDataException($"Cannot find profile with ID {p.Id}", ex);
            }
        }

        public async Task UpdatePlayerProfileImage(int accountId, LargeData data)
        {
            using var conn = await GetMongoConnection();
            try
            {
                await conn.Account().UpdateOneAsync(Builders<AccountEntity>.Filter.Eq(x => x.Id, accountId),
                    Builders<AccountEntity>.Update.Set(x => x.ProfileImage, data.Data)
                    );
                await conn.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Cannot find profile with ID {accountId}");
                throw new CompetitiveBackend.Repositories.Exceptions.MissingDataException($"Cannot find profile with ID {accountId}", ex);
            }
        }
    }
}
