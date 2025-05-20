using CompetitiveBackend.Core.Objects;
using CompetitiveBackend.Repositories;
using Microsoft.Extensions.Logging;
using Model;
using MongoDBRepositoryRealisation.RepositoriesImplementation.MongoConnectionSetup;
using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Driver;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver.Linq;

namespace MongoDBRepositoryRealisation.RepositoriesImplementation
{
    class CompetitionRewardRepository : BaseRepository<CompetitionRewardRepository>, ICompetitionRewardRepository
    {
        public CompetitionRewardRepository(IMongoConnectionCreator contextFactory, ILogger<CompetitionRewardRepository> logger, IAutoIncrementManager manager) : base(contextFactory, logger, manager)
        {
        }
        private async Task EnsureFK(IMongoConnection conn, int compID, int rewardDescrID)
        {
            bool result = true;
            result &= await conn.Competition().Find(x => x.Id == compID).AnyAsync();
            result &= await conn.RewardDescriptions().Find(x => x.Id == rewardDescrID).AnyAsync();
            if (!result) throw new CompetitiveBackend.Repositories.Exceptions.FailedOperationException("Foreign key violation");
        }
        public async Task CreateCompetitionReward(CompetitionReward description)
        {
            using var context = await GetMongoConnection();
            try
            {
                var c = description.Convert();
                await EnsureFK(context, c.CompetitionId, c.RewardDescriptionId);
                c.Id = await NewID(context);
                await context.CompetitionReward().InsertOneAsync(c);
                await context.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Could not create competition reward");
                throw new CompetitiveBackend.Repositories.Exceptions.FailedOperationException("Could not create competition reward", ex);
            }
        }

        public async Task<CompetitionReward> GetCompetitionReward(int competitionRewardID)
        {
            using var context = await GetMongoConnection();
            try
            {
                var P = from compReward in context.CompetitionReward().AsQueryable()
                        where compReward.Id == competitionRewardID
                        join rewardDescription in context.RewardDescriptions().AsQueryable() on compReward.RewardDescriptionId equals rewardDescription.Id
                        select new { compReward, rewardDescription };
                var result = await P.SingleAsync();
                return result.compReward.Convert(result.rewardDescription);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Could not get competition reward");
                throw new CompetitiveBackend.Repositories.Exceptions.MissingDataException("Could not get competition reward", ex);
            }
        }

        public async Task<IEnumerable<CompetitionReward>> GetCompetitionRewards(int competitionID)
        {
            using var context = await GetMongoConnection();
            try
            {
                var P = from compReward in context.CompetitionReward().AsQueryable()
                        where compReward.CompetitionId == competitionID
                        join rewardDescription in context.RewardDescriptions().AsQueryable() on compReward.RewardDescriptionId equals rewardDescription.Id
                        select new { compReward, rewardDescription };
                var result = await P.ToListAsync();
                return from F in result select F.compReward.Convert(F.rewardDescription);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Could not get competition rewards");
                throw new CompetitiveBackend.Repositories.Exceptions.MissingDataException("Could not get competition rewards", ex);
            }
        }

        public async Task RemoveCompetitionReward(int competitionRewardID)
        {
            using var context = await GetMongoConnection();
            try
            {
                await context.CompetitionReward().DeleteOneAsync(x=>x.Id == competitionRewardID);
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Could not delete competition reward");
                throw new CompetitiveBackend.Repositories.Exceptions.FailedOperationException("Could not delete competition reward", ex);
            }
        }

        public async Task UpdateCompetitionReward(CompetitionReward description)
        {
            if (description.Id == null) throw new CompetitiveBackend.Repositories.Exceptions.IncorrectOperationException("ID is null");
            using var context = await GetMongoConnection();
            try
            {
                await EnsureFK(context, description.CompetitionID, description.RewardDescriptionID);
                await context.CompetitionReward().ReplaceOneAsync(x => x.Id == description.Id,
                    description.Convert());
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Could not update competition reward");
                throw new CompetitiveBackend.Repositories.Exceptions.FailedOperationException("Could not update competition reward", ex);
            }
        }
    }
}
