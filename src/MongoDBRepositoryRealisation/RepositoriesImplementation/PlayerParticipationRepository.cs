using CompetitiveBackend.Core.Objects;
using CompetitiveBackend.Repositories;
using CompetitiveBackend.Repositories.Exceptions;
using Microsoft.Extensions.Logging;
using Model;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using MongoDBRepositoryRealisation.RepositoriesImplementation.MongoConnectionSetup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoDBRepositoryRealisation.RepositoriesImplementation
{
    class PlayerParticipationRepository : BaseRepository<PlayerParticipationRepository>, IPlayerParticipationRepository
    {
        public PlayerParticipationRepository(IMongoConnectionCreator contextFactory, ILogger<PlayerParticipationRepository> logger) : base(contextFactory, logger)
        {
        }
        private async Task EnsureFK(IMongoConnection conn, int playerID, int compID)
        {
            bool result = true;
            result &= await conn.Competition().Find(x => x.Id == compID).AnyAsync();
            result &= await conn.Account().Find(x => x.Id == playerID).AnyAsync();
            if (!result) throw new CompetitiveBackend.Repositories.Exceptions.FailedOperationException("Foreign key violation");
        }
        public async Task CreateParticipation(PlayerParticipation participation)
        {
            using var context = await GetMongoConnection();
            try
            {
                await EnsureFK(context, participation.PlayerProfileId, participation.CompetitionId);
                await context.PlayerParticipation().InsertOneAsync(participation.Convert());
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Could not add participation");
                throw new FailedOperationException("Could not add participation");
            }
        }
        private FilterDefinition<PlayerParticipationEntity> FilterBy(int accountID, int competitionID)
        {
            return Builders<PlayerParticipationEntity>.Filter.And(
                 Builders<PlayerParticipationEntity>.Filter.Eq(x => x.CompetitionId, competitionID),
                  Builders<PlayerParticipationEntity>.Filter.Eq(x => x.AccountId, accountID)
                );
        }
        public async Task DeleteParticipation(int accountID, int competitionID)
        {
            using var context = await GetMongoConnection();
            try
            {
                await context.PlayerParticipation().DeleteOneAsync(FilterBy(accountID, competitionID));
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Could not delete participation");
                throw new FailedOperationException("Could not delete participation");
            }
        }
        private record PlayerParticipationJoinedEntity(PlayerParticipationEntity Main, CompetitionEntity? Competition, AccountEntity? Account);
        private PlayerParticipation Convert(PlayerParticipationJoinedEntity ent)
        {
            return ent.Main.Convert(ent.Competition, ent.Account);
        }
        private IQueryable<PlayerParticipationJoinedEntity> GetJoinedEntities(IMongoConnection context, IQueryable<PlayerParticipationEntity> main, bool bindPlayer, bool bindCompetition)
        {
            IQueryable<PlayerParticipationJoinedEntity> result = from C in main select new PlayerParticipationJoinedEntity(C, null, null);
            if (bindCompetition)
            {
                result = from player in result
                         join competition in context.Competition().AsQueryable() on player.Main.CompetitionId equals competition.Id
                         select new PlayerParticipationJoinedEntity(player.Main, competition, player.Account);
            }
            if (bindPlayer)
            {
                result = from player in result
                         join account in context.Account().AsQueryable() on player.Main.AccountId equals account.Id
                         select new PlayerParticipationJoinedEntity(player.Main, player.Competition, account);
            }
            return result;
        }
        public async Task<IEnumerable<PlayerParticipation>> GetLeaderboard(int competitionID, DataLimiter limiter, bool bindPlayer = true, bool bindCompetition = false)
        {
            using var context = await GetMongoConnection();
            try
            {
                IQueryable<PlayerParticipationEntity> ent = context.PlayerParticipation().AsQueryable()
                    .Where(x=>x.CompetitionId == competitionID)
                    .OrderByDescending(x => x.Score)
                    .ThenBy(x => x.LastUpdateTime);
                var result = await GetJoinedEntities(context, ent, bindPlayer, bindCompetition).ToListAsync();
                return from R in result select Convert(R);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Could not get leaderboard");
                throw new FailedOperationException("Could not get leaderboard");
            }
            
        }

        public async Task<PlayerParticipation> GetParticipation(int accountID, int competitionID, bool bindPlayer = false, bool bindCompetition = false)
        {
            using var context = await GetMongoConnection();
            try
            {
                IQueryable<PlayerParticipationEntity> ent = context.PlayerParticipation().AsQueryable()
                    .Where(x => x.CompetitionId == competitionID).Where(x => x.AccountId == accountID);
                var result = await GetJoinedEntities(context, ent, bindPlayer, bindCompetition).SingleAsync();
                return Convert(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Could not get player participation");
                throw new MissingDataException("Could not player participation");
            }
        }

        public async Task<IEnumerable<PlayerParticipation>> GetPlayerParticipations(int accountID, DataLimiter limiter, bool bindPlayer = false, bool bindCompetition = true)
        {
            using var context = await GetMongoConnection();
            try
            {
                IQueryable<PlayerParticipationEntity> ent = context.PlayerParticipation().AsQueryable()
                    .Where(x => x.AccountId == accountID);
                var result = await GetJoinedEntities(context, ent, bindPlayer, bindCompetition).ToListAsync();
                return from R in result select Convert(R);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Could not get player participations");
                throw new FailedOperationException("Could not get player participations");
            }
        }

        public async Task UpdateParticipation(PlayerParticipation participation)
        {
            using var context = await GetMongoConnection();
            try
            {
                await context.PlayerParticipation().UpdateOneAsync(FilterBy(participation.PlayerProfileId, participation.CompetitionId),
                    Builders<PlayerParticipationEntity>.Update.Combine(
                         Builders<PlayerParticipationEntity>.Update.Set(x => x.Score, participation.Score),
                         Builders<PlayerParticipationEntity>.Update.Set(x => x.LastUpdateTime, participation.LastUpdateTime)
                        ));
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Could not get leaderboard");
                throw new FailedOperationException("Could not get player participations");
            }
        }
    }
}
