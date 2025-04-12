using CompetitiveBackend.Core.Objects;
using CompetitiveBackend.Repositories;
using CompetitiveBackend.Repositories.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RepositoriesRealisation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoriesRealisation.RepositoriesRealisation
{
    internal class RewardDescriptionRepository : BaseRepository<RewardDescriptionRepository>, IRewardDescriptionRepository
    {
        public RewardDescriptionRepository(IDbContextFactory<BaseDbContext> contextFactory, ILogger<RewardDescriptionRepository> logger) : base(contextFactory, logger)
        {
        }

        public async Task CreateRewardDescription(RewardDescription description)
        {
            using BaseDbContext context = await GetDbContext();
            try
            {
                RewardDescriptionModel model = new RewardDescriptionModel(description.Name, description.Description);
                await context.RewardDescription.AddAsync(model);
                await context.SaveChangesAsync();
                _logger.LogInformation("RewardDescription created successfully");
            }
            catch(Exception ex) when (ex is DbUpdateException || ex is OperationCanceledException)
            {
                _logger.LogError(ex, "Could not create reward description");
                throw new FailedOperationException(ex.Message);
            }
        }

        public async Task<IEnumerable<RewardDescription>> GetAllRewardDescriptions(DataLimiter limiter)
        {
            using BaseDbContext context = await GetDbContext();
            IEnumerable<RewardDescriptionModel> models;
            try
            {
                IQueryable<RewardDescriptionModel> queryable = context.RewardDescription.OrderBy((x) => x.Name);
                if (!limiter.HasNoLimit)
                {
                    queryable = queryable.Skip(limiter.FirstIndex).Take(limiter.Partition);
                }
                models = await queryable.ToListAsync();
                return (from m in models select m.ToCoreRewardDescription());
            }
            catch(OperationCanceledException ex)
            {
                _logger.LogError(ex, "Could not fetch reward description");
                throw new FailedOperationException(ex.Message);
            }
        }
        private async Task<RewardDescriptionModel> FetchRewardDescription(int rewardID, BaseDbContext context)
        {
            RewardDescriptionModel? rd = await context.RewardDescription.FindAsync(rewardID);
            if (rd == null)
            {
                _logger.LogError($"Attempted to get {rewardID}: failure - no reward description found");
                throw new MissingDataException("No reward description found ");
            }
            else
            {
                _logger.LogInformation($"Attemptet to get {rewardID} - success");
            }
            return rd!;
        }
        private async Task<T> FetchRewardDescription<T>(int rewardID, DbSet<T> set) where T: class
        {
            T? rd = await set.FindAsync(rewardID);
            if (rd == null)
            {
                _logger.LogError($"Attempted to get {rewardID}: failure - no reward description found");
                throw new MissingDataException("No reward description found ");
            }
            else
            {
                _logger.LogInformation($"Attemptet to get {rewardID} - success");
            }
            return rd!;
        }
        public async Task<RewardDescription> GetRewardDescription(int rewardID)
        {
            using BaseDbContext context = await GetDbContext();
            var rd = await FetchRewardDescription(rewardID, context);
            return rd.ToCoreRewardDescription();
        }

        public async Task<LargeData> GetRewardGameAsset(int rewardID)
        {
            using BaseDbContext context = await GetDbContext();
            var rd = await FetchRewardDescription(rewardID, context.RewardDescriptionInGameData);
            return new LargeData(rd.InGameData ?? Array.Empty<byte>());
        }

        public async Task<LargeData> GetRewardIcon(int rewardID)
        {
            using BaseDbContext context = await GetDbContext();
            var rd = await FetchRewardDescription(rewardID, context.RewardDescriptionIconImages);
            return new LargeData(rd.IconImage ?? Array.Empty<byte>());
        }

        public async Task SetRewardGameAsset(int rewardID, LargeData data)
        {
            using BaseDbContext context = await GetDbContext();
            var rd = await FetchRewardDescription(rewardID, context.RewardDescriptionInGameData);
            rd.InGameData = data.Data;
            try
            {
                context.RewardDescriptionInGameData.Update(rd);
                await context.SaveChangesAsync();
            }
            catch(DbUpdateException exception)
            {
                _logger.LogError(exception, $"Could not update reward game asset for {rewardID}");
                throw new FailedOperationException();
            }
        }

        public async Task SetRewardIcon(int rewardID, LargeData data)
        {
            using BaseDbContext context = await GetDbContext();
            var rd = await FetchRewardDescription(rewardID, context.RewardDescriptionIconImages);
            rd.IconImage = data.Data;
            try
            {
                context.RewardDescriptionIconImages.Update(rd);
                await context.SaveChangesAsync();
            }
            catch (DbUpdateException exception)
            {
                _logger.LogError(exception, $"Could not update reward icon image for {rewardID}");
                throw new FailedOperationException();
            }
        }

        public async Task UpdateRewardDescription(RewardDescription description)
        {
            if (description.Id == null) throw new IncorrectOperationException("Cannot update with ID zero");
            using BaseDbContext context = await GetDbContext();
            var rd = await FetchRewardDescription(description.Id!.Value, context);
            try
            {
                rd.Description = description.Description;
                rd.Name = description.Name;
                context.RewardDescription.Update(rd);
                await context.SaveChangesAsync();
            }
            catch (DbUpdateException exception)
            {
                _logger.LogError(exception, $"Could not update reward name and description for {description.Id}");
                throw new FailedOperationException();
            }
        }
    }
}
