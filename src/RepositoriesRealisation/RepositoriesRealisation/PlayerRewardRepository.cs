using CompetitiveBackend.Core.Objects;
using CompetitiveBackend.Repositories;
using CompetitiveBackend.Repositories.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RepositoriesRealisation;
using RepositoriesRealisation.Models;
using RepositoriesRealisation.RepositoriesRealisation;
using RepositoriesRealisation.RewardGranters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompetitiveBackend.Repositories
{
    internal class PlayerRewardRepository : BaseRepository<PlayerRewardRepository>, IPlayerRewardRepository
    {
        private IRewardGranter _granter;
        public PlayerRewardRepository(IDbContextFactory<BaseDbContext> contextFactory, ILogger<PlayerRewardRepository> logger, IRewardGranter granter) : base(contextFactory, logger)
        {
            _granter = granter;
        }

        public async Task CreateReward(PlayerReward rw)
        {
            using var context = await GetDbContext();
            try
            {
                PlayerRewardModel model = new PlayerRewardModel(rw.PlayerID, rw.RewardDescriptionID, rw.CompetitionSource);
                await context.PlayerReward.AddAsync(model);
                await context.SaveChangesAsync();
            }
            catch (Exception ex) when (ex.IsDBException())
            {
                _logger.LogError($"Could not create reward");
                throw new FailedOperationException($"Could not create reward", ex);
            }
        }

        public async Task DeleteReward(int rewardID)
        {
            using var context = await GetDbContext();
            try
            {
                int count = await context.PlayerReward.Where(x => x.Id == rewardID).ExecuteDeleteAsync();
                if (count == 0) throw new MissingDataException($"There is no reward with ID {rewardID}");
                await context.SaveChangesAsync();
            }
            catch(Exception ex) when (ex.IsDBException())
            {
                _logger.LogError($"Could not delete reward {rewardID}");
                throw new FailedOperationException($"Could not delete reward {rewardID}", ex);
            }
        }

        public async Task<IEnumerable<PlayerReward>> GetAllRewardsOf(int accountId, DataLimiter limiter)
        {
            using var context = await GetDbContext();
            IQueryable<PlayerRewardModel> model = context.PlayerReward.Include(x => x.RewardDescription)
                                                                      .Where(x => x.PlayerID == accountId);
            if (!limiter.HasNoLimit)
            {
                model = model.Skip(limiter.FirstIndex).Take(limiter.Partition);
            }
            try
            {
                List<PlayerReward> rewards = await model.Select(x => new PlayerReward(x.PlayerID,
                    x.RewardDescriptionID, x.RewardDescription.Name, x.RewardDescription.Description ?? "", x.CompetitionID,
                    x.CreationDate, x.Id)).ToListAsync();
                return rewards;
            }
            catch(Exception ex) when (ex.IsDBException())
            {
                _logger.LogError($"Could not fetch rewards of {accountId}");
                throw new FailedOperationException($"Could not fetch rewards of {accountId}", ex);
            }
        }

        public async Task GrantRewardsFor(int competitionID)
        {
            using var context = await GetDbContext();
            try
            {
                await _granter.GrantRewards(context, competitionID);
            }
            catch(Exception ex) when (ex.IsDBException())
            {
                _logger.LogError($"Could not grant rewards for competition {competitionID}");
                throw new FailedOperationException($"Could not grant rewards for competition {competitionID}", ex);
            }
        }
    }
}
