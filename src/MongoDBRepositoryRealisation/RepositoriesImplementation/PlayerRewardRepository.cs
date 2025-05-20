using CompetitiveBackend.Core.Objects;
using CompetitiveBackend.Repositories;
using Microsoft.Extensions.Logging;
using MongoDBRepositoryRealisation.RepositoriesImplementation.MongoConnectionSetup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using CompetitiveBackend.Repositories.Exceptions;
using CompetitiveBackend.Core.RewardCondition;

namespace MongoDBRepositoryRealisation.RepositoriesImplementation
{
    class PlayerRewardRepository : BaseRepository<PlayerRewardRepository>, IPlayerRewardRepository
    {
        public PlayerRewardRepository(IMongoConnectionCreator contextFactory, ILogger<PlayerRewardRepository> logger, IAutoIncrementManager manager) : base(contextFactory, logger, manager)
        {
        }
        private async Task EnsureFK(IMongoConnection conn, int playerID, int rewardDescriptionID, int? competitionID = null)
        {
            bool result = true;
            result &= await conn.Account().Find(x => x.Id == playerID).AnyAsync();
            if(competitionID != null) result &= await conn.Competition().Find(x => x.Id == competitionID).AnyAsync();
            result &= await conn.PlayerReward().Find(x => x.Id == rewardDescriptionID).AnyAsync();
            if (!result) throw new CompetitiveBackend.Repositories.Exceptions.FailedOperationException("Foreign key violation");
        }
        public async Task CreateReward(PlayerReward rw)
        {
            using var context = await GetMongoConnection();
            try
            {
                await EnsureFK(context, rw.PlayerID, rw.RewardDescriptionID, rw.CompetitionSource);
                await context.PlayerReward().InsertOneAsync(new PlayerRewardEntity((await NewID(context)), rw));
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Could not create reward");
                throw new CompetitiveBackend.Repositories.Exceptions.FailedOperationException("Could not create reward");
            }
        }

        public async Task DeleteReward(int playerRewardID)
        {
            using var context = await GetMongoConnection();
            try
            {
                await context.PlayerReward().DeleteOneAsync(Builders<PlayerRewardEntity>.Filter.Eq(x=>x.Id, playerRewardID));
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Could not delete reward");
                throw new CompetitiveBackend.Repositories.Exceptions.FailedOperationException("Could not delete reward");
            }
        }

        public async Task<IEnumerable<PlayerReward>> GetAllRewardsOf(int accountId, DataLimiter limiter)
        {
            using var context = await GetMongoConnection();
            try
            {
                IQueryable<PlayerRewardEntity> ent = context.PlayerReward().AsQueryable()
                                                            .Where(x => x.PlayerId == accountId);
                if (!limiter.HasNoLimit)
                {
                    ent = ent.Skip(limiter.FirstIndex).Take(limiter.Partition);
                }
                var pairs = from reward in ent
                        join descr in context.RewardDescriptions().AsQueryable() on reward.RewardDescriptionId equals descr.Id
                        select new { reward, descr };
                return from P in (await pairs.ToListAsync()) select P.reward.Convert(P.descr);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Could not delete reward");
                throw new CompetitiveBackend.Repositories.Exceptions.FailedOperationException("Could not delete reward");
            }
        }

        public async Task GrantRewardsFor(int competitionID)
        {
            using var context = await GetMongoConnection();
            try
            {
                CompetitionEntity? competition = (await context.Competition().FindAsync(x => x.Id == competitionID)).SingleOrDefault();
                if (competition == null)
                {
                    throw new MissingDataException($"No competition with ID {competitionID}");
                }
                else if (competition.HasEnded)
                {
                    throw new FailedOperationException($"Rewards have already been granted for competition {competitionID}");
                }
                await context.Competition().UpdateOneAsync(x => x.Id == competitionID, Builders<CompetitionEntity>.Update.Set(x => x.HasEnded, true));
                PlayerParticipationEntity[] participations = context.PlayerParticipation().AsQueryable()
                                                                                        .Where(x => x.CompetitionId == competitionID)
                                                                                        .OrderByDescending(x => x.Score)
                                                                                        .ThenBy(x => x.LastUpdateTime)
                                                                                        .ToArray();
                List<CompetitionRewardEntity> rewards = await context.CompetitionReward().AsQueryable().Where(x => x.CompetitionId == competitionID)
                                                                                       .ToListAsync();
                foreach (CompetitionRewardEntity reward in rewards)
                {
                    GrantCondition cond = reward.CompetitionCondition.Converted;
                    if (cond is RankGrantCondition rankCondition)
                    {
                        await GrantByCondition(context, reward, participations, rankCondition);
                    }
                    else if (cond is PlaceGrantCondition placeCondition)
                    {
                        await GrantByCondition(context, reward, participations, placeCondition);
                    }
                    else
                    {
                        _logger.LogError($"Unspecified rank condition \"{cond.Type}\", skipping");
                    }
                }
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Could not give rewards to players");
                throw new CompetitiveBackend.Repositories.Exceptions.FailedOperationException("Could not give rewards to players");
            }
        }
        private async Task GrantByCondition(IMongoConnection context, CompetitionRewardEntity model, PlayerParticipationEntity[] participations, PlaceGrantCondition rankCondition)
        {
            if (participations.Length == 0)
                return;
            int competitionID = participations[0].CompetitionId;
            int startIdx = rankCondition.minPlace <= 0 ? 0 : rankCondition.minPlace - 1;
            for (int i = startIdx; i < rankCondition.maxPlace && i < participations.Length; i++)
            {
                // FK is promised by the God.
                await context.PlayerReward().InsertOneAsync(new PlayerRewardEntity(await NewID(context),
                    model.RewardDescriptionId,
                    competitionID,
                    participations[i].AccountId));
            }
        }
        private async Task GrantByCondition(IMongoConnection context, CompetitionRewardEntity model, PlayerParticipationEntity[] participations, RankGrantCondition rankCondition)
        {
            if (participations.Length == 0)
                return;
            int minPlace = (int)Math.Floor(participations.Length * (1.0 - rankCondition.maxRank)) + 1;
            int maxPlace = (int)Math.Ceiling(participations.Length * (1.0 - rankCondition.minRank)) + 1;
            await GrantByCondition(context, model, participations, new PlaceGrantCondition(minPlace, maxPlace));
        }
    }
}
