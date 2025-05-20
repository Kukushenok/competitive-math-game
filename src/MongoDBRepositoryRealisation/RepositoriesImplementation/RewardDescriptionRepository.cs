using CompetitiveBackend.Core.Objects;
using CompetitiveBackend.Repositories;
using Microsoft.Extensions.Logging;
using Model;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using MongoDBRepositoryRealisation.RepositoriesImplementation.MongoConnectionSetup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MongoDBRepositoryRealisation.RepositoriesImplementation
{
    class RewardDescriptionRepository : BaseRepository<RewardDescriptionRepository>, IRewardDescriptionRepository
    {
        public RewardDescriptionRepository(IMongoConnectionCreator contextFactory, ILogger<RewardDescriptionRepository> logger, IAutoIncrementManager manager) : base(contextFactory, logger, manager)
        {
        }

        public async Task CreateRewardDescription(RewardDescription description)
        {
            using var context = await GetMongoConnection();
            try
            {
                await context.RewardDescriptions().InsertOneAsync(new RewardDescriptionEntity(await NewID(context), description));
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Could not create reward");
                throw new CompetitiveBackend.Repositories.Exceptions.FailedOperationException("Could not create reward", ex);
            }
        }

        public async Task<IEnumerable<RewardDescription>> GetAllRewardDescriptions(DataLimiter limiter)
        {
            using var context = await GetMongoConnection();
            try
            {
                IQueryable<RewardDescriptionEntity> queryable = context.RewardDescriptions().AsQueryable().OrderBy((x) => x.RewardName);
                if (!limiter.HasNoLimit)
                {
                    queryable = queryable.Skip(limiter.FirstIndex).Take(limiter.Partition);
                }
                var models = await queryable.ToListAsync();
                return (from m in models select m.Convert());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Could not fetch reward descriptions");
                throw new CompetitiveBackend.Repositories.Exceptions.FailedOperationException("Could not fetch reward descriptions", ex);
            }
        }

        public async Task<RewardDescription> GetRewardDescription(int rewardID)
        {
            using var context = await GetMongoConnection();
            try
            {
                return (await context.RewardDescriptions().FindAsync(x => x.Id == rewardID)).Single().Convert();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Cannot find reward description with ID {rewardID}");
                throw new CompetitiveBackend.Repositories.Exceptions.MissingDataException($"Cannot find profile with ID {rewardID}", ex);
            }
        }

        public async Task<LargeData> GetRewardGameAsset(int rewardID)
        {
            using var context = await GetMongoConnection();
            try
            {
                return new LargeData((await context.RewardDescriptions().FindAsync(x => x.Id == rewardID)).Single().IngameData);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Cannot find reward description with ID {rewardID}");
                throw new CompetitiveBackend.Repositories.Exceptions.MissingDataException($"Cannot find profile with ID {rewardID}", ex);
            }
        }

        public async Task<LargeData> GetRewardIcon(int rewardID)
        {
            using var context = await GetMongoConnection();
            try
            {
                return new LargeData((await context.RewardDescriptions().FindAsync(x => x.Id == rewardID)).Single().IconImage);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Cannot find reward description with ID {rewardID}");
                throw new CompetitiveBackend.Repositories.Exceptions.MissingDataException($"Cannot find profile with ID {rewardID}", ex);
            }
        }

        public async Task SetRewardGameAsset(int rewardID, LargeData data)
        {
            using var conn = await GetMongoConnection();
            try
            {
                await conn.RewardDescriptions().UpdateOneAsync(Builders<RewardDescriptionEntity>.Filter.Eq(x => x.Id, rewardID),
                        Builders<RewardDescriptionEntity>.Update.Set(x => x.IngameData, data.Data)
                    );
                await conn.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Cannot find reward description with ID {rewardID}");
                throw new CompetitiveBackend.Repositories.Exceptions.MissingDataException($"Cannot find reward description with ID {rewardID}", ex);
            }
        }

        public async Task SetRewardIcon(int rewardID, LargeData data)
        {
            using var conn = await GetMongoConnection();
            try
            {
                await conn.RewardDescriptions().UpdateOneAsync(Builders<RewardDescriptionEntity>.Filter.Eq(x => x.Id, rewardID),
                        Builders<RewardDescriptionEntity>.Update.Set(x => x.IconImage, data.Data)
                    );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Cannot find reward description with ID {rewardID}");
                throw new CompetitiveBackend.Repositories.Exceptions.MissingDataException($"Cannot find reward description with ID {rewardID}", ex);
            }
        }

        public async Task UpdateRewardDescription(RewardDescription description)
        {
            if (description.Id == null) throw new CompetitiveBackend.Repositories.Exceptions.IncorrectOperationException("ID is null");
            using var conn = await GetMongoConnection();
            try
            {
                await conn.RewardDescriptions().UpdateOneAsync(Builders<RewardDescriptionEntity>.Filter.Eq(x => x.Id, description.Id), 
                    Builders<RewardDescriptionEntity>.Update.Combine(
                        Builders<RewardDescriptionEntity>.Update.Set(x => x.RewardName, description.Name),
                        Builders<RewardDescriptionEntity>.Update.Set(x => x.Description, description.Description)
                    ));
                await conn.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Cannot find reward description with ID {description.Id}");
                throw new CompetitiveBackend.Repositories.Exceptions.MissingDataException($"Cannot find reward description with ID {description.Id}", ex);
            }
        }
    }
}
